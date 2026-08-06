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
    public class ImageProductService
    {
        private readonly AppDbContext _appDbContext;

        public ImageProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddImageForProduct(Guid ProductId, string ImageLink)
        {
            ProductImage productImage = new ProductImage()
            {
                ProductId = ProductId,
                ImageLink = ImageLink
            };

            await _appDbContext.ProductImages.AddAsync(productImage);
            return await _appDbContext.SaveChangesAsync() > 0;
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

        public async Task<bool> UpdateImage(UpdateImageDto updateImageDto,Guid StoreId)
        {
            int NumberOfRowsAffected = await _appDbContext.ProductImages
                .Where(pi => pi.Id == updateImageDto.OldImageId && pi.Product.StoreId==StoreId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.ImageLink, updateImageDto.NewImageLink));

            return NumberOfRowsAffected > 0;
        }
    }
}
