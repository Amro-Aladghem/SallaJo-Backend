using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.OfferDto
{
    public record AddOfferDto
    {
        public string? ImageLink { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? OfferPrice { get; set; }
        public List<Guid> ProductsIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
