namespace Dnz.Storages;

/// <summary>
/// 扩展类。
/// </summary>
public static class StorageHelper
{
    /// <summary>
    /// 获取当前路径是否为绝对路径。
    /// </summary>
    /// <param name="path">当前路径实例。</param>
    /// <param name="directoryName">父级文件夹名称。</param>
    /// <returns>返回当前路径是否为绝对路径。</returns>
    public static string MapPath(string path, string? directoryName = null)
    {
        directoryName ??= Directory.GetCurrentDirectory();
        if (path.StartsWith("~/"))//虚拟目录
        {
            path = Path.Combine(directoryName, path[2..]);
        }
        else if (!IsPhysicalPath(path))//物理目录
        {
            path = Path.Combine(directoryName, path);
        }

        return new DirectoryInfo(path).FullName;
    }

    /// <summary>
    /// 判断当前路径是否为物理路径。
    /// </summary>
    /// <param name="path">当前路径。</param>
    /// <returns>返回判断结果。</returns>
    public static bool IsPhysicalPath(string path)
    {
        return path.Length > 3 && path[1] == ':' && path[2] == '\\';
    }
}