using Digital.Lib.Net.Core.Extensions.HttpUtilities;
using Digital.Lib.Net.Core.Messages;
using Newtonsoft.Json;

namespace Tests.Utils.Client;

public static class HttpUtils
{
    public static async Task SetAuthorizations(
        this HttpClient client,
        HttpResponseMessage loginResponse
    )
    {
        try
        {
            var content = await loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Result<string>>(content)!.Value;
            var refreshToken = loginResponse.TryGetCookie();

            if (refreshToken is not null)
                client.AddCookie(refreshToken);
            if (token is not null)
                client.AddAuthorization(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}