using System.Security.Cryptography;

namespace GtCores.Storages;

/// <summary>
/// 存储扩展类。
/// </summary>
public static class StorageExtensions
{
    internal static void EnsureFileDirectoryExists(this string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (directory == null || Directory.Exists(directory))
            return;
        Directory.CreateDirectory(directory);
    }

    internal static void EnsureDirectoryExists(this string directory)
    {
        if (string.IsNullOrEmpty(directory))
            return;
        if (Directory.Exists(directory))
            return;
        Directory.CreateDirectory(directory);
    }

    internal static string GetKey<TModel>(this TModel model)
    {
        if (model is IKey key)
            return key.Key;
        return string.Empty;
    }

    /// <summary>
    /// 判断当前文件或者扩展名称是否为图片格式。
    /// </summary>
    /// <param name="name">文件名称或者扩展名。</param>
    /// <returns>返回判断结果。</returns>
    public static bool IsImage(this string name)
    {
        if (!name.StartsWith('.'))
            name = Path.GetExtension(name);
        return ".jpg|.jpeg|.png|.gif|.jfif|.bmp|.tif|.tiff|.ico|.wbmp|.jpe|.rp|.fax|.net".IndexOf(name, StringComparison.OrdinalIgnoreCase) != -1;
    }

    /// <summary>
    /// 计算文件的哈希值。
    /// </summary>
    /// <param name="info">文件信息实例。</param>
    /// <returns>返回文件的哈希值。</returns>
    public static string GetMD5(this FileInfo info)
    {
        using var fs = new FileStream(info.FullName, FileMode.Open, FileAccess.Read);
        var md5 = MD5.Create();
        return md5.ComputeHash(fs).ToHexString();
    }
}
