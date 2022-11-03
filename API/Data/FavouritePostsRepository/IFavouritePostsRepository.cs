using API.Models;

namespace API.Data.FavouritePostsRepository
{
    public interface IFavouritePostsRepository
    {
        Task<List<Post>>GetFavouritePostsForUser(int userId);
        Task<FavouritePost> AddToFavourite(FavouritePost post);
        Task<FavouritePost> RemoveFromFavourite(FavouritePost post);
    }
}
