using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OfferProductService
    {
        private readonly AppDbContext _appDbContext;

        public OfferProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddProductsForOffer(Guid OfferId,List<Guid> ProductIds)
        {
            List<OfferProduct> OfferProducts = new List<OfferProduct>();

            foreach(var id in ProductIds)
            {
                OfferProducts.Add(new OfferProduct()
                {
                    OfferId = OfferId,
                    ProductId = id
                });
            }

            await _appDbContext.OfferProducts.AddRangeAsync(OfferProducts);
            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
