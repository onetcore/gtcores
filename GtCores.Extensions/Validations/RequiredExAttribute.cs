namespace GtCores.Extensions.Validations
{
    /// <summary>
    /// 必须填写特性。
    /// </summary>
    public class RequiredExAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredExAttribute()
            : this("{0}必须填写。")
        {

        }

        /// <summary>
        /// 初始化类<see cref="RequiredExAttribute"/>。
        /// </summary>
        /// <param name="errorMessage">格式化错误消息。</param>
        public RequiredExAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
