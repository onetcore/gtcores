namespace GtCores.Storages;

/// <summary>
/// 可存储特性。
/// </summary>
/// <param name="name">存储文件名称。</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class StorageAttribute(string name) : Attribute
{
    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; } = name;
}