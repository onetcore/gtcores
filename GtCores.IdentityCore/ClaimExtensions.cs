using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GtCores.IdentityCore;

/// <summary>
/// 用户声明扩展类。
/// </summary>
public static class ClaimExtensions
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
}