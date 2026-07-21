using JWTAuthService.DTOs;
using JWTAuthService.Models;

namespace JWTAuthService.Interfaces;













public interface IUserRepository
{

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByIdAsync(int id);

    Task<User> CreateAsync(User user);

    Task<bool> EmailExistsAsync(string email);

    Task<IEnumerable<User>> GetAllAsync();
}



public interface IAuthService
{

    Task<UserProfileDto> RegisterAsync(RegisterRequestDto request);




    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);

    Task<UserProfileDto?> GetProfileAsync(int userId);
}




public interface IJwtService
{

    string GenerateToken(User user);




    int? ValidateTokenAndGetUserId(string token);

    DateTime GetExpirationTime();
}
