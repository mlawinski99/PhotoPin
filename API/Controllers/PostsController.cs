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

        public PostsController(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository, IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _hostingEnvironment = hostingEnvironment;
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
        [Route("all")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetPostsForUser(int id)
        {
            var posts = await _postRepository.GetPostsForUser(id);

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostCreateDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);

            var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (userSub == null)
                return NotFound();

            var user = await _userRepository.GetUserByExternalId(userSub);

            if (user == null)
                return NotFound();

            if (postDto.Image == null || postDto.Image.Length == 0)
                return NotFound();


            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            var imagePath = Guid.NewGuid().ToString() + "_" + postDto.Image.FileName;
            string filePath = Path.Combine(uploadFolder, imagePath);
            postDto.Image.CopyTo(new FileStream(filePath, FileMode.Create));

            post.CreatedDate = DateTime.Now;
            post.UserId = user.Id;
            post.User = user;
            post.ImagePath = imagePath;

            await _postRepository.AddPost(post);

            var createdPost = _mapper.Map<PostReadDto>(post);

            return CreatedAtRoute(nameof(GetPost), 
                new {
                id = post.Id
                }, createdPost);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
                return NotFound();

            _postRepository.DeletePost(post);

            return NoContent();
        }
    }
}
