using FullyFludged.DTOs;

namespace FullyFludged.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> Authenticate(LoginRequestDto loginRequest);
    }
}
