using GtCores;
using GtCores.Extensions;
using GtCores.IdentityCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace GSites.Extensions.Emails;

/// <summary>
/// 电子邮件管理实现类。
/// </summary>
public class EmailManager(EmailDbContext context, IStringLocalizerFactory factory, IEmailSettingsManager settingsManager) : ObjectManager<EmailDbContext, Email>(context), IEmailManager
{
    /// <summary>
    /// 获取资源字符串。
    /// </summary>
    /// <param name="resourceKey">当前资源键。</param>
    /// <param name="resourceType">资源所在的类型实例。</param>
    /// <returns>返回资源字符串。</returns>
    protected string GetResource(string resourceKey, Type? resourceType) => resourceType == null ? resourceKey : factory.Create(resourceType).GetString(resourceKey);

    /// <summary>
    /// 获取资源，一般为内容。
    /// </summary>
    /// <param name="resourceKey">资源键。</param>
    /// <param name="replacement">替换对象，使用匿名类型实例。</param>
    /// <param name="resourceType">资源所在程序集的类型。</param>
    /// <returns>返回模板文本字符串。</returns>
    public virtual string GetTemplate(string resourceKey, object? replacement = null, Type? resourceType = null)
    {
        var resource = GetResource(resourceKey, resourceType);
        if (replacement != null)
        {
            resource = ReplaceTemplate(resource, replacement);
        }

        return resource;
    }

