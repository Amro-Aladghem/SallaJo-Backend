using Application.DTOs.AuthDto;
using Application.DTOs.PersonDto;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/v1/sellers")]
    [ApiController]
    public class SellerController : BaseApiController
    {
        private readonly SellerService _sellerService;
        private readonly AuthService _authService;
        private readonly StoreService _storeService;

        public SellerController(SellerService sellerService, AuthService authService, StoreService storeService)
        {
            _sellerService = sellerService;
            _authService = authService;
            _storeService = storeService;
        }

        [Authorize(Policy ="PersonRole")]
        [HttpGet("info/auth")]
        [EnableRateLimiting("fixed-5-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> GetSellerAuthInfo()
        {
            if (UserId is null)
                return Unauthorized("You are not authorized user");

            var seller = await _sellerService.GetSellerAuthInfoByPersonId(UserId.Value);

            if (seller is null)
                return NotFound("seller user was not found");

            Guid? StoreId = await _storeService.GetStoreIdBySellerId(seller.Id);

            if (StoreId is null)
                return Unauthorized("You are not authorized user");

            seller.StoreId = StoreId.Value;

            var tokenDto = await _authService.CreateToken(seller.PersonId, seller.Id, eUserTypes.Seller.ToString()
                ,StoreId);

            SetSellerTokens(tokenDto.AuthToken, tokenDto.ReffreshToken);

            return Ok(seller);
        }

        [Authorize(Policy = "PersonRole")]
        [HttpPost("info/initial")]
        [EnableRateLimiting("fixed-5-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> AddInitialSellerAsManagerInfo(AddInitialPersonInfoDto addInitialPersonInfoDto)
        {
            if (UserId is null)
                return Unauthorized(new { message = "You are not authorized" });

            Guid? Id = await _sellerService.HandleCreateSellerForFirstTimeAsManager(UserId.Value, addInitialPersonInfoDto);

            if(Id is null)
                return StatusCode(StatusCodes.Status500InternalServerError, new {message="Failed to create seller"});

            return Ok(Id);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetSellerInfo()
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            var seller = await _sellerService.GetSellerInfo(UserId.Value);

            if (seller is null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message="Failed to get data!" });

            return Ok(seller);
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPut("info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateSellerInfo(UpdatePersonDto updatePersonDto)
        {
            if (UserId is null) 
                return Unauthorized();

            bool isDone = await _sellerService.HandleUpdateSellerInfo(updatePersonDto,UserId.Value);
            return Ok(isDone);
        }

    }
}
