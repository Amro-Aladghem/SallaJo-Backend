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
    public class ProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ImageProductService _imageProductService;

        public ProductService(AppDbContext appDbContext, ImageProductService imageProductService)
        {
            _appDbContext = appDbContext;
            _imageProductService = imageProductService;
        }

        private async Task<Guid?> AddProduct(Guid StoreId, AddProductDto addProductDto)
        {
            Product product = new Product()
            {
                Name = addProductDto.Name,
                StoreId = StoreId,
                Description = addProductDto.Description,
                Price = addProductDto.Price,
                Stock = addProductDto.Stock,
                PrimaryImageLink = addProductDto.PrimaryImageLink,
                IsAcceptedToAppear = true,
                IsDeleted = false,
                NumberOfOrders = 0
            };

            await _appDbContext.Products.AddAsync(product);

            if (await _appDbContext.SaveChangesAsync() <= 0)
                return null;

            return product.Id;
        }

        public async Task<bool> UpdateProduct(Guid productId, UpdateProductDto updateProductDto)
        {
            int NumberOfRowsAffected = await _appDbContext.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(p => p.Name, updateProductDto.Name)
                    .SetProperty(p => p.Description, updateProductDto.Description)
                    .SetProperty(p => p.Price, updateProductDto.Price)
                    .SetProperty(p => p.Stock, updateProductDto.Stock)
                    .SetProperty(p => p.IsAcceptedToAppear, updateProductDto.IsAcceptedToAppear)
                    .SetProperty(p => p.PrimaryImageLink, updateProductDto.PrimaryImageLink));

            return NumberOfRowsAffected > 0;
        }

        public async Task<bool> ToggleAppearStatus(Guid productId)
        {
            int NumberOfRowsAffected = await _appDbContext.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.IsAcceptedToAppear, p => !p.IsAcceptedToAppear));

            return NumberOfRowsAffected > 0;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            int NumberOfRowsAffected = await _appDbContext.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.IsDeleted, true));

            return NumberOfRowsAffected > 0;
        }

        public async Task<GetProductsPaginatedDto> GetStoreProductsForCustomer(GetProductsPaginatedRequestDto requestDto,Guid StoreId)
        {
            var query = _appDbContext.Products
                .Where(p => p.IsAcceptedToAppear == true && p.IsDeleted == false
                    &&p.StoreId==StoreId);

            if (requestDto.LastSequenceProductNumber.HasValue)
            {
                query = query.Where(p => p.SequenceNumber < requestDto.LastSequenceProductNumber.Value);
            }

            var products = await query
                .OrderByDescending(p => p.SequenceNumber)
                .Take(requestDto.Limit)
                .Select(p => new ProductSimpleInfoDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    PrimaryImageLink = p.PrimaryImageLink,
                    Description = p.Description,
                    SequenceProductNumber = p.SequenceNumber
                })
                .ToListAsync();

            int? LastSequenceProductNumber = products.Count != 0 ? products.Last().SequenceProductNumber : requestDto.LastSequenceProductNumber;

            return new GetProductsPaginatedDto
            {
                Products = products,
                LastSequenceProductNumber = LastSequenceProductNumber
            };
        }

        public async Task<bool> HandleAddProduct(Guid StoreId, AddProductDto addProductDto)
        {
            await using (var transaction = await _appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid? ProductId = await AddProduct(StoreId, addProductDto);

                    if (ProductId == null)
                        throw new Exception("Failed to create product");

                    bool isDone = await _imageProductService.AddImagesForProduct(ProductId.Value, addProductDto.ImagesLinks);

                    if (!isDone)
                        throw new Exception("Failed to add product images");

                    await transaction.CommitAsync();
                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
