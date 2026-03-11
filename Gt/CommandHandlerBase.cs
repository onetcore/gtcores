using GtCores.Consoles;

namespace Gt;

/// <summary>
/// 命令处理基类。
/// </summary>
public abstract class CommandHandlerBase : CommandHandler
{
    /// <summary>
    /// 获取当前项目根目录。
    /// </summary>
    /// <returns>返回项目根目录。</returns>
    protected DirectoryInfo GetCurrentDirectory()
    {
        var currentDir = new DirectoryInfo(ConsoleCore.WorkingDirectory);
        var solutionDir = currentDir;
        while (solutionDir != null && !solutionDir.GetFiles("*.sln", SearchOption.TopDirectoryOnly).Any())
        {
            solutionDir = solutionDir.Parent;
        }
        return solutionDir ?? currentDir;
    }

    /// <summary>
    /// 获取项目目录。
    /// </summary>
    /// <param name="info">当前文件所在的目录。</param>
    /// <returns>返回项目目录。</returns>
    protected DirectoryInfo GetProjectDirectory(DirectoryInfo info)
    {
        var projectDir = info;
        while (projectDir != null && !projectDir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).Any())
        {
            projectDir = projectDir.Parent;
        }
        return projectDir ?? info;
    }
}
