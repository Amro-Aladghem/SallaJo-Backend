using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public record UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsAcceptedToAppear { get; set; }
        public string PrimaryImageLink { get; set; } = null!;
    }
}
