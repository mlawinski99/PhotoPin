﻿using API.Data.UserRepo;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace API.Data.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUser(int id)
        {
           return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByExternalId(string id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ExternalId == id);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _dbContext.Users
                .Where(u => u.UserName == userName)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserExists(string id)
        {
            return await _dbContext.Users.AnyAsync(u => u.ExternalId == id);
        }
    }
}
