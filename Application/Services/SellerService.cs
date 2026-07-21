using Application.DTOs.SellerDto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SellerService
    {
        private readonly AppDbContext _appDbContext;

        public SellerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<SellerAuthInfoDto?> GetSellerAuthInfoByPersonId(Guid PersonId)
        {
            SellerAuthInfoDto? sellerAuthInfoDto = await _appDbContext.Sellers
                .Where(S => S.PersonId == PersonId)
                .Select(S=>new SellerAuthInfoDto()
                {
                    Id= S.Id,
                    SellerRoleId=S.SellerId,
                    PersonId= S.PersonId
                })
                .SingleOrDefaultAsync();

            return sellerAuthInfoDto;
        }
    }
}
