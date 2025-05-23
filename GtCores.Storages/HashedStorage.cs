namespace GtCores.Storages;

/// <summary>
/// 存储文件类。
/// </summary>
public class HashedStorage : IHashedStorage
{
    private readonly IStorageDirectory _storageDirectory;
    private readonly IDataStorage<HashedFile> _dataStorage;
    private const string _directory = "medias";

    /// <summary>
    /// 初始化类<see cref="HashedStorage"/>的新实例。
    /// </summary>
    public HashedStorage(IStorageDirectory storageDirectory, IDataStorage<HashedFile> dataStorage)
    {
        storageDirectory.CreateDirectory(_directory);
        _storageDirectory = storageDirectory;
        _dataStorage = dataStorage;
    }

    /// <summary>
    /// 上传文件。
    /// </summary>
    /// <param name="stream">文件流。</param>
    /// <param name="extension">文件扩展名。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>返回上传文件实例。</returns>
    public virtual async Task<HashedFile?> UpdateAsync(Stream stream, string extension, CancellationToken cancellationToken = default)
    {
        if (stream.Length == 0)
            return null;
        var fileInfo = await _storageDirectory.SaveToTempAsync(stream, cancellationToken);
        if (cancellationToken.IsCancellationRequested)
            return null;
        var file = new HashedFile
        {
            Extension = extension,
            FileSize = fileInfo.Length,
            FileId = fileInfo.GetMD5()
        };
        var filePath = Path.Combine(_storageDirectory.GetFullPath(_directory), file.FileId);
        if (File.Exists(filePath))
            return file;
        fileInfo.MoveTo(filePath);
        _dataStorage.UpdateData(file);
        return file;
    }

    /// <summary>
    /// 获取文件实例。
    /// </summary>
    /// <param name="fileId">文件Id。</param>
    /// <returns>返回文件实例。</returns>
    public virtual HashedFile? GetFile(string fileId)
    {
        var file = _dataStorage.GetData(fileId);
        if (_storageDirectory.Exists($"{_directory}/{fileId}"))
            return file;
        return null;
    }
}