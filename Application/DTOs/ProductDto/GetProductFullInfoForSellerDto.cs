using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public class GetProductFullInfoForSellerDto
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string PrimaryImageLink { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int? SequenceProductNumber = null;

        public decimal? AmountOfDiscount = null;

        public int Stock;
        public bool? IsDeleted { get; set; }
        public bool? IsAcceptedToAppear { get; set; }

        public List<ProductImageDto> Images { get; set; }
    }
}
