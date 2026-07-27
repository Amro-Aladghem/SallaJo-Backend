using Application.DTOs.AuthDto;
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
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected Guid? UserId
        {
            get
            {
                var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(Guid.TryParse(claimId,out var parsedClaimId))
                {
                    return parsedClaimId;
                }

                return null;
            }
        }

        protected Guid? StoreId
        {
            get
            {
                var storeIdClaim = User.FindFirst("store_id")?.Value;
                if(Guid.TryParse(storeIdClaim,out var parsedStoreIdClaim))
                {
                    return parsedStoreIdClaim;
                }

                return null;
            }
        }

        protected void SetSellerTokens(string AuthToken,string ReffreshToken)
        {
            Response.Cookies.Append("AuthToken", AuthToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(7)
            });

            Response.Cookies.Append("reffreshToken", ReffreshToken, new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
