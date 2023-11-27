using Common;
using JustCli;

args = new[] { "run", "-n", "3" };

LabExecutor.Labs.Add(new Lab1.Lab());
LabExecutor.Labs.Add(new Lab2.Lab());
LabExecutor.Labs.Add(new Lab3.Lab());

await CommandLineParser.Default.ParseAndExecuteCommandAsync(args);
