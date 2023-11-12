using FluentAssertions;

namespace HillClimbing;

public class Hill
{
    char[,] _heightmap;
    private readonly char _startChar = 'S';
    private readonly char _endChar = 'E';

    private int _nrows;
    private int _ncols;
    
    public int SolveProblem1(string file)
    {
        Initialize(file);
        var answer = BFS();
        Console.Write(answer);
        return answer;
    }

    private void Initialize(string file)
    {
        string[] lines = File.ReadAllLines($"../../../{file}");

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
    }

    private int BFS()
    {
        List<(int deltaX, int deltaY)>
            directions = new() { (0, 1), (1, 0), (0, -1), (-1, 0) }; // Possible moves: right, down, left, up.
        var start = FindPosition('S');
        var destination = FindPosition('E');
        _heightmap[start.x, start.y] = 'a';
        _heightmap[destination.x, destination.y] = 'z'; // We know the coordinates, so now set them to a and z so normal rules of heigh difference apply
        
        // Implement BFS to find shortest path
        Queue<(int x, int y, int steps)> queue = new Queue<(int x, int y, int steps)>();
        bool[,] visited = new bool[_nrows, _ncols];

        // Enqueue the starting position.
        queue.Enqueue((start.x, start.y, 0));
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.x == destination.x && current.y == destination.y)
            {
                return current.steps;
            }

            // go to neighbors if not already visited
            foreach (var direction in directions)
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
                
                Console.WriteLine($"{newX} and {newY}");
                
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
}

internal static class Program
{
    static async Task Main(string[] args)
    {
        var hill = new Hill();
        // hill.SolveProblem1("dummydata.txt").Should().Be(31);
        hill.SolveProblem1("data.txt");
        // var solution1 = hill.SolveProblem1("data.txt");
        // solution1.Should().Be(12);
    }
}