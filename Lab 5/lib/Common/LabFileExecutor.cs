using Common.Models;

namespace Common;

public static class LabFileExecutor
{
    public static async Task Execute(int labNumber, string? inputFileName, string? outputFileName)
    {
        var inputFile = TryGetFile(inputFileName, defaultFileName: Constants.InputFileName);
        var outputFile = TryGetFile(outputFileName, defaultFileName:Constants.OutputFileName);

        var input = await File.ReadAllTextAsync(inputFile.FullName);

        var result = await LabExecutor.Execute(labNumber, new LabInput(input));

        await File.WriteAllTextAsync(outputFile.FullName, result.Result);
    }

    private static FileInfo TryGetFile(string? fileName, string defaultFileName)
    {
        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) 
        {
            return new FileInfo(fileName);
        }
        else
        {
            var fromEnv = TryReadFromEnvironment(defaultFileName);

            if (fromEnv == null)
            {
                var fromHome = TryReadFromHome(defaultFileName);

                if(fromHome == null)
                {
                    throw new FileNotFoundException("File was not found", fileName);
                }
                else
                {
                    return fromHome;
                }
            }
            else
            {
                return fromEnv;
            }
        }
    }

    private static FileInfo? TryReadFromEnvironment(string defaultFileName)
    {
        var path = Environment.GetEnvironmentVariable(Constants.LabEnvironmentVariableName);

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            return new FileInfo(Path.Combine(path, defaultFileName));
        }
        else 
        {
            return null!;
        }
    }

    private static FileInfo? TryReadFromHome(string defaultFileName)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), defaultFileName);

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
        {
            return new FileInfo(path);
        }
        else
        {
            return null!;
        }
    }
}
