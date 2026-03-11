namespace GtCores.Extensions.Validations
{
    /// <summary>
    /// 字符串长度设定特性。
    /// </summary>
    public class StringLengthExAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        /// <summary>
        /// 初始化类<see cref="StringLengthExAttribute"/>。
        /// </summary>
        /// <param name="min">最小长度。</param>
        /// <param name="max">最大长度。</param>
        /// <param name="errorMessage">格式化错误消息。</param>
        public StringLengthExAttribute(int min, int max, string? errorMessage = null) : this(max, errorMessage)
        {
            MinimumLength = min;
        }

        /// <summary>
        /// 初始化类<see cref="StringLengthExAttribute"/>。
        /// </summary>
        /// <param name="max">最大长度。</param>
        /// <param name="errorMessage">格式化错误消息。</param>
        public StringLengthExAttribute(int max, string? errorMessage = null) : base(max)
        {
            ErrorMessage = errorMessage ?? "{0}最少由{2}到{1}位字符组成。";
        }
    }
}
