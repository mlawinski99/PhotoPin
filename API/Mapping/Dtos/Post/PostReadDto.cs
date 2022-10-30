using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
    public class PostReadDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImagePath { get; set; }

    }
}
