using System.Net.Http.Json;
using Safari.Net.Core.Messages;
using Safari.Net.Data.Entities;
using Safari.Net.TestTools.Integration;
using SafariDigital.Api;
using SafariDigital.Data.Context;
using SafariDigital.Data.Models.Database;
using SafariDigital.Data.Models.Dto;
using SafariDigital.Data.Services;
using Tests.Utils.ApiCollections;
using Tests.Utils.Factories;

namespace Tests.SafariDigital.Api.Controllers.UserController;

public class GetUserApiTest : IntegrationTest<Program, SafariDigitalContext>
{
    private readonly UserFactory _userFactory;
    private readonly List<User> _userPool;

    public GetUserApiTest(AppFactory<Program, SafariDigitalContext> fixture) : base(fixture)
    {
        SafariDigitalRepository<User> userRepository = new(GetContext());
        _userFactory = new UserFactory(userRepository);
        _userPool = Setup();
    }

    [Fact]
    public async Task GetUser_ReturnsRows()
    {
        var response = await BaseClient.GetUsers(new UserQuery());
        var content = await response.Content.ReadFromJsonAsync<QueryResult<UserModel>>();
        Assert.Equal(_userPool.Count + 1, content?.Count);
        _userFactory.Dispose();
    }

    [Fact]
    private async Task GetUser_ReturnsRows_WhenQuerySpecificUsername()
    {
        var response = await BaseClient.GetUsers(new UserQuery { Username = "user1" });
        var content = await response.Content.ReadFromJsonAsync<QueryResult<UserModel>>();
        Assert.Equal(11, content?.Count);
        _userFactory.Dispose();
    }

    [Fact]
    private async Task GetUser_ReturnsRows_WhenQueryMultipleFilters()
    {
        var response = await BaseClient.GetUsers(new UserQuery { Username = "user4", IsActive = false });
        var content = await response.Content.ReadFromJsonAsync<QueryResult<UserModel>>();
        Assert.Equal(1, content?.Count);
        _userFactory.Dispose();
    }

    [Fact]
    public async Task GetUserById_ReturnsUser()
    {
        var response = await BaseClient.GetUser(_userPool[0].Id);
        var content = await response.Content.ReadFromJsonAsync<Result<UserModel>>();
        Assert.Equal(_userPool[0].Id, content?.Value?.Id);
        _userFactory.Dispose();
    }

    private List<User> Setup()
    {
        var (user, password) = _userFactory.CreateUser();
        BaseClient.Login(user.Username, password).Wait();

        List<User> users = [];
        for (var i = 0; i < 25; i++)
        {
            var (usr, _) = _userFactory.CreateUser(new UserPayload
            {
                Username = $"user{i}",
                Email = $"user{i}@msn.com",
                Role = i is >= 10 and <= 20 ? EUserRole.Admin : i > 20 ? EUserRole.SuperAdmin : EUserRole.User,
                IsActive = i > 5
            });
            users.Add(usr);
        }

        return users;
    }
}