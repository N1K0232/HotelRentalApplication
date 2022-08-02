using HotelRentalManager.Abstractions.Controllers;
using HotelRentalManager.Abstractions.Filters;
using HotelRentalManager.Authentication;
using HotelRentalManager.Authentication.Extensions;
using HotelRentalManager.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelRentalManager.Controllers;

public class MeController : ApiController
{
    [HttpGet("GetMe")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.PowerUser, RoleNames.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetMe()
    {
        var user = new User
        {
            Id = User.GetId(),
            FirstName = User.GetFirstName(),
            LastName = User.GetLastName(),
            DateOfBirth = User.GetDateOfBirth(),
            Age = User.GetAge(),
            Gender = User.GetGender(),
            City = User.GetCity(),
            Country = User.GetCountry(),
            PhoneNumber = User.GetPhoneNumber(),
            Email = User.GetEmail(),
            UserName = User.GetUserName()
        };

        return Ok(user);
    }
}