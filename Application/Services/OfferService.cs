using Application.DTOs.OfferDto;
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
    public class OfferService
    {
        private readonly AppDbContext _appDbContext;
        private readonly OfferProductService _offerProductService;

        public OfferService(AppDbContext appDbContext)
        { 
            _appDbContext = appDbContext; 
        }

        private async Task<Guid?> AddOffer(Guid StoreId,AddOfferDto addOfferDto)
        {
            Offer offer = new Offer()
            {
                ImageLink = addOfferDto.ImageLink,
                Title = addOfferDto.Title,
                Description = addOfferDto.Description,
                OfferPrice = addOfferDto.OfferPrice,
                ProductsStringIds = string.Join(",", addOfferDto.ProductsIds),
                StartDate = addOfferDto.StartDate,
                EndDate = addOfferDto.EndDate
            };

            await _appDbContext.Offers.AddAsync(offer);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                return null;

            return offer.Id;
        }

        public async Task<bool> HandleCreateOffer(Guid StoreId, AddOfferDto addOfferDto)
        {
            
            await using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid? OfferId = await AddOffer(StoreId, addOfferDto);

                    if (OfferId == null)
                        throw new Exception("Failed to create offer");

                    bool isDone = await _offerProductService.AddProductsForOffer(OfferId.Value, addOfferDto.ProductsIds);

                    if (!isDone)
                        throw new Exception("Failed to create offerProducts");

                    await transaction.CommitAsync();
                    return true;
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> ToggleOfferStatus(Guid offerId)
        {

            int NumberOfRowsAffected = await _appDbContext.Offers.Where(o => o.Id == offerId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.IsActive, p => !p.IsActive));

            return NumberOfRowsAffected > 0;
        }

        public async Task<List<OfferCustomerInfoDto>> GetOffersForCustomer(Guid storeId)
        {
            return await _appDbContext.Offers
                .Where(o => o.StoreId == storeId)
                .OrderByDescending(o => o.Id)
                .Take(3)
                .Select(o => new OfferCustomerInfoDto
                {
                    Id = o.Id,
                    ImageLink = o.ImageLink,
                    Title = o.Title,
                    Description = o.Description,
                    OfferPrice = o.OfferPrice,
                    ProductsStringIds = o.ProductsStringIds,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    IsActive = o.IsActive
                })
                .ToListAsync();
        }
    }
}
