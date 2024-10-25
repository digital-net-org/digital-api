using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using SafariDigital.Core.Application;
using SafariDigital.Data.Models.Database.Users;

namespace SafariDigital.Services.Authentication;

public static class AuthenticationUtils
{
    public static bool VerifyPassword(User user, string password) =>
        BCrypt.Net.BCrypt.Verify(password, user.Password);

    public static string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(10));

    public static bool VerifyPasswordValidity(string password, Regex pattern) =>
        pattern.IsMatch(password);

    public static Regex GetPasswordRegex(this IConfiguration configuration)
    {
        var pattern = configuration.GetSection<string>(EApplicationSetting.SecurityPasswordRegex);
        return new Regex(pattern);
    }
}