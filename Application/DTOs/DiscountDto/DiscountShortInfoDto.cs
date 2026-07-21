using Application.DTOs.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DiscountDto
{
    public record DiscountShortInfoDto
    {
        public decimal? DiscountAmount { get; set; }
        public int? LeastAmountNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProductSimpleInfoDto Product { get; set; }
    }
}
