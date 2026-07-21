using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DiscountDto
{
    public record AddDiscountDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? LeastAmountNumber { get; set; }
    }
}
