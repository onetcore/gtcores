using System.Collections.Concurrent;
using System.Reflection;

namespace GtCores.Storages;

/// <summary>
///  数据存储实现类。
/// </summary>
/// <typeparam name="TModel">数据模型类型。</typeparam>
public class DataStorage<TModel> : IDataStorage<TModel>
{
    private readonly IStorageDirectory _storageDirectory;
    private readonly Mutex _mutex = new(true, typeof(TModel).FullName);
    private ConcurrentDictionary<string, TModel>? _storages;
    /// <summary>
    /// 当前存储的数据列表。
    /// </summary>
    protected ConcurrentDictionary<string, TModel> Storeages
    {
        get
        {
            if (_storages == null)
                LoadStorages();
            return _storages!;
        }
    }

    /// <summary>
    /// 当前存储的物理路径。
    /// </summary>
    protected string FullPath { get; }

    /// <summary>
    /// 初始化类<see cref="DataStorage{TModel}"/>的新实例。
    /// </summary>
    /// <param name="storageDirectory">存储接口。</param>
    public DataStorage(IStorageDirectory storageDirectory)
    {
        _storageDirectory = storageDirectory;
        var type = typeof(TModel);
        var name = type.GetCustomAttribute<StorageAttribute>()?.Name ?? type.Name;
        FullPath = $"data/{type.Assembly.GetName().Name}/{name.ToLower()}.db";
    }

    /// <summary>
    /// 重新加载。
    /// </summary>
    public virtual void Reload()
    {
        _storages = null;
    }

    /// <summary>
    /// 加载存储数据实例列表。
    /// </summary>
    protected virtual void LoadStorages()
    {
        _storages = new ConcurrentDictionary<string, TModel>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var stored = _storageDirectory.ReadFile(FullPath);
            if (string.IsNullOrWhiteSpace(stored))
                return;
            stored = Cores.Decrypto(stored);
            var models = Cores.FromJsonString<List<TModel>>(stored)!;
            foreach (var model in models)
            {
                _storages.AddOrUpdate(model.GetKey(), _ => model, (_, __) => model);
            }
        }
        catch { }
    }

    /// <summary>
    /// 获取模型存储实例。
    /// </summary>
    /// <returns>返回模型实例对象。</returns>
    public TModel? GetData()
    {
        return GetData(string.Empty);
    }

    /// <summary>
    /// 获取当前模型实例。
    /// </summary>
    /// <param name="key">唯一键。</param>
    /// <returns>返回模型实例对象。</returns>
    public TModel? GetData(string key)
    {
        Storeages.TryGetValue(key, out var model);
        return model;
    }

    /// <summary>
    /// 更新实例。
    /// </summary>
    /// <param name="model">实例模型。</param>
    /// <param name="saved">是否直接保存到存储文件中</param>
    public void UpdateData(TModel model, bool saved = true)
    {
        var key = model.GetKey();
        Storeages.AddOrUpdate(key, _ => model, (_, __) => model);
        if (saved) SaveFile();
    }

    /// <summary>
    /// 更新列表。
    /// </summary>
    /// <param name="action">更新方法。</param>
    /// <param name="saved">是否直接保存到存储文件中</param>
    public void UpdateData(Action<IEnumerable<TModel>> action, bool saved = true)
    {
        action(Storeages.Values);
        if (saved) SaveFile();
    }

    /// <summary>
    /// 保存存储文件。
    /// </summary>
    protected void SaveFile()
    {
        try
        {
            _mutex.WaitOne();
            var json = _storages.ToJsonString()!;
            json = Cores.Encrypto(json);
            _storageDirectory.SaveFile(FullPath, json);
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// 删除对象。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="saved">是否保存到存储中。</param>
    public void DeleteData(Func<TModel, bool> predicate, bool saved = true)
    {
        var deletings = Storeages.Values.Where(predicate).Select(x => x.GetKey());
        foreach (var key in deletings)
        {
            Storeages.TryRemove(key, out _);
        }
        if (saved) SaveFile();
    }

    /// <summary>
    /// 获取数据列表。
    /// </summary>
    /// <returns>返回数据列表实例。</returns>
    public IEnumerable<TModel> AsEnumerable()
    {
        return Storeages.Values;
    }
}
