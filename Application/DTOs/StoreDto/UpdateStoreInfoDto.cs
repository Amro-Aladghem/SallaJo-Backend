using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StoreDto
{
    public class UpdateStoreInfoDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LogoImageUrl { get; set; }

        [Required]
        public int PrimaryColorId { get; set; }

        [Required]
        public int SecondaryColorId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int GovernorateId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }

        [Required]
        public string? WelcomeHeaderText { get; set; }

        [Required]
        public string? CoverStoreImageLink { get; set; }

        public bool IsAcceptedToShowStoke { get; set; }
    }
}
