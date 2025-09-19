namespace GtCores.Components;

public static class BsExtensions
{
    /// <summary>
    /// 获取 Bootstrap 颜色类型的 CSS 类名。
    /// </summary>
    /// <param name="type">Bootstrap 颜色类型。</param>
    /// <returns>对应的 CSS 类名。</returns>
    public static string ToCssClass(this BsType type) => type switch
    {
        BsType.Primary => "primary",
        BsType.Secondary => "secondary",
        BsType.Success => "success",
        BsType.Danger => "danger",
        BsType.Warning => "warning",
        BsType.Info => "info",
        BsType.Light => "light",
        BsType.Dark => "dark",
        _ => "primary",
    };

    /// <summary>
    /// 获取 Bootstrap 警告框的 CSS 类名。
    /// </summary>
    /// <param name="type">颜色类型。</param>
    /// <returns>返回警告框的 CSS 类名。</returns>
    public static string ToAlertCssClass(this BsType type) => $"alert alert-{type.ToCssClass()}";
}