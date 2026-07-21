using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Infrastructure.ExternalServices
{
    public class BlobStorageUploadService
    {
        private readonly IConfiguration _configuration;

        public BlobStorageUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckIfFileTypeIsPdf(string ContentType, string FileName)
        {
            var extension = Path.GetExtension(FileName).ToLowerInvariant();
            if (extension != ".pdf")
            {
                return false;
            }

            if (ContentType != "application/pdf")
            {
                return false;
            }

            return true;
        }

        public async Task<string> UploadSpecificTypeAsync(Stream stream, string fileName, string contentType)
        {
            Guid id = Guid.NewGuid();

            BlobClient blobClient = GetBlobClient(fileName + id.ToString());

            var blobHttpHeader = new BlobHttpHeaders
            {
                ContentType = contentType
            };

            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeader
            });


            return blobClient.Uri.ToString();
        }

        public async Task<string> UploadAsync(Stream stream, string fileName,Guid Id)
        {

            BlobClient blobClient = GetBlobClient(fileName + Id.ToString());

            await blobClient.UploadAsync(stream);

            return blobClient.Uri.ToString();
        }

        private BlobClient GetBlobClient(string fileName)
        {
            string connectionString = _configuration.GetSection("blobconnectionstring").Value!;
            string containerName = "taskalayze";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            return containerClient.GetBlobClient(fileName);
        }

    }
}
