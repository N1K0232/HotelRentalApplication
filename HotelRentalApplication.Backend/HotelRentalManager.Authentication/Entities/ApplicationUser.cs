using Microsoft.AspNetCore.Identity;

namespace HotelRentalManager.Authentication.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Age { get; set; }

    public string Gender { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDate { get; set; }

    public List<ApplicationUserRole> UserRoles { get; set; }
}