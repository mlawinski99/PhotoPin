namespace API.Mapping.Dtos.Comment
{
    public class CommentCreateDto
    {
        public int PostId { get; set; }
        public string Text { get; set; }
        public string userId { get; set; }
    }
}
