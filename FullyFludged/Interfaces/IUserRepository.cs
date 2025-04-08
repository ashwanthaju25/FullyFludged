using FullyFludged.Models;
using FullyFludged.DTOs;

namespace FullyFludged.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterAsync(RegisterRequestDto requestDto);
        Task<bool> UserExistsAsync(string username);
        Task<User?> GetUserByUsernameAsync(string username);

    }
}
