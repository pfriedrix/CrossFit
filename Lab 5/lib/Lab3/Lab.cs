using Common;
using Common.Models;

namespace Lab3;

public class Lab : LabBase
{
    private List<(int Destination, bool IsPaved)>[] _graph;
    private readonly List<(int, int)> _bridges = new();
    private readonly List<(int, int)> _roads = new();
    private int[] _parent, _depth, _up;
    private bool[] _visited;
    private int _timer;

    public override int Code => Constants.ThirdLabCode;

    public override async Task<LabResult> Execute(LabInput input)
    {
        ParseInput(input.Input);
        FindBridges();
        int answer = CountCriticalConnections();
        return new LabResult(answer.ToString());
    }

    private void ParseInput(string input)
    {
        var elements = input.Split(' ');
        int nodeCount = int.Parse(elements[0]), edgeCount = int.Parse(elements[1]);

        InitializeGraph(nodeCount);

        int index = 2; // Starting index for edge data
        for (int i = 0; i < edgeCount; i++)
        {
            int source = int.Parse(elements[index++]) - 1;
            int destination = int.Parse(elements[index++]) - 1;
            bool isPaved = elements[index++] == "1";

            AddEdge(source, destination, isPaved);
        }
    }

    private void InitializeGraph(int nodeCount)
    {
        _graph = new List<(int, bool)>[nodeCount];
        for (int i = 0; i < nodeCount; i++)
        {
            _graph[i] = new List<(int, bool)>();
        }
    }

    private void AddEdge(int source, int destination, bool isPaved)
    {
        _graph[source].Add((destination, isPaved));
        _graph[destination].Add((source, isPaved));

        if (isPaved)
            _bridges.Add((source, destination));
        else
            _roads.Add((source, destination));
    }

    private void FindBridges()
    {
        int n = _graph.Length;
        _visited = new bool[n];
        _depth = new int[n];
        _up = new int[n];
        _parent = new int[n];
        Array.Fill(_parent, -1);

        for (int i = 0; i < n; i++)
            if (!_visited[i])
                DepthFirstSearch(i);
    }

    private void DepthFirstSearch(int vertex)
    {
        _visited[vertex] = true;
        _depth[vertex] = _up[vertex] = _timer++;

        foreach (var (to, isPaved) in _graph[vertex])
        {
            if (to == _parent[vertex]) continue;
            if (_visited[to])
                _up[vertex] = Math.Min(_up[vertex], _depth[to]);
            else
            {
                _parent[to] = vertex;
                DepthFirstSearch(to);
                _up[vertex] = Math.Min(_up[vertex], _up[to]);
                if (_up[to] > _depth[vertex] && isPaved)
                    _bridges.Add((vertex, to));
            }
        }
    }

    private int CountCriticalConnections()
    {
        int answer = 0;
        foreach (var bridge in _bridges)
            foreach (var road in _roads)
                if (!IsGraphConnectedAfterRemoving(bridge, road))
                    answer++;

        return answer;
    }

    private bool IsGraphConnectedAfterRemoving((int, int) bridge, (int, int) road)
    {
        var modifiedGraph = CloneGraph();
        RemoveEdge(modifiedGraph, bridge);
        RemoveEdge(modifiedGraph, road);

        return IsConnected(modifiedGraph);
    }

    private List<(int, bool)>[] CloneGraph()
    {
        return _graph.Select(g => new List<(int, bool)>(g)).ToArray();
    }

    private static void RemoveEdge(List<(int, bool)>[] graph, (int source, int destination) edge)
    {
        graph[edge.source].RemoveAll(e => e.Item1 == edge.destination);
        graph[edge.destination].RemoveAll(e => e.Item1 == edge.source);
    }

    private bool IsConnected(List<(int, bool)>[] graph)
    {
        _visited = new bool[graph.Length];
        Queue<int> queue = new();
        queue.Enqueue(0);
        _visited[0] = true;

        int visitedVertices = 1;
        while (queue.Count > 0)
        {
            int v = queue.Dequeue();
            foreach (var (to, _) in graph[v])
            {
                if (!_visited[to])
                {
                    queue.Enqueue(to);
                    _visited[to] = true;
                    visitedVertices++;
                }
            }
        }

        return visitedVertices == graph.Length;
    }
}
