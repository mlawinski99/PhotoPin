﻿using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
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

    }
}