using GtCores.Consoles;
using System.Text;

namespace Gt;

public class BiCommandHandler : CommandHandlerBase
{
    /// <summary>
    /// 描述。
    /// </summary>
    public override string Description => "生成bootstrap图标的枚举代码文件。";

    /// <summary>
    /// 执行命令。
    /// </summary>
    public override async Task ExecuteAsync(CommandArgs args, CancellationToken token = default)
    {
        var currentDirectory = GetCurrentDirectory();
        var iconFile = currentDirectory.GetFiles("bootstrap-icons.json", SearchOption.AllDirectories).FirstOrDefault();
        if (iconFile == null || !iconFile.Exists)
        {
            ConsoleCore.ErrorLine("bootstrap-icons.json 文件不存在。");
            return;
        }
        var iconsJson = await File.ReadAllTextAsync(iconFile.FullName, token);
        var icons = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(iconsJson)?.Select(x => x.Key).ToList();
        if (icons == null || icons.Count == 0)
        {
            ConsoleCore.ErrorLine("未能解析出图标列表。");
            return;
        }

        currentDirectory = GetProjectDirectory(iconFile.Directory!);
        using var fs = new FileStream(Path.Combine(currentDirectory.FullName, "IconName.cs"), FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(fs, Encoding.UTF8);
        await writer.WriteLineAsync($@"using System.ComponentModel;

namespace {currentDirectory.Name};

/// <summary>
/// {icons.Count} bootstrap icons for csharp.
/// </summary>
public enum IconName
{{");
        foreach (var icon in icons)
        {
            var enumName = ToPascalCase(icon);
            await writer.WriteLineAsync($"    /// <summary>");
            await writer.WriteLineAsync($"    /// {icon} 图标。");
            await writer.WriteLineAsync($"    /// </summary>");
            await writer.WriteLineAsync($"    {enumName},");
        }
        await writer.WriteLineAsync("}");
        ConsoleCore.SuccessLine("生成 IconName.cs 文件完成。");
    }

    private string ToPascalCase(string str)
    {
        var parts = str.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
        var pascalCase = string.Concat(parts.Select(p => char.IsDigit(p[0]) ? $"N{p}" : char.ToUpperInvariant(p[0]) + p.Substring(1)));
        return pascalCase;
    }

    public override void ShowHelp()
    {

    }
}
