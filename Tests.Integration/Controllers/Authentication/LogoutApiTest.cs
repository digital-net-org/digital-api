using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SafariDigital.Api;
using SafariDigital.Database.Context;
using SafariDigital.Database.Models.UserTable;
using SafariDigital.Database.Repository;
using SafariDigital.Services.Authentication.Models;
using Tests.Core.Factories;
using Tests.Core.Integration;
using Tests.Core.Utils;

namespace Tests.Integration.Controllers.Authentication;

public class LogoutApiTest : IntegrationTest
{
    private const string LogoutApi = "/authentication/logout";
    private const string LoginApi = "/authentication/login";
    private readonly Repository<User> _userRepository;

    public LogoutApiTest(ApiFactory<Program> fixture) : base(fixture)
    {
        _userRepository =
            new Repository<User>(Factory.Services.GetRequiredService<SafariDigitalContext>());
    }

    [Fact]
    public async Task Logout_ShouldLogoutClient()
    {
        // Arrange
        var user = Setup();
        var loginResponse = await Client.PostAsJsonAsync(
            LoginApi,
            new LoginRequest(user.Username, user.Password)
        );
        await Client.SetAuthorizations(loginResponse);

        // Act
        var logoutResponse = await Client.PostAsync(LogoutApi, null);
        var cookies = Client.DefaultRequestHeaders.GetValues("Cookie");
        // Assert
        Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);
        Assert.DoesNotContain("RefreshToken", cookies);
    }

    private User Setup(User? user = null)
    {
        user ??= UserFactory.CreateUser();
        _userRepository.Create(UserFactory.CreateUserWithHashedPassword(user));
        _userRepository.Save();
        var retrieved = _userRepository.Get(u => u.Username == user.Username).FirstOrDefault()!;
        retrieved.Password = user.Password;
        return retrieved;
    }
}