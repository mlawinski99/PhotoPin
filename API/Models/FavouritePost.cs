using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class FavouritePost
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public  Post Post { get; set; }
        public  User User { get; set; }
      //  public int UserId { get; set; }
    }
}
