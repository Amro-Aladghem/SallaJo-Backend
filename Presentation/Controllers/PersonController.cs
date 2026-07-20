using Application.DTOs.AuthDto;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Presentation.Controllers
{
    [Route("api/v1/persons")]
    [ApiController]
    public class PersonController:  ControllerBase
    {
        private readonly PersonService _personService;
        private readonly AuthService _authService;

        public PersonController(PersonService personService, AuthService authService)
        {
            _personService = personService;
            _authService = authService;
        }

        [HttpGet("login")]
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
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            return Ok(new { person });
        }

        [HttpGet("register")]
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
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            return Ok(new { person });
        }




    }
}
