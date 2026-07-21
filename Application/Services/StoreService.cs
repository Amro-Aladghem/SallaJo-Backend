using Application.DTOs.StoreDto;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> UpdateStoreInfo(UpdateStoreInfoDto updateStoreInfoDto,Guid StoreId)
        {
            int NumberOfRowsAffected = await _appDbContext.Stores.ExecuteUpdateAsync(
                sp => sp.SetProperty(p => p.Name, updateStoreInfoDto.Name)
                .SetProperty(p => p.LogoImageUrl, updateStoreInfoDto.LogoImageUrl)
                .SetProperty(p => p.PrimaryColorId, updateStoreInfoDto.PrimaryColorId)
                .SetProperty(p => p.SecondaryColorId, updateStoreInfoDto.SecondaryColorId)
                .SetProperty(p => p.Description, updateStoreInfoDto.Description)
                .SetProperty(p => p.GovernorateId, updateStoreInfoDto.GovernorateId)
                .SetProperty(p => p.PhoneNumber, updateStoreInfoDto.PhoneNumber)
                .SetProperty(p => p.Email, updateStoreInfoDto.Email)
                .SetProperty(p => p.FacebookLink, updateStoreInfoDto.FacebookLink)
                .SetProperty(p => p.InstagramLink, updateStoreInfoDto.InstagramLink)
                .SetProperty(p => p.WelcomeHeaderText, updateStoreInfoDto.WelcomeHeaderText)
                .SetProperty(p => p.CoverStoreImageLink, updateStoreInfoDto.CoverStoreImageLink)
            );

            return NumberOfRowsAffected>0;
        }

        public async Task<Guid> GetStoreIdBySellerId(Guid SellerId)
        {
            Guid StoreId = await _appDbContext.Stores.Where(S => S.SellerId == SellerId)
                .Select(S => S.Id)
                .FirstAsync();

            return StoreId;
        }
    }
}
