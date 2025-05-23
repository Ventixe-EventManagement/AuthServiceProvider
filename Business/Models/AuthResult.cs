namespace Business.Models;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; } = 200;

    public static AuthResult CreateSuccess(int statusCode = 200) => new()
    {
        Success = true,
        StatusCode = statusCode
    };

    public static AuthResult CreateFailure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Error = error,
        StatusCode = statusCode
    };
}
public class AuthResult<T> : AuthResult
{
    public T? Result { get; set; }

    public static AuthResult<T> CreateSuccess(T result, int statusCode = 200) => new()
    {
        Success = true,
        Result = result,
        StatusCode = statusCode
    };

    public new static AuthResult<T> CreateFailure(string error, int statusCode = 400) => new()
    {
        Success = false,
        Error = error,
        StatusCode = statusCode
    };
}