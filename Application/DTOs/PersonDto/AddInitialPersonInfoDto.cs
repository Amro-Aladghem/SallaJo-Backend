using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PersonDto
{
    public record AddInitialPersonInfoDto
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public int GovernorateId { get; set; }
    }
}
