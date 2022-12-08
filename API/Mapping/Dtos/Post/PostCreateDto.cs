using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
    public class PostCreateDto
    {
        [Required]
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public string userId { get; set; }
    }
}
