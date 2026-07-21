using Application.DTOs.DiscountDto;
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
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
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

            bool isDone = await _productService.AddDiscount(addDiscountDto,productId);

            return Ok(new { isDone });
        }
    }
}
