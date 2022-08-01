using HotelRentalManager.Authentication.Entities;
using HotelRentalManager.Authentication.Extensions;
using HotelRentalManager.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HotelRentalManager.Authorization.Handlers;

public sealed class UserActiveHandler : AuthorizationHandler<UserActiveRequirement>
{
    private readonly UserManager<ApplicationUser> userManager;

    public UserActiveHandler(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserActiveRequirement requirement)
    {
        var userId = context.User.GetId();

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user != null && user.LockoutEnd.GetValueOrDefault() <= DateTimeOffset.UtcNow)
        {
            context.Succeed(requirement);
        }
    }
}