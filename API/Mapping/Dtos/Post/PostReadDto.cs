using API.Controllers;
using API.Mapping.Dtos.Comment;
using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
    public class PostReadDto
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public List<CommentReadDto> Comments { get; set; }

    }
}
