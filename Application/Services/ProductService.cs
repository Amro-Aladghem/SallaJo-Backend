using Application.DTOs.ProductDto;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        private async Task<GetProductsPaginatedDto> GetStoreProductsForListing(IQueryable<Product> query,GetProductsPaginatedRequestDto requestDto)
        {
            var now = DateTime.UtcNow;

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
                    Description = p.Description.Substring(0, 50),
                    SequenceProductNumber = p.SequenceNumber,
                    AmountOfDiscount = p.Discounts.Where(d => d.IsActive == true
                    && d.EndDate >= now)
                    .OrderByDescending(d => d.StartDate)
                    .Select(d => d.DiscountAmount)
                    .FirstOrDefault()
                })
                .ToListAsync();

            int? LastSequenceProductNumber = products.Count != 0 ? products.Last().SequenceProductNumber : requestDto.LastSequenceProductNumber;

            return new GetProductsPaginatedDto
            {
                Products = products,
                LastSequenceProductNumber = LastSequenceProductNumber
            };
        }

        public async Task<ProductFullInfoForCustomerDto?> GetProductFullInfoForCustomer(Guid productId)
        {
            var now = DateTime.UtcNow;

            return await _appDbContext.Products
                .Where(p => p.Id == productId && p.IsAcceptedToAppear == true && p.IsDeleted == false)
                .Select(p => new ProductFullInfoForCustomerDto
                {
                    Id = p.Id,
                    StoreId = p.StoreId,
                    StoreName = p.Store.Name,
                    StoreImageLink = p.Store.LogoImageUrl,
                    IsAcceptToShowTheStock=p.Store.IsAcceptedToShowStoke,
                    Name = p.Name,
                    Price = p.Price,
                    PrimaryImageLink = p.PrimaryImageLink,
                    Description = p.Description,
                    Stoke=p.Stock.Value,
                    AmountOfDiscount = p.Discounts
                        .Where(d => d.IsActive == true && d.EndDate >= now && now>=d.StartDate)
                        .Select(d => d.DiscountAmount)
                        .FirstOrDefault(),
                    Images = p.ProductImages.Select(pi => new ProductImageDto
                    {
                        ImageLink = pi.ImageLink,
                        Id = pi.Id
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GetProductsPaginatedDto> GetProductsForCustomer(GetProductsPaginatedRequestDto requestDto)
        {
            var query = _appDbContext.Products
                .Where(p => p.IsAcceptedToAppear == true && p.IsDeleted == false);

            return await GetStoreProductsForListing(query, requestDto);
        }

        public async Task<GetProductsPaginatedDto> GetStoreProductsForCustomer(GetProductsPaginatedRequestDto requestDto,string storeSlug)
        {
            var query = _appDbContext.Products
                .Where(p => p.IsAcceptedToAppear == true && p.IsDeleted == false
                    &&p.Store.Slug==storeSlug);

            return await GetStoreProductsForListing(query, requestDto);
        }

        public async Task<GetProductsPaginatedDto> GetStoreProductsForSeller(GetProductsPaginatedRequestDto requestDto, Guid StoreId)
        {
            var query = _appDbContext.Products
                .Where(p=> p.IsDeleted == false
                    && p.StoreId==StoreId);

            return await GetStoreProductsForListing(query, requestDto);
        }

        public async Task<GetProductFullInfoForSellerDto?> GetProductFullInfoForSeller(Guid productId)
        {
            var now = DateTime.UtcNow;

            return await _appDbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => new GetProductFullInfoForSellerDto
                {
                    Id = p.Id,
                    StoreId = p.StoreId,
                    Name = p.Name,
                    Price = p.Price,
                    PrimaryImageLink = p.PrimaryImageLink,
                    Description = p.Description,
                    Stock = p.Stock,
                    IsDeleted = p.IsDeleted,
                    IsAcceptedToAppear = p.IsAcceptedToAppear,
                    AmountOfDiscount = p.Discounts
                        .Where(d => d.IsActive == true && d.EndDate >= now)
                        .Select(d => d.DiscountAmount)
                        .FirstOrDefault(),
                    Images = p.ProductImages.Select(pi => new ProductImageDto
                    {
                        ImageLink = pi.ImageLink,
                        Id = pi.Id
                    }).ToList()
                })
                .FirstOrDefaultAsync();
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
