namespace API.Mapping.Dtos.Post
{
    public class PostCreateDto
    {
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public int UserId { get; set; }
    }
}
