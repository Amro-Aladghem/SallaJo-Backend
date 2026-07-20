namespace Domain.Entities;

public class Discount
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
    public decimal? DiscountAmount { get; set; }
    public int? LeastAmountNumber { get; set; }

    public Product Product { get; set; } = null!;
}
