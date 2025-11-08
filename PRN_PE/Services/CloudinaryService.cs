using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PRN_PE.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System; // Cần thiết cho ArgumentException và InvalidOperationException

namespace PRN_PE.Services
{
    // Class ánh xạ từ section "CloudinarySettings" trong appsettings.json
    public class CloudinarySettings
    {
        // Sử dụng kiểu string? để tránh cảnh báo Nullability, vì giá trị có thể là null nếu binding lỗi.
        public string? CloudName { get; set; }
        public string? ApiKey { get; set; }
        public string? ApiSecret { get; set; }
    }

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            // Kiểm tra rõ ràng để đảm bảo Cấu hình đã được đọc thành công
            if (string.IsNullOrEmpty(settings.CloudName) || string.IsNullOrEmpty(settings.ApiKey) || string.IsNullOrEmpty(settings.ApiSecret))
            {
                // Ném lỗi ArgumentException tùy chỉnh nếu cấu hình thiếu
                throw new ArgumentException("Cloudinary configuration settings (CloudName, ApiKey, ApiSecret) are missing or invalid. Check appsettings.json and Program.cs binding.");
            }

            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            // Tạo ID công khai duy nhất để tránh xung đột khi Overwrite = false
            var publicId = Path.GetFileNameWithoutExtension(file.FileName) + "_" + System.DateTime.Now.Ticks;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = false,
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK || uploadResult.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return uploadResult.SecureUri.ToString();
            }

            // Ném exception chi tiết hơn nếu upload thất bại
            throw new InvalidOperationException($"Cloudinary upload failed with status: {uploadResult.StatusCode} and error: {uploadResult.Error?.Message}");
        }
    }
}