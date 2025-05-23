using System.Text.Json.Serialization;

namespace GtCores.Storages;

/// <summary>
/// 存储文件类。
/// </summary>
public class HashedFile : IKey
{
    /// <summary>
    /// 扩展名。
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// 文件大小。
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// 文件MD5值。
    /// </summary>
    public string? FileId { get; set; }

    /// <summary>
    /// 唯一键，用于存储。
    /// </summary>
    [JsonIgnore]
    public string Key => FileId ?? string.Empty;
}
