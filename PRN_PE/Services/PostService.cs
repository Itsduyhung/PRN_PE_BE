using PRN_PE.DTOs.Request;
using PRN_PE.DTOs.Response;
using PRN_PE.Interfaces;
using PRN_PE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN_PE.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repo;
        private readonly ICloudinaryService _cloudinaryService;

        public PostService(IPostRepository repo, ICloudinaryService cloudinaryService)
        {
            _repo = repo;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<PostResponse> CreateAsync(PostRequest request)
        {
            string? imageUrl = null;

            if (request.ImageFile != null)
            {
                imageUrl = await _cloudinaryService.UploadImageAsync(request.ImageFile);
            }

            var entity = new PostEntity
            {
                Name = request.Name,
                Description = request.Description,
                Rating = request.Rating,
                ImageUrl = imageUrl
            };

            var created = await _repo.AddAsync(entity);

            return MapToResponse(created);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(existing);
            return true;
        }

        public async Task<IEnumerable<PostResponse>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(MapToResponse);
        }

        public async Task<PostResponse?> GetByIdAsync(Guid id)
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return null;
            return MapToResponse(e);
        }

        public async Task<PostResponse?> UpdateAsync(Guid id, PostRequest request)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.Rating = request.Rating;
            existing.UpdatedAt = DateTime.UtcNow;

            if (request.ImageFile != null)
            {
                var newUrl = await _cloudinaryService.UploadImageAsync(request.ImageFile);
                if (!string.IsNullOrEmpty(newUrl))
                    existing.ImageUrl = newUrl;
            }

            await _repo.UpdateAsync(existing);
            return MapToResponse(existing);
        }

        private PostResponse MapToResponse(PostEntity e) =>
            new PostResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Rating = e.Rating,
                ImageUrl = e.ImageUrl,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            };
    }
}