using Business.DTOs;
using Business.Models;

namespace Business.Interfaces;

public interface IAccountHttpClient
{
    Task<ValidatedUserDto?> ValidateLoginAsync(LoginRequest request);
}