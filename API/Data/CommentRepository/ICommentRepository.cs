using API.Models;

namespace API.Data.CommentRepository
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsForPost(int postId);
        Task<Comment> AddComment(Comment comment);
        Task<Comment> UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
