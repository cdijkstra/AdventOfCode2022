using FluentAssertions;

namespace Hill;

public class Hill
{
    private List<List<char>> _grid = new();
    private List<int> _routeLengths = new();
    
    public async Task<int> SolveProblem1(string file)
    {
        Initialize(file);
        var startIndex = findStartIndex();

        await SolvePuzzle(startIndex, 0, new List<(int x, int y)>());
        return _routeLengths.Min() - 1;
    }

    private void Initialize(string file)
    {
        var instructions = File.ReadLines(file).ToList();
        foreach (var instruction in instructions)
        {
            _grid.Add(instruction.ToCharArray().ToList());
        }
    }

    private async Task SolvePuzzle((int x, int y) coordinates, int length, List<(int x, int y)> placesVisited)
    {
        placesVisited.Add(coordinates);
        length++;

        var neighbors = FindNeighborsToVisit(coordinates).Where(neighbor => !placesVisited.Contains(neighbor));
        // foreach (var valueTuple in neighbors)
        // {
        //     Console.WriteLine($"Visiting {valueTuple.x},{valueTuple.y} from {coordinates.x},{coordinates.y} and length {length}");
        // }

        foreach (var neighborToVisit in neighbors)
        {
            await SolvePuzzle(neighborToVisit, length, placesVisited);
        }

        if (_grid[coordinates.x][coordinates.y] == '{')
        {
            _routeLengths.Add(length);
        }
        
        placesVisited.Remove(coordinates);
    }

    private List<(int x, int y)> FindNeighborsToVisit((int x, int y) coordinates)
    {
        List<(int x, int y)> neighborsToVisit = new();
        if (coordinates.x > 0 &&
            (_grid[coordinates.x - 1][coordinates.y] <= _grid[coordinates.x][coordinates.y] + 1))
        {
            neighborsToVisit.Add((coordinates.x - 1, coordinates.y));
        }

        if (coordinates.x < _grid.Count - 1 &&
            (_grid[coordinates.x + 1][coordinates.y] <= _grid[coordinates.x][coordinates.y] + 1))
        {
            neighborsToVisit.Add((coordinates.x + 1, coordinates.y));
        }

        if (coordinates.y > 0 &&
            (_grid[coordinates.x][coordinates.y - 1] <= _grid[coordinates.x][coordinates.y] + 1))
        {
            neighborsToVisit.Add((coordinates.x, coordinates.y - 1));
        }

        if (coordinates.y < _grid.First().Count - 1 &&
            (_grid[coordinates.x][coordinates.y + 1] <= _grid[coordinates.x][coordinates.y] + 1))
        {
            neighborsToVisit.Add((coordinates.x, coordinates.y + 1));
        }

        return neighborsToVisit;
    }

    private (int x, int y) findStartIndex()
    {
        ReplaceLastIndex();
        for (var idx = 0; idx != _grid.Count; idx++)
        {
            for (var idy = 0; idy != _grid.First().Count; idy++)
            {
                if (_grid[idx][idy] == 'S')
                {
                    _grid[idx][idy] = 'a'; // This is the elevation of 'S'
                    return (idx, idy);
                }
            }
        }

        return (0, 0);
    }
    
    private void ReplaceLastIndex()
    {
        for (var idx = 0; idx != _grid.Count; idx++)
        {
            for (var idy = 0; idy != _grid.First().Count; idy++)
            {
                if (_grid[idx][idy] == 'E')
                {
                    _grid[idx][idy] = '{'; // = 'z' + 1
                }
            }
        }
    }
}

internal static class Program
{
    static async Task Main(string[] args)
    {
        var hill = new Hill();
        hill.SolveProblem1("dummydata.txt").Result.Should().Be(31);
        var solution1 = await hill.SolveProblem1("data.txt");
        Console.WriteLine(solution1);
        
        // var solution2 = ropeBridge.SolveProblem2("data.txt");
        //
        // Console.WriteLine($"Solutions are {solution1} and {solution2}");
    }
}