namespace HotelRentalManager.DataAccessLayer.Entities.Common;

public abstract class DeletableEntity : BaseEntity
{
	protected DeletableEntity()
	{
	}

	public bool IsDeleted { get; set; }

	public DateTime? DeletedDate { get; set; }
}