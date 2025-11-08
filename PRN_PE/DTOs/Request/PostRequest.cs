using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PRN_PE.DTOs.Request
{
    public class PostRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        // Only allow file upload
        public IFormFile? ImageFile { get; set; }
    }
}