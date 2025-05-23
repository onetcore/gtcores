namespace GtCores.Storages;

/// <summary>
/// 存储文件接口。
/// </summary>
public interface IHashedStorage : ISingletonService
{
    /// <summary>
    /// 上传文件。
    /// </summary>
    /// <param name="stream">文件流。</param>
    /// <param name="extension">文件扩展名。</param>
    /// <param name="cancellationToken">取消令牌。</param>
    /// <returns>返回上传文件实例。</returns>
    Task<HashedFile?> UpdateAsync(Stream stream, string extension, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取文件实例。
    /// </summary>
    /// <param name="fileId">文件Id。</param>
    /// <returns>返回文件实例。</returns>
    HashedFile? GetFile(string fileId);
}
