using FluentAssertions;

namespace RopeBridge;

public class Regolith
{
    private List<List<char>> _grid = new ();
    private List<(int x, int y)> _sandCoordinates = new();
    private bool _fallingForever = false;

    private (int x, int y) _startPosition = (500, 0);
    private const int _xMax = 750;
    private const int _yMax = 200;

    private int _yFloor = 0;
    private readonly char[] _sandOrRock = { '#', 'o' };

    public int SolveProblem1(string file)
    {
        Initialize(file);
        var answer = SolvePuzzle(file);
        ShowOutput();
        return answer;
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file);
        for (var idx = 0; idx != _xMax; idx++)
        {
            _grid[_yFloor][idx] = '#';
        }
        
        var answer = SolveSecondPuzzle(file);
        ShowOutput();
        return answer;
    }
    
    private void Initialize(string file)
    {
        _sandCoordinates.Clear();
        _grid.Clear();
        _fallingForever = false;
        _yFloor = 0;
        foreach (var idx in Enumerable.Range(0, _yMax))
        {
            List<char> newRow = Enumerable.Repeat('.', _xMax).ToList();
            _grid.Add(newRow);
        }

        List<(int xs, int ys)> allIntEntries = new();
        foreach (var line in File.ReadLines(file))
        {
            var entries = line.Split(" -> ");
            List<(int xs, int ys)> intEntries = entries.Select(entry => (int.Parse(entry.Split(",")[0]), int.Parse(entry.Split(",")[1]))).ToList();
            allIntEntries.AddRange(intEntries);

            for (var lineIdx = 0; lineIdx != intEntries.Count - 1; lineIdx++)
            {
                if (intEntries[lineIdx].xs != intEntries[lineIdx + 1].xs)
                {
                    if (intEntries[lineIdx].xs < intEntries[lineIdx + 1].xs)
                    {
                        var length = intEntries[lineIdx + 1].xs - intEntries[lineIdx].xs + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].xs, length))
                        {
                            _grid[intEntries[lineIdx].ys][newEntry] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[intEntries[lineIdx].ys][newEntry] = '#';
                        }
                    }
                }
                else if (intEntries[lineIdx].ys != intEntries[lineIdx + 1].ys)
                {
                    if (intEntries[lineIdx].ys < intEntries[lineIdx + 1].ys)
                    {
                        var length = intEntries[lineIdx + 1].ys - intEntries[lineIdx].ys + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].ys, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].xs] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].ys - intEntries[lineIdx + 1].ys + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].ys, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].xs] = '#';
                        }
                    }
                }
                else
                {
                    throw new Exception("Unexpected scenario");
                }
            }
        }

        _yFloor = allIntEntries.Select(entry => entry.ys).Max() + 2;
    }

    private void ShowOutput()
    {
        for (var idy = 0; idy != _yMax; idy++)
        {
            for (var idx = 350; idx != _xMax - 50; idx++)
            {
                Console.Write(_grid[idy][idx]);
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Floor at {_yFloor}");
    }

    private int SolvePuzzle(string file)
    {
        var startPosition = (500, 0);

        while (!_fallingForever)
        {
            // Find position where it falls down
            var sandCoordinates = GetFinalCoordinates(startPosition);
            if (sandCoordinates.y != _yMax)
            {
                _sandCoordinates.Add(sandCoordinates);
                _grid[sandCoordinates.y][sandCoordinates.x] = 'o';
            }
            else
            {
                return _sandCoordinates.Count;
            }
        }

        return _sandCoordinates.Count;
    }
    
    private int SolveSecondPuzzle(string file)
    {
        while (_grid[_startPosition.y][_startPosition.x] != 'o')
        {
            // Find position where it falls down
            var sandCoordinates = GetFinalCoordinates(_startPosition);
            _sandCoordinates.Add(sandCoordinates);
            _grid[sandCoordinates.y][sandCoordinates.x] = 'o';
        }

        return _sandCoordinates.Count;
    }

    private (int x, int y) GetFinalCoordinates((int x, int y) position)
    {
        if (!_sandOrRock.Contains(_grid[position.y][position.x]))
        {
            while (_grid[position.y + 1][position.x] == '.')
            {
                position.y++;

                if (position.y + 1 == _yMax)
                {
                    _fallingForever = true;
                    return (position.x, _yMax);
                }
            }
        }

        if (_sandOrRock.Contains(_grid[position.y][position.x]))
        {
            return (0, 0);
        }
        if (_sandOrRock.Contains(_grid[position.y + 1][position.x]))
        {
            var toLeftDiagonal = GetFinalCoordinates((position.x - 1, position.y + 1));
            if (toLeftDiagonal != (0,0))
            {
                return toLeftDiagonal;
            }
            
            var toRightDiagonal = GetFinalCoordinates((position.x + 1, position.y + 1));
            return toRightDiagonal != (0,0) ? toRightDiagonal : position;
        }

        return (0, 0);
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var regolith = new Regolith();
        regolith.SolveProblem1("dummydata.txt").Should().Be(24);
        var answer1 = regolith.SolveProblem1("data.txt");
        regolith.SolveProblem2("dummydata.txt").Should().Be(93);
        var answer2 = regolith.SolveProblem2("data.txt");
        Console.WriteLine($"Answers = {answer1} and {answer2}");
    }
}