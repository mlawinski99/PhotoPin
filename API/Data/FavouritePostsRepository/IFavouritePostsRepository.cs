using API.Models;

namespace API.Data.FavouritePostsRepository
{
    public interface IFavouritePostsRepository
    {
        Task<List<FavouritePost>>GetFavouritePostsForUser(int userId);
        Task<FavouritePost> GetFavouritePost(int postId, int userId);
        Task<FavouritePost> AddToFavourite(FavouritePost post);
        Task<List<FavouritePost>> GetFavouritePostsById(int id);
        void Delete(int postId);
        void RemoveFromFavourite(FavouritePost post);
    }
}
