using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StoreDto
{
    public class StoreInfoForSellerDto
    {
        public string Name { get; set; }

        public string LogoImageUrl { get; set; }

        public int PrimaryColorId { get; set; }

        public int SecondaryColorId { get; set; }

        public string Description { get; set; }

        public int GovernorateId { get; set; }

        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }

        public string? WelcomeHeaderText { get; set; }
        public string? CoverStoreImageLink { get; set; }

        public bool? IsActivatedStore { get; set; }
        public int? CountryId { get; set; }
        public string? Slug { get; set; } = null!;

        public bool IsCompletedStoreProfile { get; set; }

        public bool IsAcceptedToShowStoke { get; set; }
        public bool IsHasDelivery { get; set; }
    }
}
