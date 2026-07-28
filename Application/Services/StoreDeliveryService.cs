using Application.DTOs.StoreDeliveryDto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StoreDeliveryService
    {
        private readonly AppDbContext _appDbContext;

        public StoreDeliveryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<StoreDeliveryDto>> GetStoreDeliveries(string Slug)
        {
            List<StoreDeliveryDto> storeDeliveryDtos = await _appDbContext.StoreDeliveries
                .Where(S => S.Store.Slug==Slug)
                .Select(S => new StoreDeliveryDto()
                {
                    GovernorateId = S.GovernorateId,
                    IsDelivery = S.IsDelivered,
                    Amount = S.Amount,
                })
                .ToListAsync();

            return storeDeliveryDtos;
        }

    }
}
