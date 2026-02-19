using receptionHubBackend.Controllers.Dtos;

namespace receptionHubBackend.Services
{
    public interface IAuthenticationService
    {
        Task<TokenDto?> AutentifikujAsync(LoginDto loginDto);
        Task<bool> ValidirajTokenAsync(string token);
    }
}