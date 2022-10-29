using API.Models;

namespace API.Data.UserRepo
{
    public interface IUserRepository
    {
        Task<User?> GetUser(int id);
    }
}
