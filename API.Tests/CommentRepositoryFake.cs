using API.Data.CommentRepository;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class CommentRepositoryFake : ICommentRepository
    {
        private readonly List<Comment> comments;
        public CommentRepositoryFake()
        {
            comments = new List<Comment>()
            {
                new Comment() { Id = 1, Text = "com1", UserName="User1", CreatedDate = DateTime.Now, PostId = 1 },
                new Comment() { Id = 2, Text = "com2", UserName="User1", CreatedDate = DateTime.Now, PostId = 1 },
                new Comment() { Id = 3, Text = "com3", UserName="User1", CreatedDate = DateTime.Now, PostId = 1 },
            };
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            comments.Add(comment);
            return comment;
        }

        public void DeleteComment(Comment comment)
        {
            comments.Remove(comment);
        }

        public async Task<Comment?> GetCommentById(int id)
        {
            return comments.Where(c => c.Id == id).FirstOrDefault();
        }
    }
}
