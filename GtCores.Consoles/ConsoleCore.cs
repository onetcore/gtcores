using System.Collections.Concurrent;
using System.Reflection;

namespace GtCores.Consoles;

/// <summary>
/// 扩展类。
/// </summary>
public static class ConsoleCore
{
    /// <summary>
    /// 控制台使用全局取消标识。
    /// </summary>
    public static CancellationTokenSource TokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken());

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    public static void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void WriteLine(string message, params object?[] args)
    {
        Console.WriteLine(message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    public static void Write(string message)
    {
        Console.Write(message);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void Write(string message, params object?[] args)
    {
        Console.Write(message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="color">颜色。</param>
    /// <param name="message">输出信息。</param>
    public static void WriteLine(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

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
    /// <param name="color">颜色。</param>
    /// <param name="message">输出信息。</param>
    public static void Write(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ResetColor();
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

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void InfoLine(string message)
    {
        WriteLine(ConsoleColor.DarkYellow, message);
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
    public static void Info(string message)
    {
        Write(ConsoleColor.DarkYellow, message);
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
    public static void ErrorLine(string message)
    {
        WriteLine(ConsoleColor.DarkRed, message);
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
    public static void Error(string message)
    {
        WriteLine(ConsoleColor.DarkRed, message);
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
    /// <param name="message">输出信息。</param>
    public static void SuccessLine(string message)
    {
        WriteLine(ConsoleColor.DarkGreen, message);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void SuccessLine(string message, params object?[] args)
    {
        WriteLine(ConsoleColor.DarkGreen, message, args);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    public static void Success(string message)
    {
        WriteLine(ConsoleColor.DarkGreen, message);
    }

    /// <summary>
    /// 输出信息。
    /// </summary>
    /// <param name="message">输出信息。</param>
    /// <param name="args">参数。</param>
    public static void Success(string message, params object?[] args)
    {
        WriteLine(ConsoleColor.DarkGreen, message, args);
    }

    private static readonly ConcurrentDictionary<string, string?> _envs = new(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// 当前工作目录:wd。
    /// </summary>
    public static string WorkingDirectory { get => _envs.GetOrAdd("wd", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))!; set => UpdateValue("wd", value); }

    /// <summary>
    /// 获取全局变量。
    /// </summary>
    /// <param name="key">变量唯一键。</param>
    /// <returns>返回全局变量值。</returns>
    public static string? GetValue(string key) => _envs.TryGetValue(key, out var value) ? value : null;

    /// <summary>
    /// 尝试获取全局变量。
    /// </summary>
    /// <param name="key">变量唯一键。</param>
    /// <param name="value">变量值。</param>
    /// <returns>返回是否包含变量值。</returns>
    public static bool TryGetValue(string key, out string? value) => _envs.TryGetValue(key, out value);

    /// <summary>
    /// 更新全局变量。
    /// </summary>
    /// <param name="key">变量唯一键。</param>
    /// <param name="value">变量值。</param>
    public static void UpdateValue(string key, string? value)
    {
        if (value == null) _envs.TryRemove(key, out _);
        else _envs.AddOrUpdate(key, value, (_, __) => value);
    }

    /// <summary>
    /// 显示当前全局变量。
    /// </summary>
    public static void ShowValues()
    {
        Console.WriteLine("当前全局变量:");
        foreach (var kv in _envs)
        {
            Console.WriteLine($" {kv.Key} : {kv.Value}");
        }
    }

    private static readonly List<string> _histories = [];
    /// <summary>
    /// 读取一行字符串。
    /// </summary>
    /// <returns>返回读取结果。</returns>
    public static string ReadLine()
    {
        if (Console.BufferWidth == 0)
            return Console.ReadLine() ?? string.Empty;
        var command = "";
        var hIndex = _histories.Count;
        var key = Console.ReadKey(true);
        while (true)
        {
            var historied = false;
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (command.Length > 0)
                    {
                        command = command[..^1];
                        Console.Write("\b \b");
                    }
                    break;
                case ConsoleKey.UpArrow:
                    hIndex--;
                    if (hIndex < 0)
                        hIndex = _histories.Count - 1;
                    if (hIndex < 0)
                    {
                        hIndex = 0;
                        break;
                    }
                    if (_histories.Count > hIndex)
                    {
                        command = _histories[hIndex];
                        historied = true;
                    }
                    OverWrite(command);
                    break;
                case ConsoleKey.DownArrow:
                    hIndex++;
                    if (hIndex >= _histories.Count)
                        hIndex = 0;
                    if (_histories.Count > hIndex)
                    {
                        command = _histories[hIndex];
                        historied = true;
                    }
                    OverWrite(command);
                    break;
                case ConsoleKey.LeftArrow:
                    {
                        var current = Console.GetCursorPosition();
                        Console.SetCursorPosition(Math.Max(0, current.Left - 1), current.Top);
                    }
                    break;
                case ConsoleKey.RightArrow:
                    {
                        var current = Console.GetCursorPosition();
                        Console.SetCursorPosition(Math.Min(command.Length, current.Left + 1), current.Top);
                    }
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine();
                    if (historied)
                        _histories.RemoveAt(hIndex);
                    _histories.Add(command);
                    return command;
                default:
                    command += key.KeyChar;
                    Console.Write(key.KeyChar);
                    break;
            }
            key = Console.ReadKey(true);
        }
        void OverWrite(string command)
        {
            while (Console.CursorLeft > 0)
            {
                Console.Write("\b \b");
            }
            Console.Write(command);
        }
    }

    /// <summary>
    /// 显示描述。
    /// </summary>
    /// <param name="text">要显示的字符串。</param>
    /// <param name="s">附加符。</param>
    public static void ShowLine(string? text = null, char s = '#')
    {
        if (text == null)
        {
            Console.WriteLine(new string(s, Console.BufferWidth));
            return;
        }
        Console.Write(s);
        Console.Write(' ');
        Console.Write(text);
        Console.SetCursorPosition(Math.Max(Console.BufferWidth - 1, 0), Console.CursorTop);
        Console.Write(s);
        Console.WriteLine();
    }
}

