namespace Domain.Entities;

public class ProductImage
{
    public Guid Id { get; set; }
    public string ImageLink { get; set; } = null!;
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

}
