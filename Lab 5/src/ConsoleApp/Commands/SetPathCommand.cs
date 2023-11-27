using JustCli;
using JustCli.Attributes;

namespace ConsoleApp.Commands;

[Command("set-path", "Sets path to I/O folder.")]
public class SetPathCommand : ICommand
{
    [CommandArgument("p", "path", Description = "Path to I/O folder.")]
    public string Path { get; set; }

    [CommandOutput]
    public IOutput Output { get; set; } = default!;

    public int Execute()
    {
        if (Directory.Exists(Path))
        {
            Environment.SetEnvironmentVariable("LAB_PATH", Path);

            Output.WriteInfo($"'LAB_PATH' now at '{Path}'.");

            return ReturnCode.Success;
        }
        else
        {
            Output.WriteError($"Invalid Path.");

            return ReturnCode.Failure;
        }
    }
}
