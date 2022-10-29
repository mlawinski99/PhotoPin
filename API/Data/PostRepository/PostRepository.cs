using API.Data.PostRepo;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Data.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Post> AddPost(Post post)
        {
            await _dbContext.Posts.AddAsync(post);
            _dbContext.SaveChangesAsync();
            return post;
        }

        public void DeletePost(Post post)
        {
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dbContext.Posts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _dbContext.Posts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostForUser(int id)
        {
            return await _dbContext.Posts
                .AsNoTracking()
                .Where(p => p.Id == id)
                .ToListAsync();
        }

        public async Task<Post> UpdatePost(Post post)
        {
            _dbContext.Attach(post);
            _dbContext.Entry(post).State = EntityState.Modified;
            _dbContext.SaveChangesAsync();
            return post;
        }
    }
}
