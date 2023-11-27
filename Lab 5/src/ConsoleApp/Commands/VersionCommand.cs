using JustCli;
using JustCli.Attributes;
using System.Reflection;

namespace ConsoleApp.Commands;

[Command("version", "Prints app version & author.")]
public class VersionCommand : ICommand
{
    [CommandOutput]
    public IOutput Output { get; set; } = default!;

    public int Execute()
    {
        string? version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();


        Output.WriteInfo($"{version} Cool Guy");
        return ReturnCode.Success;
    }
}
