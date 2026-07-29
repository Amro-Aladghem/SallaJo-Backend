using Domain.Entities;
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

        public async Task<string?> CreateActivationCodeForStore(Guid StoreId)
        {
            string code = Guid.NewGuid().ToString();

            ActivationCode activationCode = new ActivationCode()
            {
                Code = code,
                IsActive = true,
                StoreId = StoreId
            };

            await _appDbContext.ActivationCodes.AddAsync(activationCode);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                return null;

            return activationCode.Code;
        }
    }
}
