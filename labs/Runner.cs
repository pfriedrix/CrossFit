namespace labs;

public class Runner
{
    public static void RunLab(int labNumber, string inputPath, string outputPath)
    {
        switch (labNumber)
        {
            case 1:
                lab1.RunLab(inputPath, outputPath);
                break;
            case 2:
                lab2.RunLab(inputPath, outputPath);
                break;
            case 3:
                lab3.RunLab(inputPath, outputPath);
                break;
            default:
                throw new ArgumentException("Invalid lab number.");
        }
    }
}