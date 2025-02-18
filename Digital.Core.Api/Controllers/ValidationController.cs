using Digital.Lib.Net.Authentication.Attributes;
using Digital.Lib.Net.Authentication.Options;
using Digital.Lib.Net.Core.Extensions.StringUtilities;
using Digital.Lib.Net.Files.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Digital.Core.Api.Controllers;

[ApiController, Route("validation")]
public class ValidationController(
    IConfiguration configuration,
    IOptions<DigitalFilesOptions> filesOptions,
    IOptions<AuthenticationOptions> authenticationOptions

) : ControllerBase
{
    [HttpGet("email/pattern")]
    public ActionResult<string> GetEmailPattern() => Ok(RegularExpressions.EmailPattern);

    [HttpGet("username/pattern"), Authorize(AuthorizeType.Jwt)]
    public ActionResult<string> GetUsernamePattern() => Ok(RegularExpressions.UsernamePattern);

    [HttpGet("password/pattern"), Authorize(AuthorizeType.Jwt)]
    public ActionResult<string> GetPasswordPattern() => Ok(authenticationOptions.Value.PasswordConfig.PasswordRegex);

    [HttpGet("avatar/size"), Authorize(AuthorizeType.Jwt)]
    public ActionResult<long> GetAvatarMaxSize() => Ok(filesOptions.Value.MaxAvatarSize);
}