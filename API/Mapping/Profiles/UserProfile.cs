using API.Mapping.Dtos.User;
using API.Models;
using AutoMapper;

namespace API.Mapping.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //source -> dest
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreatedDto, User>()
                .ForMember(u => u.ExternalId, u => u.MapFrom(u => u.Id))
                .ForMember(u => u.Id, opt => opt.Ignore());
        }
    }
}
