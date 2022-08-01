namespace HotelRentalManager.Contracts;

public interface IUserService
{
    Guid GetId();

    string GetUserName();
}