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
        }
    }
}
