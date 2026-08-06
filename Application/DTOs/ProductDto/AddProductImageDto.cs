using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public record AddProductImageDto
    {
        public string ImageUrl { get; set; }
    }
}