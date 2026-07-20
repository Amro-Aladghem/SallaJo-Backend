using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDto
{
    public record TokenDto
    {
        public string AuthToken { get; set; }
        public string ReffreshToken { get; set; }
    }
}
