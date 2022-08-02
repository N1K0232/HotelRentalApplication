namespace HotelRentalManager.DataAccessLayer.Entities.Common;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    public Guid Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }
}