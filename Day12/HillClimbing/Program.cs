using FluentAssertions;

namespace HillClimbing;

public class Hill
{
    char[,] _heightmap;
    private readonly char _startChar = 'S';
    private readonly char _endChar = 'E';

    private int _nrows;
    private int _ncols;

    private (int x, int y) _destination;

    private readonly List<(int deltaX, int deltaY)> _directions = new()
    {
        (0, 1), (1, 0), (0, -1), (-1, 0)
    };

    public int SolveProblem1(string file)
    {
        Initialize(file);
        var start = FindPosition('S');
        return BFS(start);
    }

    public int SolveProblem2(string file)
    {
        Initialize(file);
        List<int> lengths = new();
        foreach (var start in FindStarts())
        {
            var length = BFS(start);
            lengths.Add(length);
        }

        return lengths.Where(length => length != -1).Min();
    }

    private void Initialize(string file)
    {
        string[] lines = File.ReadAllLines($"../../../{file}");

        // Create multidimensional char array
        int maxCols = lines.Max(x => x.Length);
        _heightmap = new char[lines.Length, maxCols];

        for (var i = 0; i < lines.Length; i++)
        {
            char[] chars = lines[i].ToCharArray();
            for (int j = 0; j < chars.Length; j++)
            {
                _heightmap[i, j] = chars[j];
            }
        }

        _nrows = _heightmap.GetLength(0);
        _ncols = _heightmap.GetLength(1);

        _destination = FindPosition('E');
        _heightmap[_destination.x, _destination.y] = 'z'; // We know the coordinates, so now set them to a and z so normal rules of heigh difference apply
    }

    private int BFS((int x, int y) start)
    {
        _heightmap[start.x, start.y] = 'a';

        // Implement BFS to find shortest path
        Queue<(int x, int y, int steps)> queue = new();
        bool[,] visited = new bool[_nrows, _ncols];

        // Enqueue the starting position.
        queue.Enqueue((start.x, start.y, 0));
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.x == _destination.x && current.y == _destination.y)
            {
                return current.steps;
            }

            // go to neighbors if not already visited
            foreach (var direction in _directions)
            {
                var newX = current.x + direction.deltaX;
                var newY = current.y + direction.deltaY;

                if (!Enumerable.Range(0, _nrows).Contains(newX) ||
                    !Enumerable.Range(0, _ncols).Contains(newY) ||
                    visited[newX, newY]) continue;

                // Check if the elevation change is within limits (same or +1)
                char currentElevation = _heightmap[current.x, current.y];
                char newElevation = _heightmap[newX, newY];
                int elevationDifference = newElevation - currentElevation;
                if (elevationDifference > 1) continue;

                queue.Enqueue((newX, newY, current.steps + 1));
                visited[newX, newY] = true;
            }
        }

        // If no path is found.
        return -1;
    }

    private (int x, int y) FindPosition(char letter)
    {
        for (int i = 0; i < _nrows; i++)
        {
            for (int j = 0; j < _ncols; j++)
            {
                if (_heightmap[i, j] == letter)
                {
                    return (i, j);
                }
            }
        }

        throw new Exception("Not found");
    }

    private List<(int x, int y)> FindStarts()
    {
        List<(int x, int y)> starts = new();
        for (int i = 0; i < _nrows; i++)
        {
            for (int j = 0; j < _ncols; j++)
            {
                if (_heightmap[i, j] is 'a' or 'S')
                {
                    starts.Add((i, j));
                }
            }
        }

        return starts;
    }

    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var hill = new Hill();
            hill.SolveProblem1("dummydata.txt").Should().Be(31);
            Console.WriteLine(hill.SolveProblem1("data.txt"));
            hill.SolveProblem2("dummydata.txt").Should().Be(29);
            Console.WriteLine(hill.SolveProblem2("data.txt"));
        }
    }
}