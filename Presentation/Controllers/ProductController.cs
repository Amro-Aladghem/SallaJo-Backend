using Application.DTOs.DiscountDto;
using Application.DTOs.ProductDto;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly DiscountService _discountService;
        private readonly ProductService _productService;

        public ProductController(DiscountService discountService, ProductService productService)
        {
            _discountService = discountService;
            _productService = productService;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddProduct(AddProductDto addProductDto)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _productService.HandleAddProduct(StoreId.Value, addProductDto);

            return Ok(new { isDone });
        }

        [HttpPost("{productId}/discounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddDiscount(AddDiscountDto addDiscountDto, Guid productId)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _discountService.AddDiscount(addDiscountDto, productId);

            return Ok(new { isDone });
        }

        [HttpPut("{id}/discounts/{discountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ToggleDiscountStatus(Guid id, Guid discountId)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _discountService.ToggleDiscountStatus(discountId);

            return Ok(new { isDone });
        }
    }
}
