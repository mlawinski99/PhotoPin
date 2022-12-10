using API.Data.FavouritePostsRepository;
using API.Data.PostRepo;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    internal class FavouriteRepositoryFake : IFavouritePostsRepository
    {
            private readonly List<FavouritePost> favouritePosts;
            public FavouriteRepositoryFake()
            {
            favouritePosts = new List<FavouritePost>()
            {
                new FavouritePost() { Id = 1, UserId = 1, PostId=1},
                new FavouritePost() { Id = 2, UserId = 2, PostId=1},
                new FavouritePost() { Id = 3, UserId = 1, PostId=1}
            };
            }

        public async Task<FavouritePost> AddToFavourite(FavouritePost post)
        {
            favouritePosts.Add(post);
            return post;
        }

        public void Delete(int postId)
        {
            List<FavouritePost> posts = favouritePosts.Where(p => p.PostId == postId).ToList();
            if (posts.Any())
            {
                foreach (var post in posts)
                    favouritePosts.Remove(post);
            }
        }

        public async Task<FavouritePost> GetFavouritePost(int postId, int userId)
        {
            return favouritePosts
                .Where(p => p.PostId == postId && p.UserId == userId)
                .FirstOrDefault();
        }

        public async Task<List<FavouritePost>> GetFavouritePostsById(int id)
        {
            return favouritePosts.Where(p => p.PostId == id).ToList();
        }

        public async Task<List<FavouritePost>> GetFavouritePostsForUser(int userId)
        {
            return favouritePosts
                .Where(p => p.UserId == userId)
                .ToList();
        }

        public void RemoveFromFavourite(FavouritePost post)
        {
            favouritePosts.Remove(post);
        }
    }
}
