namespace GtCores.Components;

/// <summary>
/// Bootstrap 颜色类型。
/// </summary>
public enum BsType
{
    /// <summary>
    /// 默认样式。
    /// </summary>
    Primary,
    /// <summary>
    /// 次要样式。
    /// </summary>
    Secondary,
    /// <summary>
    /// 成功样式。
    /// </summary>
    Success,
    /// <summary>
    /// 危险样式。
    /// </summary>
    Danger,
    /// <summary>
    /// 警告样式。
    /// </summary>
    Warning,
    /// <summary>
    /// 信息样式。
    /// </summary>
    Info,
    /// <summary>
    /// 浅色样式。
    /// </summary>
    Light,
    /// <summary>
    /// 深色样式。
    /// </summary>
    Dark,
}

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