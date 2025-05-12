namespace Dnzer;

/// <summary>
/// 参数实例类。
/// </summary>
public class Arguments
{
    private readonly IDictionary<string, string?> _args = new Dictionary<string, string?>();

    public Arguments(DirectoryInfo currentDirectory, string args)
    {
        CurrentDirectory = currentDirectory;
        if (args.Length == 0)
            return;
        var index = args.IndexOf(' ');
        while (index != -1)
        {
            var key = args[..index];
            if (key.StartsWith("-"))
                key = key[1..];
            else
            {
                Cores.WriteLine(ConsoleColor.DarkRed, $"参数错误: {key}");
                IsValid = false;
                break;
            }
            args = args[(index + 1)..].Trim();
            var value = Read(ref args);
            _args.Add(key, value);
            index = args.IndexOf(' ');
        }
        if (args.Length > 0)
        {
            if (args[0] == '-')
                _args.Add(args[1..], null);
            else
            {
                Cores.WriteLine(ConsoleColor.DarkRed, $"参数错误: {args}");
                IsValid = false;
                return;
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
            Cores.WriteLine(ConsoleColor.DarkRed, $"参数错误: {command}");
            IsValid = false;
            return null;
        }
        while (index > 0 && command[index - 1] == '\\')
        {
            index = command.IndexOf('"', index + 1);
            if (index == -1)
            {
                Cores.WriteLine(ConsoleColor.DarkRed, $"参数错误: {command}");
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
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 当前执行的解决方案目录，如果不存在就为当前目录。
    /// </summary>
    public DirectoryInfo CurrentDirectory { get; }

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
    /// 获取参数值。
    /// 如果参数不存在，则返回 null。
    /// </summary>
    /// <param name="key">参数名称。</param>
    /// <returns>返回参数值。</returns>
    public string? GetValue(string key)
    {
        if (_args.TryGetValue(key, out var value))
            return value;
        return null;
    }
}