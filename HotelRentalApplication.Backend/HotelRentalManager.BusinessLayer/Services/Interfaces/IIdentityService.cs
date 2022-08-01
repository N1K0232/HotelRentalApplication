using HotelRentalManager.Shared.Requests;
using HotelRentalManager.Shared.Responses;

namespace HotelRentalManager.BusinessLayer.Services.Interfaces;

public interface IIdentityService
{
    Task EnableTwoFactorAuthenticationAsync();
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<RegisterResponse> RegisterPowerUserAsync(SaveUserRequest request);
    Task<RegisterResponse> RegisterUserAsync(SaveUserRequest request);
    Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
}