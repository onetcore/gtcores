namespace GtCores.Consoles;

/// <summary>
/// 命令处理器基类。
/// </summary>
public abstract class CommandHandler : ICommandHandler
{
    private string? _name;
    /// <summary>
    /// 命令。
    /// </summary>
    public virtual string Name => _name ??= GetType().Name.ToLower().Replace("commandhandler", "");

    /// <summary>
    /// 描述。
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="args">参数。</param>
    /// <param name="token">取消令牌。</param>
    /// <returns>返回执行任务。</returns>
    public abstract Task ExecuteAsync(CommandArgs args, CancellationToken token = default);

    /// <summary>
    /// 显示帮助。
    /// </summary>
    public abstract void ShowHelp();

    /// <summary>
    /// 显示命令格式。
    /// </summary>
    /// <param name="args">参数。</param>
    protected void ShowLine(string args)
    {
        ConsoleCore.WriteLine($"格式：{Name} {args}");
    }

    /// <summary>
    /// 显示参数帮助信息。
    /// </summary>
    /// <param name="parameter">参数。</param>
    /// <param name="help">帮助说明。</param>
    /// <param name="options">可选参数</param>
    protected void ShowLine(string parameter, string help)
    {
        ConsoleCore.Write("  ");
        Console.BackgroundColor = ConsoleColor.DarkGray;
        ConsoleCore.Write(parameter);
        Console.ResetColor();
        ConsoleCore.Write("：");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        if (parameter[0] == '[')
            ConsoleCore.Write("可选，");
        ConsoleCore.WriteLine(help);
        Console.ResetColor();
    }
}
