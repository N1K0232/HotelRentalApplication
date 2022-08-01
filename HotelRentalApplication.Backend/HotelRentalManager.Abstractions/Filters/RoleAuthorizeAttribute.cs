using Microsoft.AspNetCore.Authorization;

namespace HotelRentalManager.Abstractions.Filters;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RoleAuthorizeAttribute : AuthorizeAttribute
{
	public RoleAuthorizeAttribute(params string[] roles)
	{
		Roles = string.Join(",", roles);
	}
}