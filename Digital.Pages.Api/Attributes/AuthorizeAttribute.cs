using Digital.Lib.Net.Authentication.Attributes;
using Digital.Lib.Net.Entities.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Digital.Pages.Data.Models.Users;

namespace Digital.Pages.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeAttribute(
    AuthorizeType type,
    UserRole role = UserRole.User
) : AuthorizeAttribute<User>(type)
{
    public UserRole Role { get; set; } = role;

    public override void OnJwtAuthorization(AuthorizationFilterContext context, string? token, Guid? apiUserId)
    {
        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();
        var user = userRepository.GetById(apiUserId);
        if (user is null || user.Role < Role)
            throw new UnauthorizedAccessException();
    }
}