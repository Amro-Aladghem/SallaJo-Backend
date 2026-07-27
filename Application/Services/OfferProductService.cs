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
        private readonly ProductService _productService;
        public OfferProductService(AppDbContext appDbContext, ProductService productService)
        {
            _appDbContext = appDbContext;
            _productService = productService;
        }

        public async Task<bool> AddProductsForOffer(Guid OfferId,List<Guid> ProductIds,Guid StoreId)
        {
            if (!await _productService.IsProductsForStore(ProductIds, StoreId))
                return false;

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
