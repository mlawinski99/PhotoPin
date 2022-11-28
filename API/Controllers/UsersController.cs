using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task <IActionResult> GetUser(int id)
        {
            var user = await _userRepository.GetUser(id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {

            var user = await _userRepository.GetUserByUserName(userName);
            var test = user.Posts;
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

    }
}
