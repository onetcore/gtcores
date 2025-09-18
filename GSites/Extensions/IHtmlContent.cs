using GtCores.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace GSites.Extensions;
/// <summary>
/// 包含HTML内容的接口。
/// </summary>
public interface IHtmlContent
{
    /// <summary>
    /// HTML内容。
    /// </summary>
    string? HtmlContent { get; set; }

    /// <summary>
    /// 文本内容。
    /// </summary>
    string? TextContent { get; set; }
}
