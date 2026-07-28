using Application.DTOs.StoreDeliveryDto;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> SetStoreDeliveries(Guid storeId, List<StoreDeliveryDto> storeDeliveryDtos)
        {
            List<StoreDelivery> storeDeliveries = new List<StoreDelivery>();

            storeDeliveryDtos.ForEach(s =>
            {
                storeDeliveries.Add(new StoreDelivery()
                {
                    StoreId = storeId,
                    GovernorateId = s.GovernorateId,
                    Amount = s.Amount,
                    IsDelivered = s.IsDelivery,
                });
            });

            await _appDbContext.StoreDeliveries.AddRangeAsync(storeDeliveries);

            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
