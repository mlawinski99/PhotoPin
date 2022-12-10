using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Comment
{
    public class CommentCreateDto
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string userId { get; set; }
    }
}
