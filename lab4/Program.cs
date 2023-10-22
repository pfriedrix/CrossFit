using labs;
using System;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Enter command: ");
            args = Console.ReadLine().Split(' ');

            try
            {
                if (args.Length == 0)
                {
                    DisplayHelp();
                    continue;
                }

                switch (args[0])
                {
                    case "version":
                        Console.WriteLine("Author: Andrii Aleksandruk");
                        Console.WriteLine("Version: 1.0");
                        break;

                    case "run":
                        HandleRunCommand(args);
                        break;

                    case "set-path":
                        HandleSetPathCommand(args);
                        break;

                    default:
                        DisplayHelp();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }

    private static void HandleRunCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Please specify the lab number.");
            return;
        }

        string labName = args[1];
        if (labName != "lab1" && labName != "lab2" && labName != "lab3")
        {
            Console.WriteLine("Invalid lab number.");
            return;
        }

        string inputPath = GetArgumentValue(args, "-I", "--input");
        string outputPath = GetArgumentValue(args, "-o", "--output");
        SetDefaultPathsIfNecessary(ref inputPath, ref outputPath);

        if (string.IsNullOrEmpty(inputPath))
        {
            Console.WriteLine("Input file not found.");
            return;
        }

        Runner.RunLab(int.Parse(labName.Substring(3)), inputPath, outputPath);
    }

    private static void HandleSetPathCommand(string[] args)
    {
        string path = GetArgumentValue(args, "-p", "--path");
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Path not spacified.");
            return;
        }
        Environment.SetEnvironmentVariable("LAB_PATH", path, EnvironmentVariableTarget.User);
        Console.WriteLine($"Environment variable LAB_PATH set to {path}");
    }

    static string GetArgumentValue(string[] args, string shortArg, string longArg)
    {
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == shortArg || args[i] == longArg)
            {
                if (i + 1 < args.Length) return args[i + 1];
            }
        }
        return null;
    }

    static void SetDefaultPathsIfNecessary(ref string inputPath, ref string outputPath)
    {
        if (string.IsNullOrEmpty(inputPath))
        {
            inputPath = Environment.GetEnvironmentVariable("LAB_PATH") ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        if (string.IsNullOrEmpty(outputPath))
        {
            outputPath = Environment.GetEnvironmentVariable("LAB_PATH") ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
    }

    static void DisplayHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("version - Display program info");
        Console.WriteLine("run [lab1|lab2|lab3] - Run the specified lab");
        Console.WriteLine("set-path - Set the folder path");
    }
}