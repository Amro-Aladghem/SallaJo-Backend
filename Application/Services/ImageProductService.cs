using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageProductService
    {
        private readonly AppDbContext _appDbContext;

        public ImageProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddImagesForProduct(Guid ProductId, List<string> ImagesLinks)
        {
            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var link in ImagesLinks)
            {
                productImages.Add(new ProductImage()
                {
                    ProductId = ProductId,
                    ImageLink = link
                });
            }

            await _appDbContext.ProductImages.AddRangeAsync(productImages);
            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
