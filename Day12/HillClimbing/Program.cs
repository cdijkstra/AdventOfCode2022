using FluentAssertions;

namespace HillClimbing;

public class Hill
{
    char[,] _heightmap;
    private readonly char _startChar = 'S';
    private readonly char _endChar = 'E';
    private Node _startNode = null;
    private Node _endNode = null;
    private int _nrows = 0;
    private int _ncols = 0;

    private Dictionary<Node, Dictionary<Node, int>> _graph = new();

    public int SolveProblem1(string file)
    {
        Initialize(file);
        return DijkstaAlgorithm();
    }

    private void Initialize(string file)
    {
        _startNode = null;
        _endNode = null;
        _nrows = 0;
        _ncols = 0;
        _graph = new Dictionary<Node, Dictionary<Node, int>>();
        
        string[] lines = File.ReadAllLines(file);

        // Create multidimensional char array
        int maxCols = lines.Max(x => x.Length);
        _heightmap = new char[lines.Length, maxCols];

        for (int i = 0; i < lines.Length; i++)
        {
            char[] chars = lines[i].ToCharArray();
            for (int j = 0; j < chars.Length; j++)
            {
                _heightmap[i, j] = chars[j];
            }
        }

        _nrows = _heightmap.GetLength(0);
        _ncols = _heightmap.GetLength(1);

        Console.WriteLine($"Dimensions {_nrows} and {_ncols}");

        CreateGraph();
    }

    private void CreateGraph()
    {
        // Find the start and end nodes on the heightmap. Distance is how many steps we've traversed to get here
        // This is set to int.MaxValue if it still has to be computed.
        for (int i = 0; i < _nrows; i++)
        {
            for (int j = 0; j < _ncols; j++)
            {
                // Should we insert 'a' and 'z' instead of 'S' and 'E' here?
                if (_heightmap[i, j] == _startChar)
                {
                    _startNode = new Node(i, j, 'a');
                    Console.WriteLine("Found start");
                }
                else if (_heightmap[i, j] == _endChar)
                {
                    _endNode = new Node(i, j, 'z');
                    Console.WriteLine("Found end");
                }
            }
        }

        if (_startNode == null || _endNode == null)
        {
            throw new Exception("Start or end node not found");
        }

        // Replace S by a and E by z in the heightmap
        for (int i = 0; i < _heightmap.GetLength(0); i++)
        {
            for (int j = 0; j < _heightmap.GetLength(1); j++)
            {
                if (_heightmap[i, j] == 'S')
                {
                    _heightmap[i, j] = 'a';
                }

                if (_heightmap[i, j] == 'E')
                {
                    _heightmap[i, j] = 'z';
                }
            }
        }

        // Create the graph as a dictionary of nodes and their neighbors
        for (int i = 0; i < _nrows; i++)
        {
            for (int j = 0; j < _ncols; j++)
            {
                Node currentNode = new Node(i, j, _heightmap[i, j]);
                _graph[currentNode] = new Dictionary<Node, int>();

                if (i > 0 && Math.Abs(_heightmap[i, j] - _heightmap[i - 1, j]) <= 1)
                {
                    _graph[currentNode][new Node(i - 1, j, _heightmap[i - 1, j])] = 1;
                }

                if (j > 0 && Math.Abs(_heightmap[i, j] - _heightmap[i, j - 1]) <= 1)
                {
                    _graph[currentNode][new Node(i, j - 1, _heightmap[i, j - 1])] = 1;
                }

                if (i < _nrows - 1 && Math.Abs(_heightmap[i, j] - _heightmap[i + 1, j]) <= 1)
                {
                    _graph[currentNode][new Node(i + 1, j, _heightmap[i + 1, j])] = 1;
                }

                if (j < _ncols - 1 && Math.Abs(_heightmap[i, j] - _heightmap[i, j + 1]) <= 1)
                {
                    _graph[currentNode][new Node(i, j + 1, _heightmap[i, j + 1])] = 1;
                }
            }
        }
    }

    private int DijkstaAlgorithm()
    {
        // Perform Dijkstra's algorithm
        HashSet<Node> visited = new HashSet<Node>();
        PriorityQueue<Node> priorityQueue = new PriorityQueue<Node>((a, b) => a.Cost - b.Cost);
        _startNode.Cost = 0;
        priorityQueue.Enqueue(_startNode);

        while (priorityQueue.Count > 0)
        {
            Node currentNode = priorityQueue.Dequeue();

            Console.WriteLine(
                $"Visiting {currentNode.Height} with Cost {currentNode.Cost} at {currentNode.Row},{currentNode.Col}");

            if (currentNode.Equals(_endNode))
            {
                Console.WriteLine("Shortest distance from S to E: " + currentNode.Cost);
                return currentNode.Cost;
            }

            if (visited.Contains(currentNode))
            {
                continue;
            }

            visited.Add(currentNode);

            foreach (Node neighborNode in _graph[currentNode].Keys)
            {
                int cost = _graph[currentNode][neighborNode];

                if (!visited.Contains(neighborNode) && currentNode.Cost + cost < neighborNode.Cost)
                {
                    neighborNode.Cost = currentNode.Cost + cost;
                    priorityQueue.Enqueue(neighborNode);
                }
            }
        }

        Console.WriteLine("Could not solve...");
        return 0;
    }

    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var hill = new Hill();
            hill.SolveProblem1("dummydata.txt").Should().Be(31);
            hill.SolveProblem1("data.txt").Should().Be(31);
            // var solution1 = hill.SolveProblem1("data.txt");
            // solution1.Should().Be(12);
        }
    }
}