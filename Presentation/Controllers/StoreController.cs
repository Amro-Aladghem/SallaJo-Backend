using Application.DTOs.OfferDto;
using Application.DTOs.ProductDto;
using Application.DTOs.StoreDto;
using Application.Services;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/v1/stores")]
    [ApiController]
    public class StoreController : BaseApiController
    {
        private readonly BlobStorageUploadService _storageUploadService;
        private readonly StoreService _storeService;
        private readonly OfferService _offerService;
        private readonly ProductService _productService;
        private readonly DiscountService _discountService;

        public StoreController(BlobStorageUploadService storageUploadService, StoreService storeService,
            OfferService offerService, ProductService productService, DiscountService discountService)
        {
            _storageUploadService = storageUploadService;
            _storeService = storeService;
            _offerService = offerService;
            _productService = productService;
            _discountService = discountService;
        }


        [Authorize(Policy = "PersonRole")]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> CreateStoreWithInitialInformation([FromForm] AddInitialStoreInfoDto addInitialStoreInfoDto,
            IFormFile Image)
        {
            string? UploadedImageUrl = null;

            if (UserId is null)
                return Unauthorized();

            if (Image is not null)
            {
                using (var stream = Image.OpenReadStream())
                {
                    UploadedImageUrl = await _storageUploadService.UploadAsync(stream, Image.FileName, addInitialStoreInfoDto.SellerId);
                }
            }

            var store = await _storeService.AddInitialStoreInfo(addInitialStoreInfoDto);
            if (store is null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "فشل اضافة المتجر الرجاء اعادة المحاولة" });

            return Ok(store);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetStoreInfoForSeller()
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            var store = await _storeService.GetStoreInfoForSeller(StoreId.Value);

            if (store is null)
                return StatusCode(StatusCodes.Status500InternalServerError, new {message="Failed to get data!"});

            return Ok(store);
        }

        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetStorePageInfo(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return BadRequest(new { message = "store slug is not valid!" });

            var store = await _storeService.GetStorePageInfo(slug);

            if (store is null)
                return NotFound();

            return Ok(store);
        }

        [HttpGet("{slug}/info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetStoreInfoForCustomer(string slug)
        {
            var store = await _storeService.GetStoreInfoForCustomer(slug);

            if (store is null)
                return NotFound();

            return Ok(store);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateStoreInfo([FromForm] UpdateStoreInfoDto updateStoreInfoDto,Guid id)
        {
            if (UserId is null)
                return Unauthorized();

            bool isDone = await _storeService.UpdateStoreInfo(updateStoreInfoDto,id);
            return Ok(isDone);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPost("offer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> CreateOfferForStore(AddOfferDto addOfferDto)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _offerService.HandleCreateOffer(StoreId.Value, addOfferDto);

            return Ok(isDone);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPut("offer/{id}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ToggleOfferStatus(Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _offerService.ToggleOfferStatus(id);

            return Ok(isDone);
        }

        [HttpGet("{slug}/offers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetOffersForCustomer(string slug)
        {
            var offers = await _offerService.GetOffersForCustomer(slug);

            return Ok(offers);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPut("offers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOffer(UpdateOfferDto updateOfferDto, Guid id)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            bool isDone = await _offerService.UpdateOffer(id, updateOfferDto);

            return Ok(isDone);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpGet("offers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetOffersForSeller()
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            var offers = await _offerService.GetOffersForSeller(StoreId.Value);

            return Ok(offers);
        }

        [HttpGet("{slug}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProductsForCustomer([FromQuery] GetProductsPaginatedRequestDto requestDto,string slug)
        {
            
            var result = await _productService.GetStoreProductsForCustomer(requestDto, slug);

            return Ok(result);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetProductsForSeller([FromQuery] GetProductsPaginatedRequestDto requestDto)
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            var result = await _productService.GetStoreProductsForSeller(requestDto, StoreId.Value);

            return Ok(result);
        }

        [HttpGet("{slug}/active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetActiveDiscounts(string slug)
        {
            var discounts = await _discountService.GetActiveDiscounts(slug);

            return Ok(discounts);
        }

    }
}
