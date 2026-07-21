using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfferProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OfferId { get; set; }

        public Product product { get; set; }
        public Offer offer { get; set; }
    }
}
