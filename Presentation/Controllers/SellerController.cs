using Application.DTOs.AuthDto;
using Application.DTOs.PersonDto;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public SellerController(SellerService sellerService, AuthService authService)
        {
            _sellerService = sellerService;
            _authService = authService;
        }

        [HttpGet("info/auth")]
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

            var tokenDto = await _authService.CreateToken(seller.PersonId, seller.Id, eUserTypes.Seller.ToString());

            Response.Cookies.Append("AuthToken", tokenDto.AuthToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(7)
            });

            Response.Cookies.Append("reffreshToken", tokenDto.ReffreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { seller });
        }

        [HttpPost("info/initial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> AddInitialSellerAsManagerInfo(AddInitialPersonInfoDto addInitialPersonInfoDto)
        {
            if (UserId is null)
                return Unauthorized(new { message = "You are not authorized" });

            bool isDone = await _sellerService.HandleCreateSellerForFirstTimeAsManager(UserId.Value, addInitialPersonInfoDto);

            return Ok(new { isDone });
        }
    }
}
