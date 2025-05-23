namespace GtCores.Storages;

/// <summary>
/// 存储文件夹接口。
/// </summary>
public interface IStorageDirectory : ISingletonService
{
    /// <summary>
    /// 获取路径的物理路径。
    /// </summary>
    /// <param name="path">当前目录相对路径。</param>
    /// <returns>返回物理路径。</returns>
    string GetFullPath(string path);

    /// <summary>
    /// 创建目录，并且返回当前目录的物理路径。
    /// </summary>
    /// <param name="path">当前目录相对路径。</param>
    /// <returns>返回当前目录的物理路径。</returns>
    string CreateDirectory(string path);

    /// <summary>
    /// 获取文件信息实例。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回文件信息实例。</returns>
    FileInfo GetFile(string filePath);

    /// <summary>
    /// 读取字符串。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回读取结果。</returns>
    string? ReadFile(string filePath);

    /// <summary>
    /// 保存文件。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <param name="text">要保存的文本字符串。</param>
    void SaveFile(string filePath, string? text);

    /// <summary>
    /// 读取字符串。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回读取结果。</returns>
    Task<string?> ReadFileAsync(string filePath);

    /// <summary>
    /// 保存文件。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <param name="text">要保存的文本字符串。</param>
    Task SaveFileAsync(string filePath, string? text);

    /// <summary>
    /// 将文件实例保存到临时文件夹中。
    /// </summary>
    /// <param name="file">表单文件实例。</param>
    /// <returns>返回文件实例。</returns>
    Task<FileInfo> SaveToTempAsync(Stream file);

    /// <summary>
    /// 清理文件夹。
    /// </summary>
    void CleanStorages();
}
