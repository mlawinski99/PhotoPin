using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
    public class PostCreateDto
    {
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
