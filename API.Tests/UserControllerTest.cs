using API.Controllers;
using API.Data;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.User;
using API.Mapping.Profiles;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Tests
{
	public class UserControllerTest 
	{
		private readonly IMapper _mapper;
		private readonly UserRepositoryFake _userRepository;
		private readonly UsersController _usersController;
		public UserControllerTest()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new UserProfile());
			});
			_mapper = config.CreateMapper();
			_userRepository = new UserRepositoryFake();
			_usersController = new UsersController(_mapper, _userRepository);
		}


		[Fact]
		public async Task GetUserResultCodeTest()
		{
			int userId = 1;

			var okResult = await _usersController.GetUser(userId);

			Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
		}

		[Fact]
		public async Task GetUser_NotFound()
		{
			int userId = 0;

			var notFoundResult = await _usersController.GetUser(userId);
			
			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async Task GetExistingUser()
		{
			int userId = 1;

			var result = await _usersController.GetUser(userId) as OkObjectResult;
			var user = _userRepository.GetUser(userId).Result;

			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(user.UserName, (result.Value as UserReadDto).UserName);

		}

		//----------------------------------------------------------------------------

		[Fact]
		public async Task GetUserByUserNameResultCodeTest()
		{
			string userName = "User1";

			var okResult = await _usersController.GetUserByUserName(userName);

			Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
		}

		[Fact]
		public async Task GetUserByUserName_NotFound()
		{
			string userName = "NotExisting";

			var notFoundResult =  await _usersController.GetUserByUserName(userName);

			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async Task GetExistingUserByUserName()
		{
			string userName = "User1";

			var result = await _usersController.GetUserByUserName(userName) as OkObjectResult;

			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(userName, (result.Value as UserReadDto).UserName);

		}
	}
}