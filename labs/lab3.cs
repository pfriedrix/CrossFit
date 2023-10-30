namespace labs;

public class lab3
{
    static List<(int, bool)>[] graph;
    static int[] parent;
    static bool[] used;
    static int[] depth;
    static int[] up;
    static int timer;
    static List<(int, int)> bridges;
    static List<(int, int)> roads;

    public static void RunLab(string inputPath, string outputPath)
    {
        string inputPathFile = Path.Combine(inputPath, "input3.txt");
        string outputPathFile = Path.Combine(outputPath, "output3.txt");
        var input = File.ReadAllLines(inputPathFile);
        var nm = input[0].Split().Select(int.Parse).ToArray();
        int n = nm[0], m = nm[1];

        graph = new List<(int, bool)>[n];
        parent = new int[n];
        used = new bool[n];
        depth = new int[n];
        up = new int[n];
        bridges = new List<(int, int)>();
        roads = new List<(int, int)>();

        for (int i = 0; i < n; i++)
        {
            graph[i] = new List<(int, bool)>();
        }

        for (int i = 0; i < m; i++)
        {
            var road = input[i + 1].Split().Select(int.Parse).ToArray();
            int u = road[0] - 1, v = road[1] - 1;
            bool isPaved = road[2] == 1;
            graph[u].Add((v, isPaved));
            graph[v].Add((u, isPaved));
            if (isPaved)
            {
                bridges.Add((u, v));
            }
            else
            {
                roads.Add((u, v));
            }
        }

        FindBridges(n);

        int answer = 0;
        foreach (var bridge in bridges)
        {
            foreach (var road in roads)
            {
                var newGraph = new List<(int, bool)>[n];
                for (int i = 0; i < n; i++)
                {
                    newGraph[i] = new List<(int, bool)>(graph[i]);
                }

                newGraph[bridge.Item1].Remove((bridge.Item2, true));
                newGraph[bridge.Item2].Remove((bridge.Item1, true));
                newGraph[road.Item1].Remove((road.Item2, false));
                newGraph[road.Item2].Remove((road.Item1, false));

                used = new bool[n];
                if (!IsConnected(newGraph, n))
                {
                    answer++;
                }
            }
        }

        File.WriteAllText(outputPathFile, answer.ToString());
    }
    
    static void FindBridges(int n)
    {
        timer = 0;
        used = new bool[n];
        depth = new int[n];
        up = new int[n];
        parent = new int[n];
        for (int i = 0; i < n; i++)
        {
            used[i] = false;
            parent[i] = -1;
        }

        for (int i = 0; i < n; i++)
        {
            if (!used[i])
            {
                Dfs(i);
            }
        }
    }

    static void Dfs(int v)
    {
        used[v] = true;
        depth[v] = up[v] = timer++;
        foreach (var edge in graph[v])
        {
            int to = edge.Item1;
            bool isPaved = edge.Item2;
            if (to == parent[v]) continue;
            if (used[to]) up[v] = Math.Min(up[v], depth[to]);
            else
            {
                parent[to] = v;
                Dfs(to);
                up[v] = Math.Min(up[v], up[to]);
                if (up[to] > depth[v] && isPaved)
                {
                    bridges.Add((v, to));
                }
            }
        }
    }

    static bool IsConnected(List<(int, bool)>[] graph, int n)
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(0);
        used[0] = true;

        int visitedVertices = 1;
        while (queue.Count > 0)
        {
            int v = queue.Dequeue();
            foreach (var edge in graph[v])
            {
                int to = edge.Item1;
                if (!used[to])
                {
                    used[to] = true;
                    queue.Enqueue(to);
                    visitedVertices++;
                }
            }
        }

        return visitedVertices == n;
    }
}