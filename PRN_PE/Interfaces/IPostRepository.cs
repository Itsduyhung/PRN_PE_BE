using PRN_PE.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN_PE.Interfaces
{
    public interface IPostRepository
    {
        Task<PostEntity> AddAsync(PostEntity post);
        Task<PostEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<PostEntity>> GetAllAsync();
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(PostEntity post);
    }
}