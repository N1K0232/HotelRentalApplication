using HotelRentalManager.Shared.Models;

namespace HotelRentalManager.BusinessLayer.Services.Interfaces;

public interface IAuthenticatedService
{
    Task<User> GetAsync(Guid id);
}