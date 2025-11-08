using PRN_PE.DTOs.Request;
using PRN_PE.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN_PE.Interfaces
{
    public interface IPostService
    {
        Task<PostResponse> CreateAsync(PostRequest request);
        Task<PostResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<PostResponse>> GetAllAsync();
        Task<PostResponse?> UpdateAsync(Guid id, PostRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}