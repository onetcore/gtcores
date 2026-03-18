using Microsoft.AspNetCore.Components;

namespace GSites.Extensions.Components
{
    /// <summary>
    /// Tab接口。
    /// </summary>
    public interface ITab
    {
        /// <summary>
        /// 子内容。
        /// </summary>
        RenderFragment? ChildContent { get; }
    }
}
