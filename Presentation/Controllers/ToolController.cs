using Application.DTOs.StoreDto;
using Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Presentation.Controllers
{

    [Route("api/v1/tools")]
    [ApiController]
    public class ToolController :BaseApiController
    {
        private readonly BlobStorageUploadService _blobStorageUploadService;

        private HashSet<string> ImagesType = new HashSet<string>()
        {
            "image/jpg",
            "image/jpeg",
            "image/png",
            "image/webp",
        };

        public ToolController(BlobStorageUploadService blobStorageUploadService)
        {
            _blobStorageUploadService = blobStorageUploadService;
        }

        [Authorize(Policy = "SellerRole")]
        [HttpPost("upload/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> UploadImageFile(IFormFile imageFile)
        {
            string? UploadedImageUrl = null;

            if (UserId is null)
                return Unauthorized();

            if (!ImagesType.Contains(imageFile.ContentType))
                return BadRequest(new { message = "نوع الصورة يجب ان يكون jpg,png,jpeg,webp" });

            if (imageFile is not null)
            {
                using (var stream = imageFile.OpenReadStream())
                {
                    UploadedImageUrl = await _blobStorageUploadService.UploadAsync(stream, imageFile.FileName, UserId.Value);
                }
            }

            return Ok(UploadedImageUrl);
        }
            

    }
}
