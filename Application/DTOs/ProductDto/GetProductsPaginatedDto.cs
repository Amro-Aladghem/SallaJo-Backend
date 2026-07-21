using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public class GetProductsPaginatedDto
    {
        public List<ProductSimpleInfoDto> Products { get; set; }
        public int? LastSequenceProductNumber { get; set; }
    }
}
