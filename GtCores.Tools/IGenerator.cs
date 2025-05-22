namespace GtCores.Tools;

/// <summary>
/// 执行接口。
/// </summary>
public interface IGenerator
{
    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="args">参数。</param>
    void Generate(Arguments args);
}