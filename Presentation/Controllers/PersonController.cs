using Application.DTOs.AuthDto;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;


namespace Presentation.Controllers
{
    [Route("api/v1/persons")]
    [ApiController]
    public class PersonController : BaseApiController
    {
        private readonly PersonService _personService;
        private readonly AuthService _authService;
        private readonly StoreService _storeService;

        public PersonController(PersonService personService, AuthService authService, StoreService storeService)
        {
            _personService = personService;
            _authService = authService;
            _storeService = storeService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("fixed-10-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login(PersonAuthDto personAuthDto)
        {
            var person = await _personService.Login(personAuthDto);

            if (person == null)
                return Unauthorized(new { message = "كلمة السر او رقم الهاتف غير صحيح" });

            TokenDto tokenDto = await _authService.CreateToken(person.SysId, person.SysId, eUserTypes.Person.ToString());

            Response.Cookies.Append("AuthToken", tokenDto.AuthToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(40)
            });

            return Ok(person);
        }

        [Authorize(Policy = "PersonRole")]
        [HttpPut("activate")]
        [EnableRateLimiting("fixed-10-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ActivatePersonByActivationCode(string ActivationCode)
        {
            if (UserId is null)
                return Unauthorized();

            Guid? StoreId = await _storeService.CheckAndGetIdIfPersonHasNotActiveStore(UserId.Value);
            if (StoreId is null)
                return Forbid("You don't have created a store yet");

            bool isDone  = await _personService.ChangePersonRoleToSellerRoleWithActivationCode(UserId.Value, ActivationCode,
                StoreId.Value);

            return Ok(isDone);
        }

        [HttpPost("register")]
        [EnableRateLimiting("fixed-10-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register(PersonAuthDto personAuthDto)
        {
            var person = await _personService.Register(personAuthDto);

            if (person == null)
                return Unauthorized(new { message = "فشل انشاء حساب , الرجاء اعادة المحاولة" });

            TokenDto tokenDto = await _authService.CreateToken(person.SysId, person.SysId, eUserTypes.Person.ToString());

            Response.Cookies.Append("AuthToken", tokenDto.AuthToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(40)
            });

            return Ok(person);
        }


        [HttpPost("token/reffresh")]
        [EnableRateLimiting("fixed-10-per-15min-ip")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult> ReffreshToken()
        {
            string? token = Request.Cookies["reffreshToken"] ?? null;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Data is missing" });

            var person = await _personService.GetPersonInfoWithReffreshToken(token);

            if (person == null)
                return Forbid("Token is not valid");

            var tokenDto = _authService.CreateAuthTokenOnly(person.SysId, person.SysId, eUserTypes.Person.ToString());

            Response.Cookies.Append("AuthToken", tokenDto.AuthToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            return Ok(person);
        }
    }
}
