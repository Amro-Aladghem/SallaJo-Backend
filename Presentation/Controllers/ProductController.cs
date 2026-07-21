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
        private readonly ImageProductService _imageProductService;

        public ProductController(DiscountService discountService, ProductService productService, ImageProductService imageProductService)
        {
            _discountService = discountService;
            _productService = productService;
            _imageProductService = imageProductService;
        }

      

        [HttpGet("show")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProductsForCustomer([FromQuery] GetProductsPaginatedRequestDto requestDto)
        {
            var result = await _productService.GetProductsForCustomer(requestDto);

            return Ok(new { result.Products, result.LastSequenceProductNumber });
        }

        [HttpGet("{id}/public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProductFullInfoForCustomer(Guid id)
        {
            var product = await _productService.GetProductFullInfoForCustomer(id);

            if (product is null)
                return NotFound();

            return Ok(new { product });
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateProduct(UpdateProductDto updateProductDto, Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _productService.UpdateProduct(id, updateProductDto);

            return Ok(new { isDone });
        }

        [HttpPut("{id}/appear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ToggleAppearStatus(Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _productService.ToggleAppearStatus(id);

            return Ok(new { isDone });
        }

        [HttpPut("{id}/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _productService.DeleteProduct(id);

            return Ok(new { isDone });
        }

        [HttpPut("{id}/images")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateImage(UpdateImageDto updateImageDto, Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _imageProductService.UpdateImage(updateImageDto);

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
