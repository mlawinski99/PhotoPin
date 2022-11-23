using API.Models;

namespace API.Data.FavouritePostsRepository
{
    public interface IFavouritePostsRepository
    {
        Task<List<FavouritePost>>GetFavouritePostsForUser(int userId);
        Task<FavouritePost> GetFavouritePost(int postId, int userId);
        Task<FavouritePost> AddToFavourite(FavouritePost post);
        void RemoveFromFavourite(FavouritePost post);
    }
}
