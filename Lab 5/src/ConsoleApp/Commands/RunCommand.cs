using Common;
using JustCli;
using JustCli.Attributes;

namespace ConsoleApp.Commands;

[Command("run", "Runs provided lab.")]
public class RunCommand : ICommandAsync
{
    [CommandArgument("n", "lab-number", Description = "Number of certain lab.")]
    public int LabNumber { get; set; }

    [CommandArgument("i", "input", Description = "Sets input file paht.", DefaultValue = "")]
    public string InputFilePath { get; set; } = default!;

    [CommandArgument("o", "output", Description = "Sets output file paht.", DefaultValue = "")]
    public string OutputFilePath { get; set; } = default!;

    [CommandOutput]
    public IOutput Output { get; set; } = default!;

    public async Task<int> ExecuteAsync()
    {
        await LabFileExecutor.Execute(LabNumber, InputFilePath, OutputFilePath);

        return ReturnCode.Success;
    }
}
