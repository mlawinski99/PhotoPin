using API.Data.UserRepo;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
	internal class UserRepositoryFake : IUserRepository
	{
		private readonly List<User> users;
		public UserRepositoryFake()
		{
			users = new List<User>()
			{
				new User() { Id = 1, ExternalId = new string("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
					UserName = "User1" },
				new User() { Id = 2, ExternalId = new string("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
					UserName = "User2" },
				new User() { Id = 3, ExternalId = new string("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
					UserName = "User3"}
			};
		}
		public async Task<User> AddUser(User user)
		{
			users.Add(user);
			return user;
		}

		public async Task<User?> GetUser(int id)
		{
			return users.FirstOrDefault(u => u.Id == id);
		}

		public async Task<User> GetUserByExternalId(string id)
		{
			return users.FirstOrDefault(u => u.ExternalId == id);
		}

		public async Task<User> GetUserByUserName(string userName)
		{
			return users.FirstOrDefault(u => u.UserName == userName);
		}

		public async Task<bool> IsUserExists(string id)
		{
			return users.Any(u => u.ExternalId == id);
		}
	}
}