    private string ReplaceTemplate(string resourceKey, object fields)
    {
        var replacements = fields.ToDictionary(StringComparer.OrdinalIgnoreCase);
        foreach (var replacement in replacements)
        {
            resourceKey = resourceKey.Replace($"[[{replacement.Key}]]", replacement.Value?.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        return resourceKey;
    }

    /// <summary>
    /// 添加电子邮件接口。
    /// </summary>
    /// <param name="message">电子邮件实例对象。</param>
    /// <returns>返回添加结果。</returns>
    public virtual bool Save(Email message)
    {
        if (!settingsManager.IsEnabled())
        {
            return true;
        }

        if (message.Id > 0)
        {
            return Database.Update(message);
        }

        return Database.Create(message);
    }

    /// <summary>
    /// 添加电子邮件接口。
    /// </summary>
    /// <param name="message">电子邮件实例对象。</param>
    /// <returns>返回添加结果。</returns>
    public virtual async Task<bool> SaveAsync(Email message)
    {
        if (!await settingsManager.IsEnabledAsync())
        {
            return true;
        }

        if (message.Id > 0)
        {
            return await Database.UpdateAsync(message);
        }

        return await Database.CreateAsync(message);
    }

    /// <summary>
    /// 判断电子邮件是否已经存在，用<see cref="Email.HashKey"/>判断。
    /// </summary>
    /// <param name="message">电子邮件实例对象。</param>
    /// <param name="expiredSeconds">过期时间（秒）。</param>
    /// <returns>返回判断结果。</returns>
    public virtual bool IsExisted(Email message, int expiredSeconds = 300)
    {
        if (message.Id > 0)
        {
            return true;
        }

        var msg = Database.Find(x => x.HashKey == message.HashKey);
        if (msg == null)
        {
            return false;
        }

        return msg.CreatedDate.AddSeconds(expiredSeconds) > DateTimeOffset.Now;
    }

    /// <summary>
    /// 判断电子邮件是否已经存在，用<see cref="Email.HashKey"/>判断。
    /// </summary>
    /// <param name="message">电子邮件实例对象。</param>
    /// <param name="expiredSeconds">过期时间（秒）。</param>
    /// <returns>返回判断结果。</returns>
    public virtual async Task<bool> IsExistedAsync(Email message, int expiredSeconds = 300)
    {
        if (message.Id > 0)
        {
            return true;
        }

        var msg = await Database.FindAsync(x => x.HashKey == message.HashKey);
        if (msg == null)
        {
            return false;
        }

        return msg.CreatedDate.AddSeconds(expiredSeconds) > DateTimeOffset.Now;
    }

    /// <summary>
    /// 发送电子邮件。
    /// </summary>
    /// <param name="userId">用户Id。</param>
    /// <param name="emailAddress">电子邮件地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="content">内容。</param>
    /// <param name="action">实例化方法。</param>
    /// <returns>返回发送结果。</returns>
    public virtual bool SendEmail(int userId, string emailAddress, string title, string content, Action<Email>? action = null)
    {
        var message = new Email();
        message.UserId = userId;
        message.To = emailAddress;
        message.Title = title;
        message.TextContent = content;
        action?.Invoke(message);
        return Save(message);
    }

    /// <summary>
    /// 发送电子邮件。
    /// </summary>
    /// <param name="userId">用户Id。</param>
    /// <param name="emailAddress">电子邮件地址。</param>
    /// <param name="title">标题。</param>
    /// <param name="content">内容。</param>
    /// <param name="action">实例化方法。</param>
    /// <returns>返回发送结果。</returns>
    public virtual async Task<bool> SendEmailAsync(int userId, string emailAddress, string title, string content, Action<Email>? action = null)
    {
        var message = new Email();
        message.UserId = userId;
        message.To = emailAddress;
        message.Title = title;
        message.TextContent = content;
        action?.Invoke(message);
        return await SaveAsync(message);
    }

    /// <summary>
    /// 发送电子邮件。
    /// </summary>
    /// <param name="user">用户实例。</param>
    /// <param name="resourceKey">资源键：<paramref name="resourceKey"/>_{Title}，<paramref name="resourceKey"/>_{Content}。</param>
    /// <param name="replacement">替换对象，使用匿名类型实例。</param>
    /// <param name="resourceType">资源所在程序集的类型。</param>
    /// <param name="action">实例化方法。</param>
    /// <returns>返回发送结果。</returns>
    public bool SendEmail(UserBase user, string resourceKey, object? replacement = null, Type? resourceType = null, Action<Email>? action = null)
    {
        var title = GetTemplate(resourceKey + "_Title", replacement, resourceType);
        var content = GetTemplate(resourceKey + "_Content", replacement, resourceType);
        return SendEmail(user.Id, user.Email!, title, content, action);
    }

    /// <summary>
    /// 发送电子邮件。
    /// </summary>
    /// <param name="user">用户实例。</param>
    /// <param name="resourceKey">资源键：<paramref name="resourceKey"/>_{Title}，<paramref name="resourceKey"/>_{Content}。</param>
    /// <param name="replacement">替换对象，使用匿名类型实例。</param>
    /// <param name="resourceType">资源所在程序集的类型。</param>
    /// <param name="action">实例化方法。</param>
    /// <returns>返回发送结果。</returns>
    public Task<bool> SendEmailAsync(UserBase user, string resourceKey, object? replacement = null, Type? resourceType = null, Action<Email>? action = null)
    {
        var title = GetTemplate(resourceKey + "_Title", replacement, resourceType);
        var content = GetTemplate(resourceKey + "_Content", replacement, resourceType);
        return SendEmailAsync(user.Id, user.Email!, title, content, action);
    }

    /// <summary>
    /// 加载电子邮件列表。
    /// </summary>
    /// <param name="status">状态。</param>
    /// <returns>返回电子邮件列表。</returns>
    public virtual IEnumerable<Email> Load(EmailStatus? status = null)
    {
        var query = Context.Emails.AsNoTracking();
        if (status != null)
        {
            query.Where(x => x.Status == status);
        }

        query.OrderBy(x => x.Id);
        query.Take(100);
        return query.ToList();
    }

    /// <summary>
    /// 加载电子邮件列表。
    /// </summary>
    /// <param name="status">状态。</param>
    /// <returns>返回电子邮件列表。</returns>
    public virtual async Task<IEnumerable<Email>> LoadAsync(EmailStatus? status = null)
    {
        var query = Context.Emails.AsNoTracking();
        if (status != null)
        {
            query.Where(x => x.Status == status);
        }

        query.OrderBy(x => x.Id);
        query.Take(100);
        return await query.ToListAsync();
    }

    /// <summary>
    /// 设置失败状态。
    /// </summary>
    /// <param name="id">当前电子邮件Id。</param>
    /// <param name="maxTryTimes">最大失败次数。</param>
    /// <returns>返回设置结果。</returns>
    public virtual bool SetFailured(int id, int maxTryTimes)
    {
        using var trasaction = Context.Database.BeginTransaction();
        if (Context.Emails.Where(x => x.Id == id).ExecuteUpdate(x => x.SetProperty(e => e.TryTimes, e => e.TryTimes + 1)) == 0)
        {
            trasaction.Rollback();
            return false;
        }
        if (Context.Emails.Where(x => x.Id == id && x.TryTimes > maxTryTimes).ExecuteUpdate(x => x.SetProperty(e => e.Status, EmailStatus.Failured).SetProperty(e => e.ConfirmDate, DateTimeOffset.Now)) > 0)
        {
            trasaction.Commit();
            return true;
        }
        trasaction.Rollback();
        return false;
    }

    /// <summary>
    /// 设置失败状态。
    /// </summary>
    /// <param name="id">当前电子邮件Id。</param>
    /// <param name="maxTryTimes">最大失败次数。</param>
    /// <returns>返回设置结果。</returns>
    public virtual async Task<bool> SetFailuredAsync(int id, int maxTryTimes)
    {
        using var trasaction = await Context.Database.BeginTransactionAsync();
        if (await Context.Emails.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(e => e.TryTimes, e => e.TryTimes + 1)) == 0)
        {
            trasaction.Rollback();
            return false;
        }
        if (await Context.Emails.Where(x => x.Id == id && x.TryTimes > maxTryTimes).ExecuteUpdateAsync(x => x.SetProperty(e => e.Status, EmailStatus.Failured).SetProperty(e => e.ConfirmDate, DateTimeOffset.Now)) > 0)
        {
            trasaction.Commit();
            return true;
        }
        trasaction.Rollback();
        return false;
    }

    /// <summary>
    /// 设置成功状态。
    /// </summary>
    /// <param name="id">当前电子邮件Id。</param>
    /// <param name="settingsId">配置ID。</param>
    /// <returns>返回设置结果。</returns>
    public virtual bool SetSuccess(int id, int settingsId)
    {
        using var trasaction = Context.Database.BeginTransaction();
        if (Context.Emails.Where(x => x.Id == id).ExecuteUpdate(x => x.SetProperty(e => e.Status, EmailStatus.Completed).SetProperty(e => e.ConfirmDate, DateTimeOffset.Now).SetProperty(e => e.SettingsId, settingsId)) == 0)
        {
            trasaction.Rollback();
            return false;
        }
        if (Context.Settings.Where(x => x.Id == settingsId).ExecuteUpdate(x => x.SetProperty(e => e.Count, e => e.Count + 1)) > 0)
        {
            trasaction.Commit();
            return true;
        }
        trasaction.Rollback();
        return false;
    }

    /// <summary>
    /// 设置成功状态。
    /// </summary>
    /// <param name="id">当前电子邮件Id。</param>
    /// <param name="settingsId">配置ID。</param>
    /// <returns>返回设置结果。</returns>
    public virtual async Task<bool> SetSuccessAsync(int id, int settingsId)
    {
        using var trasaction = await Context.Database.BeginTransactionAsync();
        if (await Context.Emails.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(e => e.Status, EmailStatus.Completed).SetProperty(e => e.ConfirmDate, DateTimeOffset.Now).SetProperty(e => e.SettingsId, settingsId)) == 0)
        {
            trasaction.Rollback();
            return false;
        }
        if (await Context.Settings.Where(x => x.Id == settingsId).ExecuteUpdateAsync(x => x.SetProperty(e => e.Count, e => e.Count + 1)) > 0)
        {
            trasaction.Commit();
            return true;
        }
        trasaction.Rollback();
        return false;
    }

    /// <summary>
    /// 通过Id查询电子邮件。
    /// </summary>
    /// <param name="id">电子邮件id。</param>
    /// <returns>返回电子邮件实例。</returns>
    public virtual Email? Find(int id) => Database.Find(id);

    /// <summary>
    /// 通过Id查询电子邮件。
    /// </summary>
    /// <param name="id">电子邮件id。</param>
    /// <returns>返回电子邮件实例。</returns>
    public virtual async Task<Email?> FindAsync(int id) => await Database.FindAsync(id);

    /// <summary>
    /// 删除邮件。
    /// </summary>
    /// <param name="ids">邮件Id列表。</param>
    /// <returns>返回删除结果。</returns>
    public Task<bool> DeleteAsync(int[] ids)
    {
        return Database.DeleteAsync(x => ids.Contains(x.Id));
    }

    public IPageEnumerable<Email> Load(QueryBase<Email> query) => Database.Load(query);

    public Task<IPageEnumerable<Email>> LoadAsync(QueryBase<Email> query, CancellationToken cancellationToken = default) => Database.LoadAsync(query, cancellationToken);
}