using JWTAuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthService.Helpers;






















public static class PasswordHelper
{
    private static readonly PasswordHasher<User> _hasher = new();





    public static string HashPassword(string plainTextPassword)
    {


        var dummyUser = new User();
        return _hasher.HashPassword(dummyUser, plainTextPassword);
    }






    public static bool VerifyPassword(string storedHash, string plainTextPassword)
    {
        var dummyUser = new User();
        var result = _hasher.VerifyHashedPassword(dummyUser, storedHash, plainTextPassword);

        return result == PasswordVerificationResult.Success
            || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
