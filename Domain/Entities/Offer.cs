namespace Domain.Entities;

public class Offer
{
    public Guid Id { get; set; }
    public Guid StoreId { get; set; }
    public string? ImageLink { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? OfferPrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsActive { get; set; }
    public List<OfferProduct> OfferProducts { get; set; }
    public Store Store { get; set; }    
}
