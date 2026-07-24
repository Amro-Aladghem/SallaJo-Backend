using Application.DTOs.OfferDto;
using Application.DTOs.ProductDto;
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

        public OfferService(AppDbContext appDbContext, OfferProductService offerProductService)
        { 
            _appDbContext = appDbContext;
            _offerProductService = offerProductService;
        }

        private async Task<Guid?> AddOffer(Guid StoreId,AddOfferDto addOfferDto)
        {
            Offer offer = new Offer()
            {
                ImageLink = addOfferDto.ImageLink,
                Title = addOfferDto.Title,
                Description = addOfferDto.Description,
                OfferPrice = addOfferDto.OfferPrice,
                StartDate = addOfferDto.StartDate,
                EndDate = addOfferDto.EndDate,
                StoreId = StoreId,
                IsActive=true,
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
                    
                    if(addOfferDto.ProductsIds.Count>0)
                    {
                        bool isDone = await _offerProductService.AddProductsForOffer(OfferId.Value, addOfferDto.ProductsIds);

                        if (!isDone)
                            throw new Exception("Failed to create offerProducts");
                    }

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

        public async Task<List<OfferCustomerInfoDto>> GetOffersForCustomer(string slug)
        {
            return await _appDbContext.Offers
                .Where(o => o.Store.Slug == slug)
                .OrderByDescending(o => o.Id)
                .Take(10)
                .Select(o => new OfferCustomerInfoDto
                {
                    Id = o.Id,
                    ImageLink = o.ImageLink,
                    Title = o.Title,
                    Description = o.Description,
                    OfferPrice = o.OfferPrice,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    IsActive = o.IsActive,
                    Products = o.OfferProducts.Select(op => new ProductSimpleInfoDto
                    {
                        Id = op.product.Id,
                        Description = op.product.Description,
                        Name = op.product.Name,
                        Price = op.product.Price,
                        PrimaryImageLink = op.product.PrimaryImageLink,
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateOffer(Guid offerId, UpdateOfferDto updateOfferDto)
        {
            int NumberOfRowsAffected = await _appDbContext.Offers
                .Where(o => o.Id == offerId)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(p => p.ImageLink, updateOfferDto.ImageLink)
                    .SetProperty(p => p.Title, updateOfferDto.Title)
                    .SetProperty(p => p.Description, updateOfferDto.Description)
                    .SetProperty(p => p.OfferPrice, updateOfferDto.OfferPrice)
                    .SetProperty(p => p.StartDate, updateOfferDto.StartDate)
                    .SetProperty(p => p.EndDate, updateOfferDto.EndDate));

            return NumberOfRowsAffected > 0;
        }

        public async Task<List<OfferFullInfoDto>> GetOffersForSeller(Guid storeId)
        {
            return await _appDbContext.Offers
                .Where(o => o.StoreId == storeId)
                .OrderByDescending(o => o.Id)
                .Select(o => new OfferFullInfoDto
                {
                    Id = o.Id,
                    StoreId = o.StoreId,
                    ImageLink = o.ImageLink,
                    Title = o.Title,
                    Description = o.Description,
                    OfferPrice = o.OfferPrice,
                    StartDate = o.StartDate,
                    EndDate = o.EndDate,
                    IsActive = o.IsActive,
                    OfferProducts = o.OfferProducts.Select(op => new ProductSimpleInfoDto
                    {
                        Id = op.product.Id,
                        Description = op.product.Description,
                        Name = op.product.Name,
                        Price = op.product.Price,
                        PrimaryImageLink = op.product.PrimaryImageLink,
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
