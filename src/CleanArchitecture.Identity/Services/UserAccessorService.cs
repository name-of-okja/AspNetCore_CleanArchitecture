using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Identity.Services;
public class UserAccessorService : IUserAccessorService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserAccessorService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string GetUserEamil()
    {
        return _contextAccessor.HttpContext.User.FindFirstValue(ApplicationClaimTypes.Email);
    }

    public string GetUserId()
    {
        return _contextAccessor.HttpContext.User.FindFirstValue(ApplicationClaimTypes.Uid);
    }

    public string GetUserName()
    {
        return _contextAccessor.HttpContext.User.FindFirstValue(ApplicationClaimTypes.UserName);
    }
}
