using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProductDto
{
    public record UpdateImageDto
    {
        public Guid OldImageId { get; set; }
        public string NewImageLink { get; set; }
        public bool IsPrimaryImage { get; set; }
    }
}
