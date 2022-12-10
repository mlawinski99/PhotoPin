using API.Data.FavouritePostsRepository;
using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.Post;
using API.Mapping.Dtos.User;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFavouritePostsRepository _favouritePostsRepository;

        public PostsController(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository, IHostingEnvironment hostingEnvironment, IFavouritePostsRepository favouritePosts)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _hostingEnvironment = hostingEnvironment;
            _favouritePostsRepository = favouritePosts;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postRepository.GetPostById(id);

            if (post == null)
                return NotFound();

                return Ok(_mapper.Map<PostReadDto>(post));
        }


        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetPostsForUser(string userId)
        {

            //var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userId == null)
                return NotFound();

            var user = await _userRepository.GetUserByExternalId(userId);

            if (user == null)
                return NotFound();

            var posts = await _postRepository.GetPostsForUser(user.Id);

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostCreateDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            //var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

           // if (postDto.userId == null)
           //     return NotFound();

            var user = await _userRepository.GetUserByExternalId(postDto.userId);

            if (user == null)
                return NotFound();

            if (postDto.Image.Length == 0)
                return NotFound();


            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            var imagePath = Guid.NewGuid().ToString() + "_" + postDto.Image.FileName;
            string filePath = Path.Combine(uploadFolder, imagePath);
            postDto.Image.CopyTo(new FileStream(filePath, FileMode.Create));

            post.CreatedDate = DateTime.Now;
            post.UserId = user.Id;
           // post.User = user;
            post.ImagePath = imagePath;

            await _postRepository.AddPost(post);

            var createdPost = _mapper.Map<PostReadDto>(post);

            return CreatedAtRoute(nameof(GetPost), 
                new {
                id = post.Id
                }, createdPost);
        }

        [HttpDelete()]
        public async Task<ActionResult> DeletePost([FromBody] PostIdDto postModel)
        {

		//	if (postModel.userId == null)
		//		return NotFound();

			var user = await _userRepository.GetUserByExternalId(postModel.userId);

			if (user == null)
				return NotFound();

			var post = await _postRepository.GetPostById(postModel.id);

            if (post == null || post.UserId != user.Id)
                return BadRequest();

            _favouritePostsRepository.Delete(post.Id);
            _postRepository.DeletePost(post);

            return NoContent();
        }
    }
}
