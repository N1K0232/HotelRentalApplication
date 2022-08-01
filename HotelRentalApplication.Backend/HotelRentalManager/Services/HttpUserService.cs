using HotelRentalManager.Authentication.Extensions;
using HotelRentalManager.Contracts;

namespace HotelRentalManager.Services;

internal sealed class HttpUserService : IUserService
{
    private readonly HttpContext httpContext;

    public HttpUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext;
    }

    public Guid GetId()
    {
        return httpContext.User.GetId();
    }

    public string GetUserName()
    {
        return httpContext.User.GetUserName();
    }
}