using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public record GetProductsPaginatedRequestDto
    {
        public int? LastSequenceProductNumber { get; set; }
        public int Limit { get; set; }
    }
}
