using Microsoft.AspNetCore.Identity;

namespace HotelRentalManager.Authentication.Entities;

public sealed class ApplicationRole : IdentityRole<Guid>
{
	public ApplicationRole() : base()
	{
	}

	public ApplicationRole(string roleName) : base(roleName)
	{
	}

	public List<ApplicationUserRole> UserRoles { get; set; }
}