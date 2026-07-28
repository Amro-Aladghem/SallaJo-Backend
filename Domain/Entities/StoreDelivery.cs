using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StoreDelivery
    {
        public Guid Id { get; set; }    
        public int GovernorateId { get; set; }
        public Guid StoreId { get; set; }
        public bool IsDelivered { get; set; }
        public decimal? Amount { get; set; }

        public Governorate Governorate { get; set; }
        public Store Store { get; set; }
    }
}
