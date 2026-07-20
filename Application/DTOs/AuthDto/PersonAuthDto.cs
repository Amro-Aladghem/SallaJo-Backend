using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AuthDto
{
    public record PersonAuthDto
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
