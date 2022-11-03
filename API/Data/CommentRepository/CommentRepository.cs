using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace API.Data.CommentRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public void DeleteComment(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsForPost(int postId)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.Post.Id == postId)
                .ToListAsync();
        }

        public async Task<Comment> UpdateComment(Comment comment)
        {
            _dbContext.Attach(comment);
            _dbContext.Entry(comment).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return comment;
        }
    }
}
