using Application.DTOs.OfferDto;
using Domain.Entities;
using Infrastructure.Data;
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

        public OfferService(AppDbContext appDbContext)
        { 
            _appDbContext = appDbContext; 
        }

        public async Task<bool> AddOffer(Guid StoreId,AddOfferDto addOfferDto)
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

            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
