using System.ComponentModel.DataAnnotations;

namespace JWTAuthService.DTOs;












public class RegisterRequestDto
{

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Full name must be between 2 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6,
        ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;





    public string Role { get; set; } = "User";
}

public class LoginRequestDto
{

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{


    public string Token { get; set; } = string.Empty;


    public string TokenType { get; set; } = "Bearer";

    public DateTime ExpiresAt { get; set; }

    public UserProfileDto User { get; set; } = new();
}

public class UserProfileDto
{

    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class ApiResponseDto<T>
{

    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponseDto<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponseDto<T> Fail(string message) =>
        new() { Success = false, Message = message };
}
