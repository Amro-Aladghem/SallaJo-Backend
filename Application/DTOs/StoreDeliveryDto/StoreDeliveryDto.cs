using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StoreDeliveryDto
{
    public record StoreDeliveryDto
    {
        public int GovernorateId { get; set; }
        public bool IsDelivery { get; set; }
        public decimal? Amount { get; set; }
    }
}
