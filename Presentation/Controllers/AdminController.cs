using Application.DTOs.StoreDeliveryDto;
using Application.DTOs.StoreDto;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [Route("api/v1/admin")]
    [Authorize(Policy = "AdminRole")]
    [EnableRateLimiting("fixed-150-per-1h-ip")]
    public class AdminController : BaseApiController
    {
        private readonly StoreDeliveryService _storeDeliveryService;
        private readonly StoreService _storeService;

        public AdminController(StoreDeliveryService storeDeliveryService, StoreService storeService)
        {
            _storeDeliveryService = storeDeliveryService;
            _storeService = storeService;
        }

        [HttpPost("stores/{storeId}/deliveries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SetStoreDeliveries(Guid storeId, [FromBody] List<StoreDeliveryDto> storeDeliveryDtos)
        {
            var result = await _storeDeliveryService.SetStoreDeliveries(storeId, storeDeliveryDtos);
            return Ok(result);
        }

        [HttpPut("stores/activate-subscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ActivateStoreSubscriptionByAdmin([FromBody] ActivateStoreByAdminDto activateStoreByAdminDto)
        {
            var result = await _storeService.ActivateStoreSubscriptionByAdmin(activateStoreByAdminDto);
            return Ok(result);
        }
    }
}
