using JWTAuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthService.Helpers;

// ============================================================
//  PASSWORD HELPER
//  ----------------
//  Uses ASP.NET Core Identity's PasswordHasher<T> to hash
//  and verify passwords using PBKDF2 with HMAC-SHA256.
//
//  WHY NOT STORE PLAIN TEXT?
//  --------------------------
//  If the database is compromised, plain text passwords expose
//  every user's credentials immediately. A hash is a one-way
//  transformation — you cannot reverse it to get the original.
//
//  HOW PBKDF2 WORKS:
//  -----------------
//  1. A random salt is generated for each password
//  2. The password + salt is hashed thousands of times
//  3. The salt + hash are stored together
//  4. On verification, the same process is repeated and compared
// ============================================================

/// <summary>
/// Provides password hashing and verification using PBKDF2 (ASP.NET Core Identity).
/// </summary>
public static class PasswordHelper
{
    private static readonly PasswordHasher<User> _hasher = new();

    /// <summary>
    /// Hashes a plain text password using PBKDF2 with a random salt.
    /// </summary>
    /// <param name="plainTextPassword">The password entered by the user.</param>
    /// <returns>A secure hash string safe to store in the database.</returns>
    public static string HashPassword(string plainTextPassword)
    {
        // PasswordHasher requires a user instance for context,
        // but the hash itself does not depend on user properties.
        var dummyUser = new User();
        return _hasher.HashPassword(dummyUser, plainTextPassword);
    }

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="storedHash">The hash retrieved from the database.</param>
    /// <param name="plainTextPassword">The password entered by the user at login.</param>
    /// <returns>True if the password matches the hash; false otherwise.</returns>
    public static bool VerifyPassword(string storedHash, string plainTextPassword)
    {
        var dummyUser = new User();
        var result = _hasher.VerifyHashedPassword(dummyUser, storedHash, plainTextPassword);

        // VerifyHashedPassword returns Success or SuccessRehashNeeded on match
        return result == PasswordVerificationResult.Success
            || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
