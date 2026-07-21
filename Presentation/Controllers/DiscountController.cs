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
    [Route("api/v1/discounts")]
    [ApiController]
    public class DiscountController : BaseApiController
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> GetDiscounts()
        {
            if (UserId is null || StoreId is null)
                return Unauthorized();

            var discounts = await _discountService.GetAllDiscounts();

            return Ok(new { discounts });
        }

        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetActiveDiscounts()
        {
            var discounts = await _discountService.GetActiveDiscounts();

            return Ok(new { discounts });
        }
    }
}
