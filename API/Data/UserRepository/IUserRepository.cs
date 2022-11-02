using API.Mapping.Dtos.User;
using API.Models;

namespace API.Data.UserRepo
{
    public interface IUserRepository
    {
        Task<bool> IsUserExists(string id);
        Task<User?> GetUser(int id);
        Task<User> AddUser(User user);
    }
}
