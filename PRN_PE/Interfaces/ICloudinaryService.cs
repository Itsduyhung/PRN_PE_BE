using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PRN_PE.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}