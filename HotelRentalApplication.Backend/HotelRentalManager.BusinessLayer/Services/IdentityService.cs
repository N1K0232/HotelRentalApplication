using HotelRentalManager.Authentication;
using HotelRentalManager.Authentication.Entities;
using HotelRentalManager.Authentication.Extensions;
using HotelRentalManager.BusinessLayer.Services.Interfaces;
using HotelRentalManager.BusinessLayer.Settings;
using HotelRentalManager.Shared.Requests;
using HotelRentalManager.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelRentalManager.BusinessLayer.Services;

public sealed class IdentityService : IIdentityService
{
	private readonly RandomNumberGenerator generator;

	private readonly JwtSettings jwtSettings;

	private readonly UserManager<ApplicationUser> userManager;
	private readonly SignInManager<ApplicationUser> signInManager;

	public IdentityService(IOptions<JwtSettings> jwtSettingsOptions, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
	{
		generator = RandomNumberGenerator.Create();

		jwtSettings = jwtSettingsOptions.Value;

		this.userManager = userManager;
		this.signInManager = signInManager;
	}

	public async Task<AuthResponse> LoginAsync(LoginRequest request)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return null;
		}

		var signInResult = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
		if (!signInResult.Succeeded)
		{
			return null;
		}

		var roles = await userManager.GetRolesAsync(user);

		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim(ClaimTypes.GivenName, user.FirstName),
			new Claim(ClaimTypes.Surname, user.LastName),
			new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
			new Claim(CustomClaimTypes.Age, user.Age.ToString()),
			new Claim(ClaimTypes.Gender, user.Gender),
			new Claim(CustomClaimTypes.City, user.City),
			new Claim(ClaimTypes.Country, user.Country),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
		}.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var response = CreateToken(claims);
		await SaveRefreshTokenAsync(user, response.RefreshToken);
		return response;
	}
	public Task EnableTwoFactorAuthenticationAsync() => Task.CompletedTask;
	public async Task<RegisterResponse> RegisterUserAsync(SaveUserRequest request)
	{
		var result = await RegisterAsync(request);

		if (result.Succeeded)
		{
			var user = await userManager.FindByNameAsync(request.UserName);
			result = await userManager.AddToRoleAsync(user, RoleNames.User);
		}

		return new(result.Succeeded, result.Errors.Select(e => e.Description));
	}
	public async Task<RegisterResponse> RegisterPowerUserAsync(SaveUserRequest request)
	{
		var result = await RegisterAsync(request);

		if (result.Succeeded)
		{
			var user = await userManager.FindByIdAsync(request.UserName);
			result = await userManager.AddToRoleAsync(user, RoleNames.PowerUser);
		}

		return new(result.Succeeded, result.Errors.Select(e => e.Description));
	}
	public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
	{
		var user = ValidateAccessToken(request.AccessToken);
		if (user != null)
		{
			var userId = user.GetId();
			var dbUser = await userManager.FindByIdAsync(userId.ToString());

			if (dbUser?.RefreshToken == null || dbUser?.RefreshTokenExpirationDate < DateTime.UtcNow || dbUser?.RefreshToken != request.RefreshToken)
			{
				return null;
			}

			var response = CreateToken(user.Claims);
			await SaveRefreshTokenAsync(dbUser, response.RefreshToken);
			return response;
		}

		return null;
	}
	public async Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return new(false, new List<string> { "User not found" });
		}

		var result = await userManager.ChangePasswordAsync(user, user.PasswordHash, request.NewPassword);
		return new(result.Succeeded, result.Errors.Select(e => e.Description));
	}

	private AuthResponse CreateToken(IEnumerable<Claim> claims)
	{
		string accessToken = GetAccessToken(claims);
		string refreshToken = GetRefreshToken();
		return new(accessToken, refreshToken);
	}
	private string GetAccessToken(IEnumerable<Claim> claims)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);
		SymmetricSecurityKey symmetricSecurityKey = new(bytes);
		SigningCredentials signInCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

		DateTime notBefore = DateTime.UtcNow;
		DateTime expire = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

		JwtSecurityToken jwtSecurityToken = new(jwtSettings.Issuer, jwtSettings.Audience, claims,
			notBefore, expire, signInCredentials);

		JwtSecurityTokenHandler handler = new();
		return handler.WriteToken(jwtSecurityToken);
	}
	private string GetRefreshToken()
	{
		byte[] randomNumber = new byte[256];
		generator.GetBytes(randomNumber);
		generator.Dispose();
		return Convert.ToBase64String(randomNumber);
	}
	private ClaimsPrincipal ValidateAccessToken(string accessToken)
	{
		var parameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,
			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,
			ValidateLifetime = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
			RequireExpirationTime = true,
			ClockSkew = TimeSpan.Zero
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		try
		{
			var user = tokenHandler.ValidateToken(accessToken, parameters, out var securityToken);
			if (securityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg == SecurityAlgorithms.HmacSha256)
			{
				return user;
			}
		}
		catch
		{
		}

		return null;
	}
	private async Task<IdentityResult> RegisterAsync(SaveUserRequest request)
	{
		var user = new ApplicationUser
		{
			FirstName = request.FirstName,
			LastName = request.LastName,
			DateOfBirth = request.DateOfBirth,
			Age = Convert.ToInt32((DateTime.UtcNow.Date - request.DateOfBirth).TotalDays / 365),
			Gender = request.Gender,
			City = request.City,
			Country = request.Country,
			Email = request.Email,
			UserName = request.UserName
		};

		var identityResult = await userManager.CreateAsync(user, request.Password);
		return identityResult;
	}
	private async Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken)
	{
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes);
		await userManager.UpdateAsync(user);
	}
}