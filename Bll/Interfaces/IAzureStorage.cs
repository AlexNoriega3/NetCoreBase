using Microsoft.AspNetCore.Http;

namespace Bll.Interfaces
{
    public interface IAzureStorage
    {
        Task DeleteFile(string path, string container);

        Task<string> EditFile(string container, IFormFile file, string path);

        Task<string> SaveFile(string container, IFormFile file);

        Task<string> SaveFile(string container, Stream file);
    }
}