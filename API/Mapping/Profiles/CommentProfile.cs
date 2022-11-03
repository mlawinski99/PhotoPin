using API.Mapping.Dtos.Comment;
using API.Mapping.Dtos.Post;
using API.Models;
using AutoMapper;

namespace API.Mapping.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            //source -> dest
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>(); 
            CreateMap<Comment, CommentReadDto>()
                .ForMember(c => c.UserName, p => p.MapFrom(p => p.Post.User.UserName));
        }
    }
}
