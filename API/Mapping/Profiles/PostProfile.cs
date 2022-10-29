using API.Mapping.Dtos.Post;
using API.Models;
using AutoMapper;

namespace API.Mapping.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            //source -> dest
            CreateMap<PostCreateDto, Post>();
            CreateMap<Post, PostReadDto>()
                .ForMember(p => p.UserName, p => p.MapFrom(p => p.User.UserName));
        }
    }
}
