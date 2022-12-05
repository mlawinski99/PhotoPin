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
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public void DeletePost(Post post)
        {
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            return await _dbContext.Posts
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Post?> GetPostById(int id)
        {
            return await _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedDate).Take(5))
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsForUser(int id)
        {
            return await _dbContext.Posts
                .AsNoTracking()
                .Where(p => p.UserId == id)
                .ToListAsync();
        }
    }
}
