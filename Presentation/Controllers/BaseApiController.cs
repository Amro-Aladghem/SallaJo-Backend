using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


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
    }
}
