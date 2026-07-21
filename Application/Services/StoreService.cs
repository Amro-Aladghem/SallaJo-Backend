using Application.DTOs.StoreDto;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StoreService
    {
        private readonly AppDbContext _appDbContext;

        public StoreService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<InitialStoreInfoDto?> AddInitialStoreInfo(AddInitialStoreInfoDto addInitialStoreInfoDto)
        {
            Store store = new Store()
            {
                SellerId = addInitialStoreInfoDto.SellerId,
                Description = addInitialStoreInfoDto.Description,
                Name = addInitialStoreInfoDto.Name,
                GovernorateId = addInitialStoreInfoDto.GovernorateId,
                CountryId = 1,
                IsCompletedStoreProfile = false,
                LogoImageUrl = addInitialStoreInfoDto.LogoImageUrl
            };

            await _appDbContext.Stores.AddAsync(store);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                return null;

            return new InitialStoreInfoDto()
            {
                StoreId = store.Id,
                Description = store.Description,
                Name = store.Name,
                GovernorateId = store.GovernorateId.Value,
                LogoImageUrl = store.LogoImageUrl,
                SellerId = store.SellerId,
            };
        }
    }
}
