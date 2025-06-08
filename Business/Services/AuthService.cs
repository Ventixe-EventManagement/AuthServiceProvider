using Business.Interfaces;
using Business.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// AuthService handles authentication logic, such as validating credentials and issuing JWT tokens.
public class AuthService(IAccountHttpClient accountHttpClient, IConfiguration config) : IAuthService
{
    private readonly IAccountHttpClient _accountHttp = accountHttpClient;

    // Application configuration, used to access JWT settings
    private readonly IConfiguration _config = config;

    // Handles login logic: validates user credentials and issues a JWT token
    public async Task<AuthResult<string>> LoginAsync(LoginRequest request)
    {
        // Call the account service to validate the login request
        var validatedUser = await _accountHttp.ValidateLoginAsync(request);

        // If validation fails, return an unauthorized result
        if (validatedUser == null)
            return AuthResult<string>.CreateFailure("Invalid credentials", 401);

        // Create standard JWT claims based on the validated user
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, validatedUser.Id),
            new Claim(ClaimTypes.NameIdentifier, validatedUser.Id),
            new Claim(ClaimTypes.Email, validatedUser.Email)
        };

        // Add role claims for each user role
        foreach (var role in validatedUser.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create the symmetric security key using the secret from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the JWT token with claims and expiration time
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        // Convert the token object to a string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Return the token wrapped in a successful result
        return AuthResult<string>.CreateSuccess(tokenString);
    }

    // Placeholder for logout functionality (currently stateless)
    public Task<AuthResult> LogoutAsync() =>
        Task.FromResult(AuthResult.CreateSuccess());
}
