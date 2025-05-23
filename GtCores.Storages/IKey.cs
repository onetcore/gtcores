namespace GtCores.Storages;

/// <summary>
/// 存储唯一键接口，继承这个接口的实例将保存一个列表。
/// </summary>
public interface IKey
{
    /// <summary>
    /// 唯一键实例。
    /// </summary>
    string Key { get; }
}
