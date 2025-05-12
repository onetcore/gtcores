using System.Reflection;
using Dnzer;

var currentDirectory = GetCurrentDirectory();
Console.WriteLine($"当前目录: {currentDirectory.FullName}");
var generators = LoadGenerators();
Console.WriteLine($"可用工具集:\n {string.Join(", ", generators.Keys)}");
Console.WriteLine(" 可使用 help 命令查看帮助。");
Console.WriteLine(" 可使用 exit 或 quit 退出程序。");
Console.WriteLine("请输入要执行的工具:");
var command = Console.ReadLine()?.Trim();
while (command != "exit" && command != "quit")
{
    while (command == null || command.Length == 0)
    {
        Console.WriteLine("请输入要执行的工具:");
        command = Console.ReadLine()?.Trim();
        if (command == "exit" || command == "quit")
            return;
        Thread.Sleep(100);
        continue;
    }

    var index = command.IndexOf(' ');
    var name = index == -1 ? command : command[..index];
    command = index == -1 ? "" : command[(index + 1)..];
    // 处理帮助命令
    if (name == "help")
    {
        if (command.Length == 0)
        {
            Console.WriteLine("可用工具集:");
            foreach (var generater in generators)
            {
                Console.WriteLine($" {generater.Key} {generater.Value}");
            }
        }
        else if (generators.TryGetValue(command, out var gc))
        {
            Console.WriteLine($"{command} {gc}");
        }
        else
        {
            Cores.WriteLine(ConsoleColor.DarkRed, $"未找到工具: {command}");
        }

        command = null;
        continue;
    }
    if (generators.TryGetValue(name, out var generator))
    {
        Console.WriteLine($"正在执行: {name} ...");
        var arguments = new Arguments(currentDirectory, command.Trim());
        if (arguments.IsValid)
            generator.Generate(arguments);
        command = null;
        continue;
    }
    else
    {
        Cores.WriteLine(ConsoleColor.DarkRed, $"未找到工具: {name}");
    }
}

// 执行方法列表。
IDictionary<string, IGenerator> LoadGenerators()
{
    var generators = new Dictionary<string, IGenerator>(StringComparer.OrdinalIgnoreCase);
    var assembly = Assembly.GetExecutingAssembly();
    var types = assembly.GetTypes();
    foreach (var type in types)
    {
        if (type.IsClass && !type.IsAbstract && typeof(IGenerator).IsAssignableFrom(type))
        {
            var instance = (IGenerator)Activator.CreateInstance(type)!;
            var name = type.Name.ToLower().Replace("generator", string.Empty).Replace("generater", string.Empty);
            if (generators.ContainsKey(name))
            {
                Console.WriteLine($"工具已经存在: {name}");
                continue;
            }
            generators.Add(name, instance);
        }
    }
    return generators;
}

/// 获取当前目录
DirectoryInfo GetCurrentDirectory()
{
    var currentDir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
    var solutionDir = currentDir;
    while (solutionDir != null && !solutionDir.GetFiles("*.sln", SearchOption.TopDirectoryOnly).Any())
    {
        solutionDir = solutionDir.Parent;
    }
    return solutionDir ?? currentDir;
}

