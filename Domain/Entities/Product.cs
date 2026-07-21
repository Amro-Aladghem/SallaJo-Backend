namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid StoreId { get; set; }
    public string Description { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public bool? IsAcceptedToAppear { get; set; }
    public int? NumberOfOrders { get; set; }
    public string PrimaryImageLink { get; set; } = null!;
    public bool? IsDeleted { get; set; }
    public int SequenceNumber { get; set; }

    public Store Store { get; set; } = null!;
    public IList<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();

}
