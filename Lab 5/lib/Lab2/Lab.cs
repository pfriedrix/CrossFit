using Common;
using Common.Models;

namespace Lab2;

public class Lab : LabBase
{
    public override int Code => Constants.SecondLabCode;

    public override async Task<LabResult> Execute(LabInput input)
    {
        string[] inputLines = input.Input.Replace("  ", " ").Split(" ");
        Console.WriteLine(inputLines.Length);
        int N = int.Parse(inputLines[0]);
        int K = int.Parse(inputLines[1]);
        string chain = inputLines[2];

        int[] dp = new int[N];
        dp[0] = 0; 

        for (int i = 1; i < N; i++)
        {
            dp[i] = int.MaxValue;
            for (int j = Math.Max(0, i - K); j < i; j++)
            {
                int cost = chain[i] == chain[j] ? 0 : 1;
                dp[i] = Math.Min(dp[i], dp[j] + cost);
            }
        }
        return new LabResult(dp[N - 1].ToString());
    }
}