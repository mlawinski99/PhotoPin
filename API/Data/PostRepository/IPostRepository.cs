using API.Models;

namespace API.Data.PostRepo
{
    public interface IPostRepository
    {
        Task<Post?> GetPostById(int id);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostsForUser(int id);
        Task<Post> AddPost(Post post);
        void DeletePost(Post post);
    }
}
