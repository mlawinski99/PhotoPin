using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.FavouritePostsRepository
{
    public class FavouritePostsRepository : IFavouritePostsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FavouritePostsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<FavouritePost> AddToFavourite(FavouritePost post)
        {
            await _dbContext.FavouritePosts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            return post;
        }

        public async Task<FavouritePost> GetFavouritePost(int postId, int userId)
        {
            return await _dbContext.FavouritePosts
                .Where(p => p.PostId == postId && p.UserId == userId)
                .Include(p => p.Post)
                .FirstOrDefaultAsync();
        }

		public async Task<List<FavouritePost>> GetFavouritePostsById(int id)
		{
            return await _dbContext.FavouritePosts
                .Where(p => p.PostId == id)
                .ToListAsync();

		}

		public async Task<List<FavouritePost>> GetFavouritePostsForUser(int userId)
        {
            return await _dbContext.FavouritePosts
                .Where(p => p.UserId == userId)
                .Include(p => p.Post)
                .ThenInclude(p => p.User)
                .ToListAsync();
        }

        public void RemoveFromFavourite(FavouritePost post)
        {
            _dbContext.FavouritePosts.Remove(post);
            _dbContext.SaveChanges();
        }
    }
}
