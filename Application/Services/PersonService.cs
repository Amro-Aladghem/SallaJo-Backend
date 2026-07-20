using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonService
    {
        private readonly AppDbContext _appDbContext;

        public PersonService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
