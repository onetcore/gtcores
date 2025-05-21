namespace GtCores.Consoles;

/// <summary>
/// 命令处理器，命令以“.”开头。
/// </summary>
public interface ICommandHandler : IServices
{
    /// <summary>
    /// 命令。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 描述。
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="args">参数。</param>
    /// <param name="token">取消令牌。</param>
    /// <returns>返回执行任务。</returns>
    Task ExecuteAsync(CommandArgs args, CancellationToken token = default);

    /// <summary>
    /// 显示帮助。
    /// </summary>
    void ShowHelp();
}
