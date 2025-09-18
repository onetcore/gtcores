// using System.Net.Mail;

// namespace GSites.Extensions.Emails;

// /// <summary>
// /// 邮件发送服务。
// /// </summary>
// public abstract class EmailTaskService : TaskService
// {
//     private readonly IEmailSettingsManager _settingsManager;
//     private readonly IEmailManager _emailManager;
//     private readonly ILogger<EmailTaskService> _logger;
//     /// <summary>
//     /// 初始化类<see cref="EmailTaskService"/>。
//     /// </summary>
//     /// <param name="settingsManager">配置管理接口。</param>
//     /// <param name="emailManager">电子邮件管理接口。</param>
//     /// <param name="logger">日志接口。</param>
//     protected EmailTaskService(IEmailSettingsManager settingsManager, IEmailManager emailManager, ILogger<EmailTaskService> logger)
//     {
//         _settingsManager = settingsManager;
//         _emailManager = emailManager;
//         _logger = logger;
//     }

//     /// <summary>
//     /// 名称。
//     /// </summary>
//     public override string Name => Resources.EmailTaskService;

//     /// <summary>
//     /// 描述。
//     /// </summary>
//     public override string Description => Resources.EmailTaskService_Description;

//     /// <summary>
//     /// 执行间隔时间。
//     /// </summary>
//     public override TaskInterval Interval => 60;

//     /// <summary>
//     /// 执行方法。
//     /// </summary>
//     /// <param name="argument">参数。</param>
//     public override async Task ExecuteAsync(Argument argument)
//     {
//         var settings = await _settingsManager.GetSettingsAsync();
//         if (settings == null)
//         {
//             return;
//         }

//         var messages = await _emailManager.LoadAsync(EmailStatus.Pending);
//         if (!messages.Any())
//         {
//             return;
//         }

//         foreach (var message in messages)
//         {
//             try
//             {
//                 await SendAsync(settings, message);
//                 await _emailManager.SetSuccessAsync(message.Id, settings.Id);
//             }
//             catch (Exception exception)
//             {
//                 await _emailManager.SetFailuredAsync(message.Id, EmailSettings.MaxTryTimes);
//                 _logger.LogError(exception, "发送邮件错误");
//             }
//             await Task.Delay(100);
//         }
//     }

//     /// <summary>
//     /// 发送电子邮件。
//     /// </summary>
//     /// <param name="settings">网站配置。</param>
//     /// <param name="message">电子邮件实例。</param>
//     /// <returns>返回发送任务。</returns>
//     protected virtual async Task SendAsync(EmailSettings settings, Email message)
//     {
//         using var client = new SmtpClient(settings.SmtpServer, settings.SmtpPort)
//         {
//             EnableSsl = settings.UseSsl,
//             DeliveryMethod = SmtpDeliveryMethod.Network,
//             UseDefaultCredentials = false,
//             Credentials = new System.Net.NetworkCredential(settings.SmtpUserName, settings.SmtpPassword)
//         };

//         var from = new MailAddress(settings.SmtpUserName!, settings.SmtpDisplayName);
//         var to = new MailAddress(message.To!);
//         var mail = new MailMessage(from, to)
//         {
//             Subject = message.Title,
//             IsBodyHtml = true,
//             Body = message.HtmlContent
//         };

//         await InitAsync(mail, message);
//         await client.SendMailAsync(mail);
//         message.Status = EmailStatus.Completed;
//         mail.Dispose();
//     }

//     /// <summary>
//     /// 实例化一个电子邮件。
//     /// </summary>
//     /// <param name="mail">邮件实例。</param>
//     /// <param name="message">电子邮件实例。</param>
//     protected virtual Task InitAsync(MailMessage mail, Email message) => Task.CompletedTask;
// }