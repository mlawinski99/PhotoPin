using API.Controllers;
using API.Mapping.Dtos.Post;
using API.Mapping.Dtos.User;
using API.Mapping.Profiles;
using API.Models;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace API.Tests
{
	public class PostsControllerTests
	{

		private readonly IMapper _mapper;
		private readonly UserRepositoryFake _userRepository;
		private readonly PostsRepositoryFake _postsRepository;
		private readonly FavouriteRepositoryFake _favouriteRepository;
		private readonly PostsController _postsController;
		public PostsControllerTests()
		{
			var config = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new PostProfile());
				cfg.AddProfile(new UserProfile());
				cfg.AddProfile(new CommentProfile());
			});

			var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment
				.Setup(m => m.WebRootPath)
				.Returns("D:\\Projekty\\PhotoPin\\API\\wwwroot\\");
            mockEnvironment
			.Setup(m => m.EnvironmentName)
			.Returns("Hosting:UnitTestEnvironment");

			
            _mapper = config.CreateMapper();
			_userRepository = new UserRepositoryFake();
			_postsRepository = new PostsRepositoryFake();
			_favouriteRepository = new FavouriteRepositoryFake();
			_postsController = new PostsController(_mapper, _postsRepository, _userRepository, mockEnvironment.Object, _favouriteRepository);
		}

		[Fact]
		public async Task GetPostResultCodeTest()
		{
			int postId = 1;

			var okResult = await _postsController.GetPost(postId);

			Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
		}

		[Fact]
		public async Task GetPost_NotFound()
		{
			int postId = 0;

			var notFoundResult = await _postsController.GetPost(postId);

			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async Task GetExistingPost()
		{
			int postId = 1;

			var result = await _postsController.GetPost(postId) as OkObjectResult;

			Assert.IsType<OkObjectResult>(result);
			Assert.Equal(postId, (result.Value as PostReadDto).Id);

		}

		//---------------------------------------------------------------------------


		[Fact]
		public async Task GetPostsResultCodeTest()
		{

			var okResult = await _postsController.GetPosts();

			Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
		}

		//---------------------------------------------------------------------------
		[Fact]
		public async Task GetPostsForUserNotFound()
		{
			var notFoundResult = await _postsController.GetPostsForUser("33704c4a - 5b87 - 464c - bfb6 - 51971b4d18adas");

			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async Task GetPostsForUser()
		{
			int userId = 1;

			var user = await _userRepository.GetUser(userId);
			var okResult = await _postsController.GetPostsForUser(user.ExternalId);

			Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
		}

		//---------------------------------------------------------------------------
		[Fact]
		public async Task CreatePostInvalidUser()
        {
            var bytes = Encoding.UTF8.GetBytes("test");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "test", "test.jpg");

            var newPost = new PostCreateDto { Description = "desc", Image=file, userId = "33704c4a-5b87-464c-bfb6-51971b4d18adaga" };
			var notFoundResult = await _postsController.CreatePost(newPost);

			Assert.IsType<NotFoundResult>(notFoundResult);
        }

		[Fact]
		public async Task CreatePostEmptyImage()
		{
            var bytes = Encoding.UTF8.GetBytes("");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "test", "test.jpg");

            var newPost = new PostCreateDto { Description = "desc", Image = file, userId = "33704c4a-5b87-464c-bfb6-51971b4d18adaga" };
            var notFoundResult = await _postsController.CreatePost(newPost);

            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task CreatePostValidData()
        {
            var bytes = Encoding.UTF8.GetBytes("gdsgsdgdsgsdg");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "test", "test.jpg");

            var newPost = new PostCreateDto { Description = "desc", Image = file, userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7f" };
            var CreatedResult = await _postsController.CreatePost(newPost);

            Assert.IsType<CreatedAtRouteResult>(CreatedResult);
        }

        //---------------------------------------------------------------------------

        [Fact]
		public async Task DeletePostNotFoundUser()
		{
			var postIdDto = new PostIdDto { id = 1, userId = "33704c4a - 5b87 - 464c - bfb6 - 51971b4d18adas" };
			var notFoundResult = await _postsController.DeletePost(postIdDto);

			Assert.IsType<NotFoundResult>(notFoundResult);
		}

		[Fact]
		public async Task DeletePostNotFoundPostForUser()
		{
			var postIdDto = new PostIdDto { id = 1, userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7f" };
			var badRequestResult = await _postsController.DeletePost(postIdDto);

			Assert.IsType<BadRequestResult>(badRequestResult);
		}

		[Fact]
		public async Task DeletePostTest()
		{
			var postIdDto = new PostIdDto { id = 1, userId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200" };
			var noContent = await _postsController.DeletePost(postIdDto);

			Assert.IsType<NoContentResult>(noContent);
		}
	}
}
