using sgvf_api.Dtos.Auth;

namespace sgvf_api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}