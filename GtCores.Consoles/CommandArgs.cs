using System.Collections;

namespace GtCores.Consoles;

/// <summary>
/// 参数实例类。
/// </summary>
public class CommandArgs : IEnumerable<string>
{
    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回枚举器。</returns>
    public IEnumerator<string> GetEnumerator()
    {
        return _args.Keys.GetEnumerator();
    }

    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回枚举器。</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private readonly Dictionary<string, string?> _args = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string?> _values = new();

    /// <summary>
    /// 初始化类<see cref="CommandArgs"/>。
    /// </summary>
    /// <param name="args">参数字符串。</param>
    public CommandArgs(string args)
    {
        while (args.Length > 0)
        {
            switch (args[0])
            {
                case '-':
                    // parameter
                    {
                        var indx = args.IndexOf(' ');
                        if (indx == -1)//直接结束
                        {
                            _args[args[1..]] = null;
                            return;
                        }
                        var key = args[1..indx];
                        args = args[(indx + 1)..].Trim();
                        var value = Read(ref args);
                        _args[key] = value;
                    }
                    break;
                case '"':
                    // block
                    {
                        args = args[1..];
                        var value = ReadQuote(ref args);
                        _values.Add(value);
                    }
                    break;
                default:
                    {
                        var index = args.IndexOf(' ');
                        if (index == -1)//直接结束
                        {
                            _values.Add(args);
                            return;
                        }
                        _values.Add(args[..index]);
                        args = args[index..].Trim();
                    }
                    break;
            }
        }
    }

    private string? Read(ref string command)
    {
        if (command.Length == 0 || command[0] == '-')
            return null;

        if (command.StartsWith('"'))
        {
            command = command[1..];
            return ReadQuote(ref command);
        }

        var value = command;
        var index = command.IndexOf(' ');
        if (index != -1)
        {
            value = command[..index];
            command = command[(index + 1)..].Trim();
        }
        else
        {
            command = string.Empty;
        }
        return value?.Length == 0 ? null : value;
    }

    private string? ReadQuote(ref string command)
    {
        var index = command.IndexOf('"');
        if (index == -1)
        {
            ConsoleCore.ErrorLine($"参数错误: {command}");
            IsValid = false;
            return null;
        }
        while (index > 0 && command[index - 1] == '\\')
        {
            index = command.IndexOf('"', index + 1);
            if (index == -1)
            {
                ConsoleCore.ErrorLine($"参数错误: {command}");
                IsValid = false;
                return null;
            }
        }
        var value = command[..index];
        command = command[(index + 1)..].Trim();
        return value?.Length == 0 ? null : value;
    }

    /// <summary>
    /// 是否有效。
    /// </summary>
    public bool IsValid { get; private set; } = true;

    /// <summary>
    /// 是否为空。
    /// </summary>
    public bool IsEmpty => _args.Count == 0 && _values.Count == 0;

    /// <summary>
    /// 是否包含参数。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <returns>返回判断结果。</returns>
    public bool ContainsKey(string key)
    {
        return _args.ContainsKey(key);
    }

    /// <summary>
    /// 获取参数值。
    /// 如果参数不存在，则返回 null。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <returns>返回参数值。</returns>
    public string? this[string key]
    {
        get
        {
            if (_args.TryGetValue(key, out var value))
                return value;
            return null;
        }
    }

    /// <summary>
    /// 获取参数块或子命令。
    /// </summary>
    /// <param name="index">当前索引。</param>
    /// <returns>返回参数块或子命令，如果不存在返回null。</returns>
    public string? this[int index]
    {
        get
        {
            if (index >= _values.Count)
                return null;
            return _values[index];
        }
    }

    /// <summary>
    /// 参数块或子命令索引数量。
    /// </summary>
    public int Count => _values.Count;

    /// <summary>
    /// 获取参数值。
    /// 如果参数不存在，则返回 null。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <returns>返回参数值。</returns>
    public string? GetValue(string key)
    {
        _args.TryGetValue(key, out var value);
        return value;
    }

    /// <summary>
    /// 尝试获取参数值。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <param name="value">返回参数值。</param>
    /// <returns>返回是否包含参数。</returns>
    public bool TryGetValue(string key, out string? value) => _args.TryGetValue(key, out value);

    /// <summary>
    /// 获取参数值。
    /// 如果参数不存在，则返回 null。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <returns>返回参数值。</returns>
    public int? GetInt32(string key)
    {
        if (_args.TryGetValue(key, out var value)
            && int.TryParse(value, out var result))
            return result;
        return null;
    }

    /// <summary>
    /// 获取枚举值。
    /// </summary>
    /// <typeparam name="TEnum">枚举类型。</typeparam>
    /// <param name="key">参数名称。</param>
    /// <returns>返回参数值。</returns>
    public TEnum GetValue<TEnum>(string key) where TEnum : struct
    {
        if (_args.TryGetValue(key, out var value)
            && Enum.TryParse<TEnum>(value, true, out var result))
            return result;
        return default;
    }

    /// <summary>
    /// 转换物理路径。
    /// </summary>
    /// <param name="path">当前路径。</param>
    /// <returns>返回当前路径的物理路径。</returns>
    public string MapPath(string? path)
    {
        if (path == null || path.Length == 0)
            return ConsoleCore.WorkingDirectory;
        if (path.StartsWith("~/"))
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), path[2..]);
        if (path.StartsWith('/') || path.IndexOf(':') == 1)
            return path;
        return Path.Combine(ConsoleCore.WorkingDirectory, path);
    }

    /// <summary>
    /// 获取名称中的任何一个值，主要用于参数简写。
    /// </summary>
    /// <param name="keys">名称集合，只要参数中包含一个就会返回当前值。</param>
    /// <returns>返回参数值，如果不存在返回<c>null</c>。</returns>
    public string? GetValue(params string[] keys)
    {
        foreach (var key in keys)
        {
            if (_args.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        return null;
    }

    /// <summary>
    /// 参数字符串。
    /// </summary>
    /// <returns>返回参数字符串。</returns>
    public override string ToString()
    {
        string Format(string? value)
        {
            if (value == null)
                return "";

            if (value.IndexOf(' ') != -1)
                value = $"\"{value}\"";
            return " " + value;
        }

        var command = "";
        if (_args.Count > 0)
        {
            command += ' ';
            command += string.Join(" ", _args.Select(x => $"-{x.Key}{Format(x.Value)}"));
        }
        if (_values.Count > 0)
        {
            command += ' ';
            command += string.Join(" ", _values.Select(Format));
        }
        return command;
    }

    public static implicit operator CommandArgs(string args) => new CommandArgs(args);
}