using System;
using GSites.Extensions;
using GSites.Extensions.Menus;
using GtCores.IdentityCore;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;

namespace GSites.Components.Account.Pages.Manage;

public class MenuDataProvider(SignInManager<User> signInManager, IdentitySettings identitySettings) : MenuDataProviderBase
{
    override public string Name => "Account";

    override protected async Task InitializeAsync(NavMenuItemCollection menu)
    {
        menu.AddMenu("account/Manage", "修改账户信息", match: NavLinkMatch.All, action: item => item.Authorize())
            .AddMenu("account/Manage/Email", "修改电子邮件", action: item => item.Authorize())
            .AddMenu("account/Manage/ChangePassword", "修改密码", action: item => item.Authorize());
        if (identitySettings.EnalbedExternalLogin)
        {
            var externalSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            if (externalSchemes.Any())
                menu.AddMenu("account/Manage/ExternalLogins", "外部登录", action: item => item.Authorize());
        }
        menu.AddMenu("account/Manage/PersonalData", "个人数据", action: item => item.Authorize());
        if (identitySettings.TwoFactorAuthenticationEnabled)
            menu.AddMenu("account/Manage/TwoFactorAuthentication", "双重认证", action: item => item.Authorize());
        if (identitySettings.EnabledPasskeyLogin)
            menu.AddMenu("account/Manage/Passkeys", "通用密钥", action: item => item.Authorize());
    }
}
