namespace Domain.Entities;

public class ActivationCode
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public Guid StoreId { get; set; }
    public bool? IsActive { get; set; }

    public Store Store { get; set; } = null!;
}
