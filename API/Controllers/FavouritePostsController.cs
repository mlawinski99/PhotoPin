using API.Data.FavouritePostsRepository;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.Post;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/favourite")]
    [ApiController]
    [Authorize]
    public class FavouritePostsController : ControllerBase
    {
        private readonly IFavouritePostsRepository _favouritePostsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FavouritePostsController(IFavouritePostsRepository favouritePostsRepository, IUserRepository userRepository, IMapper mapper)
        {
            _favouritePostsRepository = favouritePostsRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddRemoveFavourites([FromBody] PostIdDto postModel)
        {
            var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userRepository.GetUserByExternalId(userSub);

            var post = await _favouritePostsRepository.GetFavouritePost(postModel.Id, user.Id);

            if (post == null)
            {
                FavouritePost favouritePost = new FavouritePost { PostId = postModel.Id, UserId = user.Id };
                await _favouritePostsRepository.AddToFavourite(favouritePost);
                return NoContent();
            }

            _favouritePostsRepository.RemoveFromFavourite(post);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetFavourites()
        {
            var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            Thread.Sleep(100);
            var user = await _userRepository.GetUserByExternalId(userSub);

            var favouritePosts = await _favouritePostsRepository.GetFavouritePostsForUser(user.Id);

            var posts = new List<Post>();
            foreach (var favouritePost in favouritePosts)
            {
                posts.Add(favouritePost.Post);
            }

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetLikesNumber(int id)
		{
		
			var favouritePosts = await _favouritePostsRepository.GetFavouritePostsById(id);
			int likesNumber = favouritePosts.Count();
            var likeModel = new PostLikesCountDto();
            likeModel.count = likesNumber;

			return Ok(likeModel);
		}
	}
}
