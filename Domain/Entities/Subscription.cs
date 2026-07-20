namespace Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }

    public Store Store { get; set; } = null!;
}
