using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public record ProductSimpleInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string PrimaryImageLink { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
