using API.Controllers;
using API.Mapping.Dtos.Post;
using API.Mapping.Dtos.User;
using API.Mapping.Profiles;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
			.Setup(m => m.EnvironmentName)
			.Returns("Hosting:UnitTestEnvironment");

			_mapper = config.CreateMapper();
			_userRepository = new UserRepositoryFake();
			_postsRepository = new PostsRepositoryFake();
			_postsController = new PostsController(_mapper,_postsRepository, _userRepository, mockEnvironment.Object);
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

		[Fact]
		public async Task GetPosts()
		{
			var result = await _postsController.GetPosts() as OkObjectResult;

			Assert.IsType<OkObjectResult>(result);

		}

		//---------------------------------------------------------------------------

		[Fact]
		public async Task GetPostsUser()
		{
			int userId = 1;

			var posts = await _postsRepository.GetPostsForUser(userId);

			Assert.Equal(2, posts.Count);
		}

		//---------------------------------------------------------------------------

		[Fact]
		public async Task CreatePost()
		{
			var post = new Post { Id = 4, UserId = 1, Description = "description", ImagePath = "ae1c7c94-7549-46d6-ad47-c1e5406dcb69_4.jpg", CreatedDate = DateTime.Now };

			var createdPost = await _postsRepository.AddPost(post);

			Assert.Equal(post, createdPost);
		}

		//---------------------------------------------------------------------------

		[Fact]
		public async Task DeletePost()
		{
			int postId = 1;

			var post = await _postsRepository.GetPostById(postId);

			_postsRepository.DeletePost(post);

			var postAfterDelete = await _postsRepository.GetPostById(postId);

			Assert.Null(postAfterDelete);
		}
	}
}
