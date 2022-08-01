using Microsoft.AspNetCore.Identity;

namespace HotelRentalManager.Authentication.Entities;

public sealed class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; }

    public ApplicationRole Role { get; set; }
}