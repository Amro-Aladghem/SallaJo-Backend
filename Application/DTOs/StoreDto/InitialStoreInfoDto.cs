using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StoreDto
{
    public class InitialStoreInfoDto
    {
       public Guid StoreId { get; set; }
       public string? LogoImageUrl { get; set; } = null!;
       public string Name { get; set; } = null!;
       public string? Description { get; set; }
       public int GovernorateId { get; set; }
       public Guid SellerId { get; set; }
    }
}
