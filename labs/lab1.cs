using System.Text.RegularExpressions;

namespace labs;

public class lab1
{
    const char EMPTY = '.';
    const char ENEMY = 't';
    const char ISLAND = 'O';
    const char PLAYER = '+';
    private static readonly int[] DX = { -1, -1, -1, 0, 1, 1, 1, 0 };
    private static readonly int[] DY = { -1, 0, 1, 1, 1, 0, -1, -1 };
    
    public static void RunLab(string inputFile, string outputFile)
    {
        string[] lines = File.ReadAllLines("INPUT.TXT");
        int n = int.Parse(lines[0]);
        char[,] field = new char[2 * n + 1, 2 * n + 1];
        (int x, int y) playerPos = (n, n);
        List<(int x, int y)> enemies = new List<(int x, int y)>();

        for (int i = 1; i <= 2 * n; i++)
        {
            for (int j = 0; j < 2 * n + 1; j++)
            {
                field[i - 1, j] = lines[i][j];
                if (field[i - 1, j] == ENEMY) enemies.Add((i - 1, j));
            }
        }

        List<(int x, int y)> moves = new List<(int x, int y)>();

        while (enemies.Count > 0)
        {
            var shotResult = TryShoot(field, playerPos, enemies);
            if (shotResult.Item1)
            {
                moves.Add((playerPos.x + 1, playerPos.y + 1));
                enemies = shotResult.Item2;
                continue;
            }

            var moveResult = TryMove(field, playerPos, enemies);
            if (moveResult.success)
            {
                moves.Add((moveResult.playerPos.x + 1, moveResult.playerPos.y + 1));
                playerPos = moveResult.playerPos;
                enemies = moveResult.enemies;
            }
            else
            {
                File.WriteAllText("OUTPUT.TXT", "IMPOSSIBLE");
                return;
            }

            // Перевірка на наявність живих ворогів
            if (enemies.Count == 0)
            {
                break;
            }

            // Перевірка на нерозв'язану ситуацію
            bool allEnemiesStuck = true;
            foreach (var enemy in enemies)
            {
                var enemyMoveResult = TryMove(field, enemy, new List<(int x, int y)>(enemies));
                if (enemyMoveResult.success)
                {
                    allEnemiesStuck = false;
                    break;
                }
            }

            if (allEnemiesStuck)
            {
                File.WriteAllText("OUTPUT.TXT", "IMPOSSIBLE");
                return;
            }
        }

        List<string> outputLines = new List<string>();
        outputLines.Add(moves.Count.ToString());
        foreach (var move in moves)
        {
            outputLines.Add($"{move.x} {move.y}");
        }

        File.WriteAllLines("OUTPUT.TXT", outputLines);
    }

    static ((int x, int y) playerPos, List<(int x, int y)> enemies, bool success) TryMove(char[,] field, (int x, int y) playerPos, List<(int x, int y)> enemies)
    {
        int n = (field.GetLength(0) - 1) / 2;
        var newPlayerPos = (playerPos.x, playerPos.y);
        var newEnemies = new List<(int x, int y)>(enemies);
        int minDist = int.MaxValue;

        for (int d = 0; d < 8; d++)
        {
            int nx = playerPos.x + DX[d];
            int ny = playerPos.y + DY[d];
            if (nx >= 0 && nx < 2 * n + 1 && ny >= 0 && ny < 2 * n + 1 && field[nx, ny] == EMPTY)
            {
                int dist = 0;
                foreach (var enemy in enemies)
                {
                    dist += Math.Abs(nx - enemy.x) + Math.Abs(ny - enemy.y);
                }
                if (dist < minDist)
                {
                    minDist = dist;
                    newPlayerPos = (nx, ny);
                }
            }
        }

        for (int i = 0; i < newEnemies.Count; i++)
        {
            var enemy = newEnemies[i];
            int maxDist = 0;
            var bestPos = enemy;

            for (int d = 0; d < 8; d++)
            {
                int nx = enemy.x + DX[d];
                int ny = enemy.y + DY[d];
                if (nx >= 0 && nx < 2 * n + 1 && ny >= 0 && ny < 2 * n + 1 && field[nx, ny] != ISLAND)
                {
                    int dist = Math.Abs(newPlayerPos.x - nx) + Math.Abs(newPlayerPos.y - ny);
                    if (dist > maxDist)
                    {
                        maxDist = dist;
                        bestPos = (nx, ny);
                    }
                }
            }

            newEnemies[i] = bestPos;
            if (bestPos == newPlayerPos)
            {
                return (newPlayerPos, newEnemies, false);
            }
        }

        return (newPlayerPos, newEnemies, true);
    }

    static (bool, List<(int x, int y)>) TryShoot(char[,] field, (int x, int y) playerPos, List<(int x, int y)> enemies)
    {
        int n = (field.GetLength(0) - 1) / 2;
        var newEnemies = new List<(int x, int y)>(enemies);

        for (int d = 0; d < 8; d += 2)
        {
            for (int dist = 1; dist <= 3; dist++)
            {
                int nx = playerPos.x + DX[d] * dist;
                int ny = playerPos.y + DY[d] * dist;
                if (nx >= 0 && nx < 2 * n + 1 && ny >= 0 && ny < 2 * n + 1)
                {
                    if (field[nx, ny] == ENEMY)
                    {
                        newEnemies.Remove((nx, ny));
                        field[nx, ny] = EMPTY;
                        break;
                    }
                    else if (field[nx, ny] == ISLAND)
                    {
                        break;
                    }
                }
            }
        }

        if (enemies.Count == newEnemies.Count)
        {
            return (false, newEnemies);
        }
        else
        {
            return (true, newEnemies);
        }
    }
}