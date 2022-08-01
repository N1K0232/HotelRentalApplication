using HotelRentalManager.Authentication.Entities;
using HotelRentalManager.BusinessLayer.Services.Interfaces;
using HotelRentalManager.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelRentalManager.BusinessLayer.Services;

public sealed class AuthenticatedService : IAuthenticatedService
{
	private readonly UserManager<ApplicationUser> userManager;

	public AuthenticatedService(UserManager<ApplicationUser> userManager)
	{
		this.userManager = userManager;
	}

	public async Task<User> GetAsync(Guid id)
	{
		if (id == Guid.Empty)
		{
			return null;
		}

		string userId = Convert.ToString(id);

		var dbUser = await userManager.FindByIdAsync(userId);

		var user = new User
		{
			FirstName = dbUser.FirstName,
			LastName = dbUser.LastName,
			DateOfBirth = dbUser.DateOfBirth,
			Age = dbUser.Age,
			Gender = dbUser.Gender,
			City = dbUser.City,
			Country = dbUser.Country,
			PhoneNumber = dbUser.PhoneNumber,
			Email = dbUser.Email,
			UserName = dbUser.UserName
		};

		return user;
	}
}