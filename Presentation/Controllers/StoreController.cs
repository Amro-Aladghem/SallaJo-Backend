using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ExternalServices;
using Application.Services;
using Application.DTOs.StoreDto;
using System.Runtime.CompilerServices;
using Application.DTOs.OfferDto;

namespace Presentation.Controllers
{
    [Route("api/v1/stores")]
    [ApiController]
    public class StoreController : BaseApiController
    {
        private readonly BlobStorageUploadService _storageUploadService;
        private readonly StoreService _storeService;
        private readonly OfferService _offerService;

        public StoreController(BlobStorageUploadService storageUploadService, StoreService storeService,
            OfferService offerService)
        {
            _storageUploadService = storageUploadService;
            _storeService = storeService;
            _offerService = offerService;
        }


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

            return Ok(new { store });
        }

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
            return Ok(new { isDone });
        }

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

            return Ok(new { isDone });
        }

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

            return Ok(new { isDone });
        }

        [HttpGet("{id}/offers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetOffersForCustomer(Guid id)
        {
            var offers = await _offerService.GetOffersForCustomer(id);

            return Ok(new { offers });
        }

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

            return Ok(new { offers });
        }
    }
}
