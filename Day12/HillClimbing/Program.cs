using FluentAssertions;

namespace HillClimbing;

public class Hill
{
    char[,] _heightmap;
    private List<int> _routeLengths = new();
    private readonly char _start = 'S';
    private readonly char _end = 'E';
    private Node _startNode = null;
    private Node _endNode = null;
    private int _nrows = 0;
    private int ncols = 0;

    private Dictionary<Node, Dictionary<Node, int>> _graph = new();
    
    public int SolveProblem1(string file)
    {
        Initialize(file);
        return DijkstaAlgorithm();
    }

    private void Initialize(string file)
    {
        // ToDo: Add this later from file
        /*var instructions = File.ReadLines(file).ToList();
        foreach (var instruction in instructions)
        {
            _heightmap.Add(instruction.ToCharArray());
        }*/
        char[,] heightmap = {
            {'S', 'a', 'b', 'q', 'p', 'o', 'n', 'm'},
            {'a', 'b', 'c', 'r', 'y', 'x', 'x', 'l'},
            {'a', 'c', 'c', 's', 'z', 'E', 'x', 'k'},
            {'a', 'c', 'c', 't', 'u', 'v', 'w', 'j'},
            {'a', 'b', 'd', 'e', 'f', 'g', 'h', 'i'}
        };
        _heightmap = heightmap;
        
        _nrows = heightmap.GetLength(0);
        ncols = heightmap.GetLength(1);

        CreateGraph();
    }

    private void CreateGraph()
    {
        // Find the start and end nodes on the heightmap. Distance is how many steps we've traversed to get here
        // This is set to int.MaxValue if it still has to be computed.
        for (int i = 0; i < _nrows; i++) {
            for (int j = 0; j < ncols; j++) {
                // if (_heightmap[i, j] == _start) {
                //     _startNode = new Node(_start, i, j, 0);
                // } else if (_heightmap[i, j] == _end) {
                //     _endNode = new Node(_end, i, j, int.MaxValue);
                // }
                
                // Should we insert 'a' and 'z' instead of 'S' and 'E' here?
                if (_heightmap[i, j] == _start) {
                    _startNode = new Node('a', i, j, 0);
                } else if (_heightmap[i, j] == _end) {
                    _endNode = new Node('z', i, j, int.MaxValue);
                }
            }
        }
        
        // Create the graph as a dictionary of nodes and their neighbors
        for (int i = 0; i < _nrows; i++) {
            for (int j = 0; j < ncols; j++) {
                Node currentNode = new Node(_heightmap[i, j], i, j, int.MaxValue);
                _graph[currentNode] = new Dictionary<Node, int>();
                
                // We first check of we can find the i/j+-1 entry or whether it's off the grid
                // Then we check if the character is the same or one higher (alphabetically).
                // If yes, the weight is 1 instead of 0
                if (i > 0 && Math.Abs(_heightmap[i, j] - _heightmap[i-1, j]) <= 1) {
                    _graph[currentNode][new Node(_heightmap[i-1, j], i-1, j, int.MaxValue)] = 1;
                }
                if (j > 0 && Math.Abs(_heightmap[i, j] - _heightmap[i, j-1]) <= 1) {
                    _graph[currentNode][new Node(_heightmap[i, j-1], i, j-1, int.MaxValue)] = 1;
                }
                if (i < _nrows-1 && Math.Abs(_heightmap[i, j] - _heightmap[i+1, j]) <= 1) {
                    _graph[currentNode][new Node(_heightmap[i+1, j], i+1, j, int.MaxValue)] = 1;
                }
                if (j < ncols-1 && Math.Abs(_heightmap[i, j] - _heightmap[i, j+1]) <= 1) {
                    _graph[currentNode][new Node(_heightmap[i, j+1], i, j+1, int.MaxValue)] = 1;
                }
            }
        }
    }
    
    private int DijkstaAlgorithm()
    {
        // Perform Dijkstra's algorithm
        HashSet<Node> visited = new HashSet<Node>();
        PriorityQueue<Node> pq = new PriorityQueue<Node>((a, b) => a.distance - b.distance);
        _startNode.distance = 0;
        pq.Enqueue(_startNode);
        
        while (pq.Count > 0) {
            Node currentNode = pq.Dequeue();
            
            if (visited.Contains(currentNode)) {
                continue;
            }
            
            visited.Add(currentNode);
            
            foreach (Node neighborNode in _graph[currentNode].Keys) {
                int dist = _graph[currentNode][neighborNode];
                
                if (!visited.Contains(neighborNode) && currentNode.distance + dist < neighborNode.distance) {
                    neighborNode.distance = currentNode.distance + dist;
                    pq.Enqueue(neighborNode);
                }
            }
        }
        
        // Print the shortest distance from S to E
        Console.WriteLine("Shortest distance from S to E: " + _endNode.distance);
        return _endNode.distance;
    }
}

internal static class Program
{
    static async Task Main(string[] args)
    {
        var hill = new Hill();
        hill.SolveProblem1("dummydata.txt").Should().Be(31);
        // var solution1 = hill.SolveProblem1("data.txt");
        // solution1.Should().Be(12);
    }
}