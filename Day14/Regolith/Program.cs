using FluentAssertions;

namespace RopeBridge;


public class Regolith
{
    private List<List<char>> _grid = new ();
    private List<(int x, int y)> _sandCoordinates = new();

    private const int _xMax = 600;
    private const int _yMax = 600;
    
    public int SolveProblem1(string file)
    {
        Initialize(file);
        return SolvePuzzle(file);
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file);
        return SolvePuzzle(file);
    }
    
    private void Initialize(string file)
    {
        _sandCoordinates.Clear();
        _grid.Clear();
        foreach (var idx in Enumerable.Range(0, _xMax))
        {
            List<char> newRow = Enumerable.Repeat('.', _yMax).ToList();
            _grid.Add(newRow);
        }
        
        foreach (var line in File.ReadLines(file))
        {
            var entries = line.Split(" -> ");
            List<(int xs, int ys)> intEntries = entries.Select(entry => (int.Parse(entry.Split(",")[0]), int.Parse(entry.Split(",")[1]))).ToList();
            for (var lineIdx = 0; lineIdx != intEntries.Count - 1; lineIdx++)
            {
                if (intEntries[lineIdx].xs != intEntries[lineIdx + 1].xs)
                {
                    if (intEntries[lineIdx].xs < intEntries[lineIdx + 1].xs)
                    {
                        var length = intEntries[lineIdx + 1].xs - intEntries[lineIdx].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].xs, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].ys] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].ys] = '#';
                        }
                    }
                }
                else if (intEntries[lineIdx].ys != intEntries[lineIdx + 1].ys)
                {
                    if (intEntries[lineIdx].ys < intEntries[lineIdx + 1].ys)
                    {
                        var length = intEntries[lineIdx + 1].ys - intEntries[lineIdx].ys;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].ys, length))
                        {
                            _grid[intEntries[lineIdx].xs][newEntry] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[intEntries[lineIdx].xs][newEntry] = '#';
                        }
                    }
                }
                else
                {
                    throw new Exception("QUEEEEE");
                }
            }
            
        }
    }

    private int SolvePuzzle(string file)
    {
        var startPosition = (500, 0);

        (int x, int y) position = startPosition; 
        for (var sand = 0; sand != 5; sand++)
        {
            // Find position where it falls down
            while (_grid[position.x][position.y - 1] != '#' && _grid[position.x][position.y - 1] != 'o')
            {
                position.y--;
                Console.WriteLine("Falling");
                _sandCoordinates.Add(GetFinalCoordinates(position));
            }
        }

        return _sandCoordinates.Count;
    }

    private (int x, int y) GetFinalCoordinates((int x, int y) position)
    {
        while (_grid[position.x][position.y] != '.')
        {
            position.y--;
        }
        if (_grid[position.x][position.y - 1] != '#')
        {
            // Finished
            return position;
        }
        if (_grid[position.x][position.y - 1] != 'o')
        {
            var toLeft = GetFinalCoordinates((position.x - 1, position.y - 1));
            if (toLeft != (0,0))
            {
                return toLeft;
            }
            
            var toRight = GetFinalCoordinates((position.x + 1, position.y - 1));
            if (toLeft != (0,0))
            {
                return toLeft;
            }
        }

        return (0, 0);
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var regolith = new Regolith();
        regolith.SolveProblem1("dummydata.txt");
    }
}