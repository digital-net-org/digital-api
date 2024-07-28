using System.Net.Http.Json;
using SafariDigital.Services.UserService.Models;

namespace Tests.Integration.TestUtils.ApiCollections;

public static class UserCollection
{
    public static async Task<HttpResponseMessage> UpdatePassword(this HttpClient client, Guid id,
        string currentPassword, string newPassword) =>
        await client.PutAsJsonAsync($"/user/{id.ToString()}/password",
            new UpdatePasswordRequest(currentPassword, newPassword));
}