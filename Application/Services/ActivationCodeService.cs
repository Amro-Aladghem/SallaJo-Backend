using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ActivationCodeService
    {
        private readonly AppDbContext _appDbContext;

        public  ActivationCodeService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> IsActivationCodeAccepted(Guid StoreId, string ActivationCode)
        {
            bool IsAccepted = await _appDbContext.ActivationCodes.Where(
                a=>a.StoreId==StoreId && a.Code==ActivationCode)
                .AnyAsync();

            return IsAccepted;
        }
    }
}
