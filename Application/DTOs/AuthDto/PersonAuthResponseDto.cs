using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDto
{
    public record PersonAuthResponseDto
    {
        public string Id { get; set; }
        public string FullName  { get; set; }
        public string ImageUrl { get; set; }
        public string IsActive { get; set; }
        public int UserTypeId { get; set; }
    }
}
