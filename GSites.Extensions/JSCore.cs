using Microsoft.JSInterop;

namespace GSites.Extensions
{
    /// <summary>
    /// 脚本操作类。
    /// </summary>
    /// <param name="jsRuntime"></param>
    public class JSCore(IJSRuntime jsRuntime) : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/GSites.Extensions/gsitecore.js").AsTask());
        /// <summary>
        /// 设置body元素属性。
        /// </summary>
        /// <param name="name">属性名称。</param>
        /// <param name="value">属性值。</param>
        public async ValueTask SetBodyAttribute(string name, string value)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setBodyAttribute", name, value);
        }

        /// <summary>
        /// 移除body元素属性。
        /// </summary>
        /// <param name="name">属性名称。</param>
        public async ValueTask RemoveBodyAttribute(string name)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setBodyAttribute", name);
        }

        /// <summary>
        /// 设置模板名称。
        /// </summary>
        /// <param name="themeName">模板名称。</param>
        public ValueTask SetThemeName(string? themeName = null)
        {
            const string name = "data-bs-theme";
            if (string.IsNullOrWhiteSpace(themeName))
            {
                return RemoveBodyAttribute(name);
            }
            else
            {
                return SetBodyAttribute(name, themeName);
            }
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
