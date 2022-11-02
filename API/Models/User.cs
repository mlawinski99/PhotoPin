using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string UserName { get; set; }

        public virtual List<Post> Posts { get; set; }
        public virtual List<FavouritePost> FavouritePosts { get; set; }
    }
}
