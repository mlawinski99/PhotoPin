using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Data.UserRepo;
using API.Data.UserRepository;
using API.Mapping.Dtos.Post;
using API.Mapping.Dtos.User;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class PostsController :ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PostRepository _postRepository;

        public PostsController(IMapper mapper, PostRepository postRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postRepository.GetPostById(id);

            if (post == null)
                return NotFound();

            return Ok(_mapper.Map<PostReadDto>(post));
        }
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPosts();

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }

        public async Task<IActionResult> GetPostsForUser(int id)
        {
            var posts = await _postRepository.GetPostsForUser(id);

            return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
        }


    }
}
