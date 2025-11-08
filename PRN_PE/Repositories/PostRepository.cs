using Microsoft.EntityFrameworkCore;
using PRN_PE.Data;
using PRN_PE.Models;
using PRN_PE.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN_PE.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _ctx;
        public PostRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PostEntity> AddAsync(PostEntity post)
        {
            _ctx.Posts.Add(post);
            await _ctx.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(PostEntity post)
        {
            post.IsDeleted = true;
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostEntity>> GetAllAsync()
        {
            return await _ctx.Posts.AsNoTracking().Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<PostEntity?> GetByIdAsync(Guid id)
        {
            return await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task UpdateAsync(PostEntity post)
        {
            _ctx.Posts.Update(post);
            await _ctx.SaveChangesAsync();
        }
    }
}