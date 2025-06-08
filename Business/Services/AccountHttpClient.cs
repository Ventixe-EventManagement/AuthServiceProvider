using Business.DTOs;
using Business.Interfaces;
using Business.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Business.Services;

// This class is responsible for communicating with the external AccountService over HTTP.
public class AccountHttpClient(HttpClient httpClient, IConfiguration config) : IAccountHttpClient
{
    // The injected HttpClient used to make HTTP requests
    private readonly HttpClient _httpClient = httpClient;

    // Base URL of the external AccountService, read from configuration
    private readonly string _baseUrl = config["AccountService:BaseUrl"]
        ?? throw new InvalidOperationException("AccountService:BaseUrl is missing in configuration.");

    // Sends login credentials to the AccountService to validate the user
    public async Task<ValidatedUserDto?> ValidateLoginAsync(LoginRequest request)
    {
        // Construct the full endpoint URL
        var url = $"{_baseUrl}/api/accounts/validate-login";

        // Send POST request with JSON body
        var response = await _httpClient.PostAsJsonAsync(url, request);

        // If the response is not successful (e.g., 401, 400), return null
        if (!response.IsSuccessStatusCode)
            return null;

        // If successful, deserialize the response JSON into a ValidatedUserDto
        return await response.Content.ReadFromJsonAsync<ValidatedUserDto>();
    }
}
