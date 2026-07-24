using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public class ProductFullInfoForCustomerDto
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreImageLink { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string PrimaryImageLink { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int? SequenceProductNumber = null;

        public int Stoke = 0;

        public bool IsAcceptToShowTheStock;

        public decimal? AmountOfDiscount  {get;set;}
        public List<ProductImageDto> Images { get; set; }
    }
}
