using SafariDigital.Core.Validation;
using SafariDigital.Services.Authentication.Models;

namespace SafariDigital.Services.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> Login(string login, string password);
    Task<Result<LoginResponse>> RefreshTokens();
    void Logout();
    void LogoutAll();
}