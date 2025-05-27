using Business.DTOs;
using Business.Interfaces;
using Business.Models;
using System.Net.Http.Json;

namespace Business.Services;

public class AccountHttpClient(HttpClient http) : IAccountHttpClient
{
    public async Task<ValidatedUserDto?> ValidateLoginAsync(LoginRequest request)
    {
        var response = await http.PostAsJsonAsync("https://ventixe-accountserviceprovider-cdhuf2azb3end8eb.swedencentral-01.azurewebsites.net", request);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ValidatedUserDto>();
    }
}