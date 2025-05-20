namespace GtCores.Storages;

/// <summary>
///  数据存储接口。
/// </summary>
/// <typeparam name="TModel">数据模型类型。</typeparam>
public interface IDataStorage<TModel>
{
    /// <summary>
    /// 重新加载。
    /// </summary>
    void Reload();

    /// <summary>
    /// 获取模型存储实例。
    /// </summary>
    /// <returns>返回模型实例对象。</returns>
    TModel? GetData();

    /// <summary>
    /// 获取当前模型实例。
    /// </summary>
    /// <param name="key">唯一键。</param>
    /// <returns>返回模型实例对象。</returns>
    TModel? GetData(string key);

    /// <summary>
    /// 更新实例。
    /// </summary>
    /// <param name="model">实例模型。</param>
    /// <param name="saved">是否直接保存到存储文件中</param>
    void UpdateData(TModel model, bool saved = true);

    /// <summary>
    /// 更新列表。
    /// </summary>
    /// <param name="action">更新方法。</param>
    /// <param name="saved">是否直接保存到存储文件中</param>
    void UpdateData(Action<IEnumerable<TModel>> action, bool saved = true);

    /// <summary>
    /// 删除对象。
    /// </summary>
    /// <param name="predicate">条件表达式。</param>
    /// <param name="saved">是否保存到存储中。</param>
    void DeleteData(Func<TModel, bool> predicate, bool saved = true);

    /// <summary>
    /// 获取数据列表。
    /// </summary>
    /// <returns>返回数据列表实例。</returns>
    IEnumerable<TModel> AsEnumerable();
}
