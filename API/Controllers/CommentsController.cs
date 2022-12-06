using API.Data.CommentRepository;
using API.Data.PostRepo;
using API.Data.PostRepository;
using API.Data.UserRepo;
using API.Mapping.Dtos.Comment;
using API.Mapping.Dtos.Post;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentsController(IMapper mapper, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody]CommentCreateDto commentDto)
        {
			var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

			if (userSub == null)
				return NotFound();

			var user = await _userRepository.GetUserByExternalId(userSub);

			var comment = _mapper.Map<Comment>(commentDto);
            comment.CreatedDate= DateTime.Now;
            comment.UserName = user.UserName;
            await _commentRepository.AddComment(comment);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
			var userSub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

			if (userSub == null)
				return NotFound();

			var user = await _userRepository.GetUserByExternalId(userSub);

            if (user == null)
                return NotFound();

			var comment = await _commentRepository.GetCommentById(id);

            if (comment == null || comment.Post.UserId != user.Id)
                return NotFound();

            _commentRepository.DeleteComment(comment);

            return NoContent();
        }
    }
}
