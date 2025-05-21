using System.Collections.Concurrent;
using System.Reflection;

namespace GtCores.Consoles;

/// <summary>
/// 命令处理器工厂实现类。
/// </summary>
public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly ConcurrentDictionary<string, ICommandHandler> _commandHandlers;

    /// <summary>
    /// 初始化类<see cref="CommandHandlerFactory"/>。
    /// </summary>
    /// <param name="commandHandlers">命令处理器列表。</param>
    public CommandHandlerFactory(IEnumerable<ICommandHandler> commandHandlers)
    {
        _commandHandlers =
            new ConcurrentDictionary<string, ICommandHandler>(commandHandlers.ToDictionary(x => x.Name),
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 执行方法。
    /// </summary>
    /// <returns>返回执行任务。</returns>
    public async Task StartAsync()
    {
        var assembly = Assembly.GetEntryAssembly()!.GetName();
        Console.Title = $"欢迎使用 {assembly.Name} 命令集";
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        ConsoleCore.ShowLine();
        ConsoleCore.ShowLine(" 1. 可使用 help 命令查看帮助。");
        ConsoleCore.ShowLine(" 2. 可使用 exit 或 quit 退出程序。");
        ConsoleCore.ShowLine($" 3. 工作目录: {ConsoleCore.WorkingDirectory}");
        ConsoleCore.ShowLine();
        Console.ResetColor();
        while (!ConsoleCore.TokenSource.IsCancellationRequested)
        {
            ConsoleCore.InfoLine("请输入命令:");
            var command = ConsoleCore.ReadLine().Trim();
            if (command == "exit" || command == "quit")
            {
                ConsoleCore.InfoLine("正在退出...");
                await ConsoleCore.TokenSource.CancelAsync();
                return;
            }

            var index = command.IndexOf(' ');
            var name = index == -1 ? command : command[..index];
            command = index == -1 ? "" : command[(index + 1)..].Trim();
            // 处理帮助命令
            if (name == "help")
            {
                if (command.Length == 0)
                {
                    ConsoleCore.InfoLine("可用命令集:");
                    var nIndex = 1;
                    foreach (var cmdx in _commandHandlers.Values)
                    {
                        ConsoleCore.Write($" {nIndex++}. {cmdx.Name}");
                        ConsoleCore.WriteLine(ConsoleColor.DarkGray, $" {cmdx.Description}");
                    }
                }
                else if (_commandHandlers.TryGetValue(command, out var cmdx))
                {
                    cmdx!.ShowHelp();
                }
                else
                {
                    ConsoleCore.ErrorLine($"未找到命令: {command}");
                }
                continue;
            }
            await ExecuteAsync(name, command, ConsoleCore.TokenSource.Token);
            Thread.Sleep(100);
        }
    }

    private async Task ExecuteAsync(string commandName, string args, CancellationToken token)
    {
        if (_commandHandlers.TryGetValue(commandName, out var handler))
        {
            try
            {
                var commandArgs = new CommandArgs(args);
                if (commandArgs.IsValid)
                {
                    ConsoleCore.WriteLine($"正在执行: {commandName} ...");
                    await handler.ExecuteAsync(commandArgs, token);
                }
            }
            catch (Exception exception)
            {
                ConsoleCore.ErrorLine(exception.Message);
#if DEBUG
                ConsoleCore.ErrorLine(exception.StackTrace!);
#endif
            }
        }
        else
        {
            ConsoleCore.ErrorLine("未找到所执行的命令：{0}", commandName);
        }
    }
}