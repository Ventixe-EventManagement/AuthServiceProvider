using Business.Models;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<AuthResult<string>> LoginAsync(LoginRequest request);
    Task<AuthResult> LogoutAsync();
}