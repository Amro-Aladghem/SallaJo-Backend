namespace Application.DTOs.StoreDto
{
    public class ActivateStoreByAdminDto
    {
        public Guid StoreId { get; set; }
        public bool IsHasDelivery { get; set; }
        public string slug { get; set; } = null!;
    }
}
