using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class lab1
{
    static char[,] sea;
    static (int x, int y) playerPos;
    static List<(int x, int y)> enemyShips;
    static List<(int x, int y)> islands;
    static (int x, int y) playerDirection;
    static int n;
    static List<(int x, int y)> moves;

    public static void RunLab(string inputFile, string outputFile)
    {
        ReadInput(inputFile);
        moves = new List<(int x, int y)>();
        string outputPathFile = Path.Combine(outputFile, "output1.txt");
        DisplaySea();
        moves.Add((playerPos.x, playerPos.y));
        while (enemyShips.Count > 0)
        {
            // count moves 
            Console.WriteLine("moves count: " + moves.Count);
            // Try to shoot
            if (TryShoot())
            {
                moves.Add((playerPos.x, playerPos.y));
                continue;
            }
            Console.WriteLine("enemy ships count: " + enemyShips.Count);

            // Try to move
            if (!TryMove())
            {
                if (IsPlayerShipDestroyed())
                {
                    File.WriteAllText(outputPathFile, "IMPOSSIBLE");
                    return;
                }
            }
        }
        
        File.WriteAllLines(outputPathFile, new string[] { moves.Count.ToString() }
            .Concat(moves.Select(m => $"{m.x} {m.y}")));
    }

    static bool TryShoot()
    {
        Console.WriteLine("try shoot");
        var nearbyEnemyShips = enemyShips.Where(enemyShip =>
        {
            int dx = Math.Abs(enemyShip.x - playerPos.x);
            int dy = Math.Abs(enemyShip.y - playerPos.y);
            return dx <= 3 && dy <= 3;
        }).ToList();

        if (nearbyEnemyShips.Count == 0)
        {
            return false;
        }
        Console.WriteLine("nearby enemy ships count: " + nearbyEnemyShips.Count);
        var nearestEnemyShip = nearbyEnemyShips.OrderBy(enemyShip =>
        {
            int dx = Math.Abs(enemyShip.x - playerPos.x);
            int dy = Math.Abs(enemyShip.y - playerPos.y);
            return dx + dy;
        }).First();
        
        int x = nearestEnemyShip.x;
        int y = nearestEnemyShip.y;
        int dx = x - playerPos.x;
        int dy = y - playerPos.y;
        Shoot(x, y, dx, dy);
        Shoot(x, y, -dx, -dy);
    
        MoveEnemyShips();
        if (IsPlayerShipDestroyed()) return false;
        DisplaySea();
        return !AreAllEnemyShipsDestroyed();
    }

    static void DisplaySea()
    {
        for (int i = 0; i < 2 * n + 1; i++)
        {
            for (int j = 0; j < 2 * n + 1; j++)
            {
                Console.Write(sea[i, j]);
            }
            Console.WriteLine();
        }
    }

    static void Shoot(int x, int y, int dx, int dy)
    {
        Console.WriteLine("shoot");
        while (Math.Abs(x - playerPos.x) <= 3 && Math.Abs(y - playerPos.y) <= 3 && IsInsideBoard(x, y))
        {
            Console.WriteLine("position: " + x + " " + y);
            if (sea[x, y] == 'O')
            {
                return;
            }
            if (sea[x, y] == 't')
            {
                enemyShips.Remove((x, y));
                sea[x, y] = '.';
                return;
            }
            x += dx;
            y += dy;
        }
    }

    static bool TryMove()
    {
        
        Console.WriteLine("try move");
        
        if (AreAllEnemyShipsDestroyed())
        {
            return false; 
        }
        if (IsPlayerShipDestroyed())
        {
            return false;
        }
        // try to move to the nearest enemy ship
        var nearestEnemyShip = enemyShips.OrderBy(enemyShip =>
        {
            int dx = Math.Abs(enemyShip.x - playerPos.x);
            int dy = Math.Abs(enemyShip.y - playerPos.y);
            return dx + dy;
        }).First();
        
        int x = nearestEnemyShip.x;
        int y = nearestEnemyShip.y;
        int dx = x - playerPos.x;
        int dy = y - playerPos.y;
        
        if (Math.Abs(dx) > 1)
        {
            dx = dx / Math.Abs(dx);
        }
        if (Math.Abs(dy) > 1)
        {
            dy = dy / Math.Abs(dy);
        }
        
        if (IsInsideBoard(playerPos.x + dx, playerPos.y + dy) && IsCellFree(playerPos.x + dx, playerPos.y + dy))
        {
            MovePlayerShip(dx, dy);
        }
        
        MoveEnemyShips();
        
        if (IsPlayerShipDestroyed())
        {
            return false;
        }
        
        if (AreAllEnemyShipsDestroyed())
        {
            return false; 
        }
        
        DisplaySea();

        return true; 
    }
    
    static bool AreAllEnemyShipsDestroyed()
    {
        return enemyShips.Count == 0;
    }

    static bool IsCellFree(int x, int y)
    {
        return sea[x, y] == '.';
    }

    static bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 2 * n + 1 && y >= 0 && y < 2 * n + 1;
    }

    static void MoveEnemyShips()
    {
        List<(int x, int y)> newEnemyShips = new List<(int x, int y)>();

        foreach (var enemyShip in enemyShips)
        {
            int x = enemyShip.x;
            int y = enemyShip.y;

            int minDistance = int.MaxValue;
            (int, int) nextPosition = (x, y);

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int newX = x + dx;
                    int newY = y + dy;

                    int distance = Math.Abs(newX - playerPos.x) + Math.Abs(newY - playerPos.y);

                    if (IsInsideBoard(newX, newY) && distance < minDistance)
                    {
                        minDistance = distance;
                        nextPosition = (newX, newY);
                    }
                }
            }

            bool isIsland = islands.Contains(nextPosition);
            bool isAnotherEnemyShip = newEnemyShips.Contains(nextPosition);
            bool isPlayerShip = playerPos == nextPosition;
           
            if (isPlayerShip)
            {
                newEnemyShips.Add(nextPosition);
                enemyShips = newEnemyShips;
                Console.WriteLine("player ship destroyed");
                return;
            }

            if (!isIsland && !isAnotherEnemyShip)
            {
                newEnemyShips.Add(nextPosition);
            }
        }
        // set new enemy ships positions on the sea 
        foreach (var enemyShip in enemyShips)
        {
            sea[enemyShip.x, enemyShip.y] = '.';
        }
        foreach (var enemyShip in newEnemyShips)
        {
            sea[enemyShip.x, enemyShip.y] = 't';
        }
        enemyShips = newEnemyShips;
    }
    
    static void MovePlayerShip(int newX, int newY)
    {
        Console.WriteLine("move player ship to " + playerPos.x + newX + " " + playerPos.y + newY);
        sea[playerPos.x, playerPos.y] = '.';
        playerPos = (playerPos.x + newX, playerPos.y + newY);
        sea[playerPos.x, playerPos.y] = '+';
        moves.Add((playerPos.x, playerPos.y));
    }

    static bool IsPlayerShipDestroyed()
    {
        Console.WriteLine("is player ship destroyed");
        if (enemyShips.Contains(playerPos))
        {
            return true; 
        }
        return false; 
    }

    static void ReadInput(string inputPath)
    {
        string inputPathFile = Path.Combine(inputPath, "input1.txt");
        string[] lines = File.ReadAllLines(inputPathFile);
        n = int.Parse(lines[0]);
        sea = new char[2 * n + 1, 2 * n + 1];
        enemyShips = new List<(int x, int y)>();
        islands = new List<(int x, int y)>();

        for (int i = 0; i < 2 * n + 1; i++)
        {
            for (int j = 0; j < 2 * n + 1; j++)
            {
                sea[i, j] = lines[i + 1][j];
                switch (sea[i, j])
                {
                    case '+':
                        playerPos = (i, j);
                        playerDirection = (i - 1, j);
                        break;
                    case 't':
                        enemyShips.Add((i, j));
                        break;
                    case 'O':
                        islands.Add((i, j));
                        break;
                }
            }
        }
    }
}