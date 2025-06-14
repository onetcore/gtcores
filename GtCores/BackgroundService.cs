﻿namespace GtCores;

/// <summary>
/// 后台服务基类。
/// 注意：如果在含有数据库的后台服务，需要在重写<see cref="ExecuteAsync"/>时调用如下代码以等待数据库迁移结束后再执行。
/// <![CDATA[
/// await cancellationToken.WaitDataMigrationCompletedAsync();
/// ]]>
/// </summary>
public abstract class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService, IServices
{
    /// <summary>
    /// 名称。
    /// </summary>
    public virtual string Name => GetType().Name;

    /// <summary>
    /// 描述。
    /// </summary>
    public virtual string? Description { get; }

    private bool _isRunning;
    /// <summary>
    /// 是否正在运行。
    /// </summary>
    public bool IsRunning
    {
        get => _isRunning;
        protected set
        {
            _isRunning = value;
            if (_isRunning)
                StartDate ??= DateTimeOffset.Now;
            else
                CompletedDate ??= DateTimeOffset.Now;
        }
    }

    /// <summary>
    /// 开始运行时间。
    /// </summary>
    public DateTimeOffset? StartDate { get; private set; }

    /// <summary>
    /// 完成运行时间。
    /// </summary>
    public DateTimeOffset? CompletedDate { get; private set; }

    /// <summary>
    /// 运行时长。
    /// </summary>
    public TimeSpan RunningTime => (CompletedDate ?? DateTimeOffset.Now) - StartDate ?? TimeSpan.Zero;

    /// <summary>
    /// 开始运行。
    /// </summary>
    /// <param name="cancellationToken">取消标志。</param>
    /// <returns>返回当前执行任务。</returns>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        IsRunning = true;
        await base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// 结束运行。
    /// </summary>
    /// <param name="cancellationToken">取消标志。</param>
    /// <returns>返回当前执行任务。</returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        IsRunning = false;
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        IsRunning = false;
    }
}