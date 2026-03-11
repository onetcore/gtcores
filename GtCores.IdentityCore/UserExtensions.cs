using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace GtCores.IdentityCore;

/// <summary>
/// 用户扩展类。
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// 获取当前上下文用户Id。
    /// </summary>
    /// <param name="contextAccessor">当前上下文访问接口。</param>
    /// <returns>返回当前登录用户Id。</returns>
    public static int GetUserId(this IHttpContextAccessor contextAccessor)
    {
        int.TryParse(contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        return userId;
    }

    /// <summary>
    /// 获取当前登录用户实例。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <param name="contextAccessor">HTTP上下文访问接口。</param>
    /// <returns>返回当前登录用户。</returns>
    public static async Task<TUser?> GetUserAsync<TUser>(this IHttpContextAccessor contextAccessor) where TUser : UserBase
    {
        var userManager = contextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<TUser>>();
        var user = await userManager.GetUserAsync(contextAccessor.HttpContext.User);
        return user;
    }
}