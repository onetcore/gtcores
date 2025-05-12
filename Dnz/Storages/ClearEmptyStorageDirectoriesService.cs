using Dnz.Properties;

namespace Dnz.Storages;

/// <summary>
/// 存储文件夹清理服务。
/// </summary>
public class ClearEmptyStorageDirectoriesService : BackgroundService
{
    private readonly IStorageDirectory _storageDirectory;
    /// <summary>
    /// 初始化类<see cref="ClearEmptyStorageDirectoriesService"/>。
    /// </summary>
    /// <param name="storageDirectory">存储文件夹接口。</param>
    public ClearEmptyStorageDirectoriesService(IStorageDirectory storageDirectory)
    {
        _storageDirectory = storageDirectory;
    }

    /// <summary>
    /// 名称。
    /// </summary>
    public override string Name => Resources.StorageClearTaskExecutor_Name;

    /// <summary>
    /// 描述。
    /// </summary>
    public override string Description => Resources.StorageClearTaskExecutor_Description;

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <param name="argument">参数。</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ExecuteCoreAsync(stoppingToken);
            }
            finally
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

    /// <summary>
    /// 执行清除任务。
    /// </summary>
    /// <param name="stoppingToken">取消标志。</param>
    protected virtual Task ExecuteCoreAsync(CancellationToken stoppingToken)
    {
        _storageDirectory.ClearEmptyDirectories();
        return Task.CompletedTask;
    }
}