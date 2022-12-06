using API.Models;

namespace API.Data.CommentRepository
{
    public interface ICommentRepository
    {
        Task<Comment> AddComment(Comment comment);
        void DeleteComment(Comment comment);
        Task<Comment?> GetCommentById(int id);

	}
}
