using Client.Models;

namespace Client.ViewModels
{
    public class ListsViewModel
    {
        public List<Post> Posts { get; set; }
        public List<Post> Favourites { get; set; }
        public List<int> LikeCount { get; set; }
    }
}
