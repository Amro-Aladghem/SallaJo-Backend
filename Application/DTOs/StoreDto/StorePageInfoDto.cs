using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StoreDto
{
    public class StorePageInfoDto
    {
        public string Name { get; set; }
        public string LogoImageUrl { get; set; }
        public string Description { get; set; }
        public int GovernorateId { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FacebookLink { get; set; }
        public string? InstagramLink { get; set; }
        public int? CountryId { get; set; }
        public string? Slug { get; set; }
        public bool? IsActivatedStore { get; set; }
    }
}

