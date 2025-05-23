using Business.DTOs;
using Business.Interfaces;
using Business.Models;
using System.Net.Http.Json;

namespace Business.Services;

public class AccountHttpClient(HttpClient http) : IAccountHttpClient
{
    public async Task<ValidatedUserDto?> ValidateLoginAsync(LoginRequest request)
    {
        var response = await http.PostAsJsonAsync("https://accountservice/api/accounts/validate-login", request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ValidatedUserDto>();
    }
}