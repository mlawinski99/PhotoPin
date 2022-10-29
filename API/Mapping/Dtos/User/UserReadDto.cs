using API.Mapping.Dtos.Post;

namespace API.Mapping.Dtos.User
{
    public class UserReadDto
    {
        public string UserName { get; set; }

        public List<PostReadDto> Posts = new List<PostReadDto>();
    }
}
