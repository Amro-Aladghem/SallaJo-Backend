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

        public async Task<bool> AddDiscount(AddDiscountDto addDiscountDto, Guid ProductId)
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

        public async Task<List<DiscountShortInfoDto>> GetActiveDiscounts()
        {
            var now = DateTime.UtcNow;

            return await _appDbContext.Discounts
                .Where(d => d.IsActive == true && d.EndDate >= now && d.StartDate<=now)
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

        public async Task<List<DiscountInfoDto>> GetAllDiscounts()
        {
            return await _appDbContext.Discounts
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

        public async Task<bool> ProductHasActiveDiscount(Guid productId)
        {
            var now = DateTime.UtcNow;
            return await _appDbContext.Discounts
                .AnyAsync(d => d.ProductId == productId && d.IsActive == true && d.EndDate >= now);
        }

        public async Task<bool> ToggleDiscountStatus(Guid discountId)
        {
            int NumberOfRowsAffected = await _appDbContext.Discounts
                .Where(d => d.Id == discountId)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.IsActive, p => !p.IsActive));

            return NumberOfRowsAffected > 0;
        }
    }
}
