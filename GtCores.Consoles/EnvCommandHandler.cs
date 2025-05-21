namespace GtCores.Consoles;

/// <summary>
/// 环境变量命令处理器。
/// </summary>
public class EnvCommandHandler : CommandHandler
{
    /// <summary>
    /// 描述。
    /// </summary>
    public override string Description => throw new NotImplementedException();

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="args">参数实例。</param>
    /// <param name="token">取消令牌。</param>
    public override Task ExecuteAsync(CommandArgs args, CancellationToken token = default)
    {
        if (args.IsEmpty)
        {
            ConsoleCore.ShowValues();
            return Task.CompletedTask;
        }
        foreach (var key in args)
        {
            ConsoleCore.UpdateValue(key, args[key]);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 显示帮助。
    /// </summary>
    public override void ShowHelp()
    {
        ShowLine("-wd [value]");
        ShowLine("-wd", "工作目录");
        ShowLine("[value]", "工作目录值，如果不指定，则删除当前工作目录");
    }
}