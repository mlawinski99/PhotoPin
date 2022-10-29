namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public string UserName { get; set; }

        public virtual List<Post> Posts { get; set; }
        public virtual List<FavouritePost> FavouritePosts { get; set; }
    }
}
