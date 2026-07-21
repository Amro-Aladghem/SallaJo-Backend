namespace Application.DTOs.StoreDto
{
    public class AddInitialStoreInfoDto
    {
        public string? LogoImageUrl { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int GovernorateId { get; set; }
        public Guid SellerId { get; set; }
    }
}
