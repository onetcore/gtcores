using System.Linq.Expressions;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace GtCores.Storages;

/// <summary>
/// 存储文件夹。
/// </summary>
public class StorageDirectory : IStorageDirectory
{
    private readonly string _currentDirectory;
    private readonly string _tempDirectory;
    /// <summary>
    /// 初始化类<see cref="StorageDirectory"/>。
    /// </summary>
    /// <param name="configuration">配置接口。</param>
    public StorageDirectory(IConfiguration configuration)
    {
        var storageDirectory = configuration["StorageDir"]?.Trim() ?? "../storages";
        if (storageDirectory.Length < 2)
            _currentDirectory = Environment.CurrentDirectory;
        else if (storageDirectory.StartsWith("~/"))
            _currentDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), storageDirectory[2..]);
        if (storageDirectory[0] == '/' || storageDirectory[1] == ':')
            _currentDirectory = storageDirectory;
        else
            _currentDirectory = Path.Combine(Environment.CurrentDirectory, storageDirectory);
        _currentDirectory.EnsureDirectoryExists();
        _tempDirectory = Path.Combine(_currentDirectory, "temp");
        _tempDirectory.EnsureDirectoryExists();
    }

    /// <summary>
    /// 获取路径的物理路径。
    /// </summary>
    /// <param name="path">当前目录相对路径。</param>
    /// <returns>返回物理路径。</returns>
    public virtual string GetFullPath(string path)
    {
        return Path.Combine(_currentDirectory, path);
    }

    /// <summary>
    /// 创建目录，并且返回当前目录的物理路径。
    /// </summary>
    /// <param name="path">当前目录路径。</param>
    /// <returns>返回当前目录的物理路径。</returns>
    public string CreateDirectory(string path)
    {
        path = GetFullPath(path);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    /// <summary>
    /// 获取文件信息实例。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回文件信息实例。</returns>
    public FileInfo GetFile(string filePath)
    {
        filePath = GetFullPath(filePath);
        return new FileInfo(filePath);
    }

    /// <summary>
    /// 读取字符串。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回读取结果。</returns>
    public virtual string? ReadFile(string filePath)
    {
        filePath = GetFullPath(filePath);
        if (File.Exists(filePath))
            return File.ReadAllText(filePath, Encoding.UTF8);
        return null;
    }

    /// <summary>
    /// 保存文件。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <param name="text">要保存的文本字符串。</param>
    public virtual void SaveFile(string filePath, string? text)
    {
        filePath = GetFullPath(filePath);
        filePath.EnsureFileDirectoryExists();
        using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var sw = new StreamWriter(fs, Encoding.UTF8);
        sw.Write(text);
    }

    /// <summary>
    /// 读取字符串。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <returns>返回读取结果。</returns>
    public virtual async Task<string?> ReadFileAsync(string filePath)
    {
        filePath = GetFullPath(filePath);
        if (File.Exists(filePath))
            return await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return null;
    }

    /// <summary>
    /// 保存文件。
    /// </summary>
    /// <param name="filePath">文件路径。</param>
    /// <param name="text">要保存的文本字符串。</param>
    public virtual Task SaveFileAsync(string filePath, string? text)
    {
        filePath = GetFullPath(filePath);
        filePath.EnsureFileDirectoryExists();
        using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var sw = new StreamWriter(fs, Encoding.UTF8);
        return sw.WriteAsync(text);
    }

    /// <summary>
    /// 将文件实例保存到临时文件夹中。
    /// </summary>
    /// <param name="file">表单文件实例。</param>
    /// <returns>返回文件实例。</returns>
    public virtual async Task<FileInfo> SaveToTempAsync(Stream file)
    {
        var info = new FileInfo(Path.Combine(_tempDirectory, Guid.NewGuid().ToString("N")));
        using var fs = new FileStream(info.FullName, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(fs);
        return info;
    }

    /// <summary>
    /// 清理文件夹。
    /// </summary>
    public virtual void CleanStorages()
    {
        var infos = new DirectoryInfo(_currentDirectory).GetDirectories("*", SearchOption.TopDirectoryOnly);
        foreach (var info in infos)
        {
            if (info.Name.Equals("temp", StringComparison.OrdinalIgnoreCase))
            {
                var expired = DateTime.Today.AddDays(-1);
                info.GetFiles()
                    .Where(x => x.LastAccessTime <= expired)
                    .ForEach(x => x.Delete());
                info.GetDirectories()
                    .Where(x => x.LastAccessTime <= expired)
                    .ForEach(x => x.Delete(true));
                continue;
            }

            var directories = info.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                if (!directory.Exists)
                {
                    continue;
                }

                var files = directory.GetFiles("*", SearchOption.AllDirectories);
                if (files.Length == 0)
                {
                    try { directory.Delete(true); }
                    catch { }
                }
            }
        }
    }
}
