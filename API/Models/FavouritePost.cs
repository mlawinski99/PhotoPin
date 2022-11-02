using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class FavouritePost
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
      //  public int UserId { get; set; }
    }
}
