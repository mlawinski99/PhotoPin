using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
    public class PostCreateDto
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string userId { get; set; }
    }
}
