namespace labs;

public class lab2
{
    public static void RunLab(string inputPath, string outputPath)
    {
        string inputPathFile = Path.Combine(inputPath, "input.txt");
        string outputPathFile = Path.Combine(outputPath, "output.txt");
        string[] inputLines = File.ReadAllLines(inputPathFile);
        string[] parameters = inputLines[0].Split();
        int N = int.Parse(parameters[0]);
        int K = int.Parse(parameters[1]);
        string chain = inputLines[1];

        int[] dp = new int[N];
        dp[0] = 0; // Вартість переходу до першого символу завжди 0

        for (int i = 1; i < N; i++)
        {
            dp[i] = int.MaxValue;
            for (int j = Math.Max(0, i - K); j < i; j++)
            {
                int cost = chain[i] == chain[j] ? 0 : 1;
                dp[i] = Math.Min(dp[i], dp[j] + cost);
            }
        }

        File.WriteAllText(outputPathFile, dp[N - 1].ToString());
    }
}