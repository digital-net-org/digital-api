using System.Net;
using Digital.Core.Api.Test.Api;
using Digital.Core.Api.Test.Utils;
using Digital.Lib.Net.TestTools.Integration;

namespace Digital.Core.Api.Test.Integration.Users.UserMutationTests;

public class UsersPasswordMutationTest(AppFactory<Program> fixture) : UsersTest(fixture)
{
    private const string NewPassword = "1newShinyPassword*";

    [Fact]
    public async Task UpdatePassword_WithValidPassword_ShouldReturnOk() =>
        await ExecuteTest(DataFactory.TestUserPassword, NewPassword, HttpStatusCode.OK);

    [Fact]
    public async Task UpdatePassword_WithInvalidPassword_ShouldReturnUnauthorized() =>
        await ExecuteTest("InvalidPassword", NewPassword, HttpStatusCode.Unauthorized);

    private async Task ExecuteTest(string password, string payload, HttpStatusCode expectedStatusCode)
    {
        var user = GetUser();
        await BaseClient.Login(user);
        Assert.Equal(expectedStatusCode, (await BaseClient.UpdatePassword(user.Id, password, payload)).StatusCode);
        Assert.Equal(expectedStatusCode, (await BaseClient.Login(user.Login, payload)).StatusCode);
    }
}