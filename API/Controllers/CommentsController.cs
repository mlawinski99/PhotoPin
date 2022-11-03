using API.Data.CommentRepository;
using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Mapping.Dtos.Comment;
using API.Mapping.Dtos.Post;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;

        public CommentsController(IMapper mapper, CommentRepository commentRepository)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsForPost(int postId)
        {
            var comments = await _commentRepository.GetCommentsForPost(postId);

            return Ok(_mapper.Map<IEnumerable<CommentReadDto>>(comments));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int postId, CommentCreateDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
             comment.PostId = postId;
            await _commentRepository.AddComment(comment);

          //  var createdPost = _mapper.Map<CommentReadDto>(comment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var comment = await _commentRepository.GetCommentById(id);

            if (comment == null)
                return NotFound();

            _commentRepository.DeleteComment(comment);

            return NoContent();
        }
    }
}
