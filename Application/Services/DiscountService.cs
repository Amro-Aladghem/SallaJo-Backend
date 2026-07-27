using Application.DTOs.DiscountDto;
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
    public class DiscountService
    {
        private readonly AppDbContext _appDbContext;

        public DiscountService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddDiscount(AddDiscountDto addDiscountDto, Guid ProductId,Guid StoreId)
        {
            Discount discount = new Discount()
            {
                ProductId = ProductId,
                StartDate = addDiscountDto.StartDate,
                EndDate = addDiscountDto.EndDate,
                DiscountAmount = addDiscountDto.DiscountAmount,
                LeastAmountNumber = addDiscountDto.LeastAmountNumber ?? 0,
                IsActive = true
            };

            await _appDbContext.Discounts.AddAsync(discount);

            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<DiscountShortInfoDto>> GetActiveDiscounts(string slug)
        {
            var now = DateTime.UtcNow;

            return await _appDbContext.Discounts
                .Where(d => d.IsActive == true 
                && d.EndDate!.Value.Date >= now.Date && d.StartDate!.Value.Date<=now.Date
                && d.Product.Store.Slug==slug)
                .Select(d => new DiscountShortInfoDto
                {
                    DiscountAmount = d.DiscountAmount,
                    LeastAmountNumber = d.LeastAmountNumber,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    Product = new ProductSimpleInfoDto
                    {
                        Id = d.Product.Id,
                        Name = d.Product.Name,
                        Price = d.Product.Price,
                        PrimaryImageLink = d.Product.PrimaryImageLink,
                        Description = d.Product.Description,
                    }
                })
                .ToListAsync();
        }

        public async Task<List<DiscountInfoDto>> GetAllDiscounts(Guid? StoreId)
        {
            return await _appDbContext.Discounts
                .Where(d=>d.Product.StoreId==StoreId)
                .Select(d => new DiscountInfoDto
                {
                    Id = d.Id,
                    StartDate = d.StartDate,
                    EndDate = d.EndDate,
                    IsActive = d.IsActive,
                    DiscountAmount = d.DiscountAmount,
                    LeastAmountNumber = d.LeastAmountNumber,
                    Product = new ProductSimpleInfoDto
                    {
                        Id = d.Product.Id,
                        Name = d.Product.Name,
                        Price = d.Product.Price,
                        PrimaryImageLink = d.Product.PrimaryImageLink,
                        Description = d.Product.Description,
                    }
                })
                .ToListAsync();
        }

        public async Task<bool> ProductHasActiveDiscount(Guid productId,Guid StoreId)
        {
            var now = DateTime.UtcNow;
            return await _appDbContext.Discounts
                .AnyAsync(d => d.ProductId == productId && d.IsActive == true && d.EndDate >= now 
                && d.Product.StoreId==StoreId);
        }

        public async Task<bool> ToggleDiscountStatus(Guid discountId,Guid StoreId)
        {
            int NumberOfRowsAffected = await _appDbContext.Discounts
                .Where(d => d.Id == discountId && d.Product.StoreId==StoreId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.IsActive, p => !p.IsActive));

            return NumberOfRowsAffected > 0;
        }
    }
}
