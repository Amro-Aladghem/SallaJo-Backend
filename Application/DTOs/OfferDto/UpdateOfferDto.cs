using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OfferDto
{
    public record UpdateOfferDto
    {
        public string? ImageLink { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? OfferPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
