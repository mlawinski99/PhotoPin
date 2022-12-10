using API.Controllers;
using API.Mapping.Dtos.Post;
using API.Mapping.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class FavouritePostControllerTests
    {
        private readonly IMapper _mapper;
        private readonly UserRepositoryFake _userRepository;
        private readonly FavouriteRepositoryFake _favouriteRepository;
        private readonly FavouritePostsController _favouritePostsController;
        public FavouritePostControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PostProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new CommentProfile());
            });

            _mapper = config.CreateMapper();
            _userRepository = new UserRepositoryFake();
            _favouriteRepository = new FavouriteRepositoryFake();
            _favouritePostsController = new FavouritePostsController(_favouriteRepository, _userRepository, _mapper);
        }

        [Fact]
        public async Task AddRemoveFavouritesBadRequest()
        {
            var postIdDto = new PostIdDto { id = 1, userId = "33704c4a-5b87-464c-bfb6-51971b4d18adas" };
            var badRequest = await _favouritePostsController.AddRemoveFavourites(postIdDto);

            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public async Task AddRemoveFavouritesNoContent()
        {
            var postIdDto = new PostIdDto { id = 1, userId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200" };
            var noContent = await _favouritePostsController.AddRemoveFavourites(postIdDto);

            Assert.IsType<NoContentResult>(noContent);
        }

        //-------------------------------------------------------------------------------------------------

        [Fact]
        public async Task GetFavouritesBadRequest()
        {
            var postIdDto = new PostIdDto { id = 1, userId = "33704c4a-5b87-464c-bfb6-51971b4d18adas" };
            var badRequest = await _favouritePostsController.GetFavourites(postIdDto);

            Assert.IsType<BadRequestResult>(badRequest);
        }

        [Fact]
        public async Task GetFavouritesTest()
        {
            var postIdDto = new PostIdDto { id = 1, userId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200" };
            var okResult = await _favouritePostsController.GetFavourites(postIdDto) as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
        }

        //---------------------------------------------------------------------------------------------

        [Fact]
        public async Task GetLikesNumberTest()
        {
            var okResult = (OkObjectResult)await _favouritePostsController.GetLikesNumber(1);

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(3, (okResult.Value as PostLikesCountDto).count);
        }
    }
}
