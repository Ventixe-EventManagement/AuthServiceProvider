using Business.Interfaces;
using Business.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService(IAccountHttpClient accountHttpClient, IConfiguration config) : IAuthService
{
    private readonly IAccountHttpClient _accountHttp = accountHttpClient;
    private readonly IConfiguration _config = config;

    public async Task<AuthResult<string>> LoginAsync(LoginRequest request)
    {
        var validatedUser = await _accountHttp.ValidateLoginAsync(request);
        if (validatedUser == null)
            return AuthResult<string>.CreateFailure("Invalid credentials", 401);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, validatedUser.Id),
            new Claim(ClaimTypes.NameIdentifier, validatedUser.Id),
            new Claim(ClaimTypes.Email, validatedUser.Email)
        };

        foreach (var role in validatedUser.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return AuthResult<string>.CreateSuccess(tokenString);
    }

    public Task<AuthResult> LogoutAsync() =>
        Task.FromResult(AuthResult.CreateSuccess());
}