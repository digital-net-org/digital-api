using System.Linq.Expressions;
using Digital.Pages.Api.Attributes;
using Digital.Lib.Net.Authentication.Attributes;
using Digital.Lib.Net.Core.Predicates;
using Digital.Lib.Net.Entities.Repositories;
using Digital.Lib.Net.Mvc.Controllers.Pagination;
using Digital.Pages.Api.Dto.Entities;
using Microsoft.AspNetCore.Mvc;
using Digital.Pages.Data.Models.Users;

namespace Digital.Pages.Api.Controllers.UserApi;

[ApiController, Route("user"), Authorize(AuthorizeType.Jwt, UserRole.Admin)]
public class UserPaginationController(
    IRepository<User> userRepository
) : PaginationController<User, UserModel, UserQuery>(userRepository)
{
    protected override Expression<Func<User, bool>> Filter(Expression<Func<User, bool>> predicate, UserQuery query)
    {
        if (!string.IsNullOrEmpty(query.Username))
            predicate = predicate.Add(x => x.Username.StartsWith(query.Username));
        if (!string.IsNullOrEmpty(query.Email))
            predicate = predicate.Add(x => x.Email.StartsWith(query.Email));
        if (query.Role.HasValue)
            predicate = predicate.Add(x => x.Role == query.Role);
        if (query.IsActive.HasValue)
            predicate = predicate.Add(x => x.IsActive == query.IsActive);
        return predicate;
    }
}