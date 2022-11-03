using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.Post;
using API.Mapping.Dtos.User;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;

        public PostsController(IMapper mapper, IPostRepository postRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
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
        [Route("api/posts/all")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpGet]
        [Route("api/posts/user/{id}")]
        public async Task<IActionResult> GetPostsForUser(int id)
        {
            var posts = await _postRepository.GetPostsForUser(id);

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostCreateDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            //post.UserId =; ToDo
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
