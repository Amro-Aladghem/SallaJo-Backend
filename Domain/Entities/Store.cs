namespace Domain.Entities;

public class Store
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string Name { get; set; } = null!;
    public string? LogoImageUrl { get; set; } = null!;
    public int? PrimaryColorId { get; set; }
    public int? SecondaryColorId { get; set; }
    public string? Description { get; set; }
    public bool? IsActivatedStore { get; set; }
    public int? CountryId { get; set; }
    public int? GovernorateId { get; set; }
    public int? NumberOfOrders { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? FacebookLink { get; set; }
    public string? InstagramLink { get; set; }
    public string? WelcomeHeaderText { get; set; } = null!;
    public string? CoverStoreImageLink { get; set; }
    public string? Slug { get; set; } = null!;
    public bool IsCompletedStoreProfile { get; set; }
    public bool IsAcceptedToShowStoke { get; set; }
    public bool IsHasDelivery { get; set; }
    public int? ContactTypeId { get; set; }

    public Color PrimaryColor { get; set; }
    public Color SecondaryColor { get; set; }
    public Country? Country { get; set; }
    public Governorate? Governorate { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public ICollection<ActivationCode> ActivationCodes { get; set; } = new List<ActivationCode>();
    public Seller Seller { get; set; }
    public ContactType ContactType { get; set; }
}
