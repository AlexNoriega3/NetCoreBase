using Azure.Storage.Blobs;
using Bll.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Bll.commons
{
    public class AzureStorage : IAzureStorage
    {
        private readonly string _connection;

        public AzureStorage(IConfiguration configuration)
        {
            this._connection = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            var client = new BlobContainerClient(this._connection, container);

            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());

            return blob.Uri.ToString();
        }

        public async Task<string> SaveFile(string container, Stream file)
        {
            var client = new BlobContainerClient(this._connection, container);

            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}.png";
            var blob = client.GetBlobClient(fileName);
            await blob.UploadAsync(file);

            return blob.Uri.ToString();
        }

        public async Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var client = new BlobContainerClient(this._connection, container);
            await client.CreateIfNotExistsAsync();
            var file = Path.GetFileName(path);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditFile(string container, IFormFile file, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(container, file);
        }
    }
}