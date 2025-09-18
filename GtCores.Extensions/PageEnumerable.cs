using System.Collections;

namespace GtCores.Extensions;

internal class PageEnumerable<TEntity> : IPageEnumerable<TEntity> where TEntity : class
{
    private readonly List<TEntity> _items = new List<TEntity>();
    public void AddRange(IEnumerable<TEntity> items)
    {
        _items.AddRange(items);
    }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public IEnumerator<TEntity> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}