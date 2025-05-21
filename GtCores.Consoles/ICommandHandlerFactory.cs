namespace GtCores.Consoles;

/// <summary>
/// 命令处理器工厂。
/// </summary>
public interface ICommandHandlerFactory : ISingletonService
{
    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <returns>返回执行任务。</returns>
    Task StartAsync();
}