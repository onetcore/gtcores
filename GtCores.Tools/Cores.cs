namespace GtCores.Tools;

/// <summary>
/// 扩展类。
/// </summary>
public static class Cores
{
    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="color">颜色。</param>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void WriteLine(ConsoleColor color, string message, params object?[] args)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message, args);
        Console.ResetColor();
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void InfoLine(string message, params object?[] args)
    {
        WriteLine(ConsoleColor.DarkYellow, message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void Info(string message, params object?[] args)
    {
        Write(ConsoleColor.DarkYellow, message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void ErrorLine(string message, params object?[] args)
    {
        WriteLine(ConsoleColor.DarkRed, message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void Error(string message, params object?[] args)
    {
        WriteLine(ConsoleColor.DarkRed, message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="color">颜色。</param>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void Write(ConsoleColor color, string message, params object?[] args)
    {
        Console.ForegroundColor = color;
        Console.Write(message, args);
        Console.ResetColor();
    }
}