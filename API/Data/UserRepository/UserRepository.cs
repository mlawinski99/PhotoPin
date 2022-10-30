﻿using API.Data.UserRepo;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetUser(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

    }
}