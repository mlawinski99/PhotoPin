using API.Controllers;
using API.Data.CommentRepository;
using API.Data.UserRepo;
using API.Mapping.Dtos.Comment;
using API.Mapping.Dtos.Post;
using API.Mapping.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class CommentControllerTests
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly CommentsController _commentsController;

        public CommentControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PostProfile());
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new CommentProfile());
            });

            _mapper = config.CreateMapper();
            _userRepository = new UserRepositoryFake();
            _commentRepository = new CommentRepositoryFake();
            _commentsController = new CommentsController(_mapper, _commentRepository, _userRepository);
        }

        [Fact]
        public async Task CreateCommentBadRequest()
        {
            var postIdDto = new CommentCreateDto { PostId = 1, Text="test",userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7asfasfaf" };
            var badRequestResult = await _commentsController.CreateComment(postIdDto);

            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public async Task CreateCommentNoContent()
        {
            var postIdDto = new CommentCreateDto { PostId = 1, Text = "test", userId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200" };
            var noContentResult = await _commentsController.CreateComment(postIdDto);

            Assert.IsType<NoContentResult>(noContentResult);
        }

        [Fact]
        public async Task DeleteCommentUserIdNull()
        {
            var commentDto = new { id = 1, userId = "" };
            var notfoundResult = await _commentsController.DeleteComment(commentDto.id, commentDto.userId);

            Assert.IsType<NotFoundResult>(notfoundResult);
        }

        [Fact]
        public async Task DeleteCommentInvalidUserId()
        {
            var commentDto = new { id = 1, userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7asfasfaf" };
            var notfoundResult = await _commentsController.DeleteComment(commentDto.id, commentDto.userId);

            Assert.IsType<NotFoundResult>(notfoundResult);
        }

        [Fact]
        public async Task DeleteCommentInvalidId()
        {
            var commentDto = new { id = 0, userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7asfasfaf" };
            var notfoundResult = await _commentsController.DeleteComment(commentDto.id, commentDto.userId);

            Assert.IsType<NotFoundResult>(notfoundResult);
        }

        [Fact]
        public async Task DeleteCommentInvalidUserForComment()
        {
            var commentDto = new { id = 1, userId = "815accac-fd5b-478a-a9d6-f171a2f6ae7asfasfaf" };
            var notfoundResult = await _commentsController.DeleteComment(commentDto.id, commentDto.userId);

            Assert.IsType<NotFoundResult>(notfoundResult);
        }

        [Fact]
        public async Task DeleteCommentTest()
        {
            var commentDto = new { id = 1, userId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200" };
            var noContentResult = await _commentsController.DeleteComment(commentDto.id, commentDto.userId);

            Assert.IsType<NoContentResult>(noContentResult);
        }

    }
}
