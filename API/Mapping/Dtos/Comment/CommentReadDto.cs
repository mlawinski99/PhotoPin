namespace API.Mapping.Dtos.Comment
{
    public class CommentReadDto
    {
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
