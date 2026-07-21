using Application.DTOs.DiscountDto;
using Domain.Entities;
using Infrastructure.Data;
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

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddDiscount(AddDiscountDto addDiscountDto,Guid ProductId)
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
    }
}
