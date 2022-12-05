using API.Data.PostRepo;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
	public class PostsRepositoryFake : IPostRepository
	{
		private readonly List<Post> posts;
		public PostsRepositoryFake()
		{
			posts = new List<Post>()
			{
				new Post() { Id = 1, UserId = 1, Description = "description", ImagePath = "ae1c7c94-7549-46d6-ad47-c1e5406dcb69_4.jpg", CreatedDate = DateTime.Now},
				new Post() { Id = 2, UserId = 2, Description = "description", ImagePath = "ae1c7c94-7549-46d6-ad47-c1e5406dcb69_4.jpg", CreatedDate = DateTime.Now},
				new Post() { Id = 3, UserId = 1, Description = "description", ImagePath = "ae1c7c94-7549-46d6-ad47-c1e5406dcb69_4.jpg", CreatedDate = DateTime.Now}
			};
		}
		public async Task<Post> AddPost(Post post)
		{
			posts.Add(post);
			return post;
		}

		public void DeletePost(Post post)
		{
			posts.Remove(post);
		}

		public async Task<List<Post>> GetAllPosts()
		{
			return posts;
		}

		public async Task<Post?> GetPostById(int id)
		{
			return posts.FirstOrDefault(p => p.Id == id);
		}

		public async Task<List<Post>> GetPostsForUser(int id)
		{
			return posts.Where(p => p.UserId == id).ToList();
		}
	}
}
