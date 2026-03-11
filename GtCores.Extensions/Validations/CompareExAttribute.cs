namespace GtCores.Extensions.Validations
{
    /// <summary>
    /// 对比特性。
    /// </summary>
    public class CompareExAttribute : System.ComponentModel.DataAnnotations.CompareAttribute
    {
        /// <summary>
        /// 初始化类<see cref="CompareExAttribute"/>。
        /// </summary>
        /// <param name="otherProperty">对比属性名称。</param>
        public CompareExAttribute(string otherProperty)
            : this(otherProperty, "{0}和{1}不匹配。")
        {

        }

        /// <summary>
        /// 初始化类<see cref="CompareExAttribute"/>。
        /// </summary>
        /// <param name="otherProperty">对比属性名称。</param>
        /// <param name="errorMessage">格式化错误消息。</param>
        public CompareExAttribute(string otherProperty, string errorMessage) : base(otherProperty)
        {
            ErrorMessage = errorMessage;
        }
    }
}
