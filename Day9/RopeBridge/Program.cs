using FluentAssertions;

namespace RopeBridge;

public enum Direction
{
    U,
    D,
    L,
    R
}

public class RopeBridge
{
    private List<List<char>> _grid = new ();
    private (int x, int y)[] _knots;

    private const int _xMax = 5000;
    private const int _yMax = 5000;
    
    public int SolveProblem1(string file)
    {
        Initialize(file, 2);
        return SolvePuzzle(file);
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file, 10);
        return SolvePuzzle(file);
    }
    
    private void Initialize(string file, int totalKnots)
    {
        var currentXPos = _xMax / 2 - 1;
        var currentYPos = _yMax / 2 - 1;
        _knots = new(int x, int y)[totalKnots];
        _knots = Enumerable.Repeat((currentXPos, currentYPos), totalKnots).ToArray();
        
        _grid.Clear();

        for (var idx = 0; idx < _xMax; idx++)
        {
            List<char> row = Enumerable.Repeat('.', _yMax).ToList();
            _grid.Add(row);
        }

        _grid[currentXPos][currentYPos] = '#';
    }

    private int SolvePuzzle(string file)
    {
        var instructions = File.ReadLines(file).ToList();
        foreach (var instruction in instructions)
        {
            var direction = Enum.Parse(typeof(Direction), instruction.Split()[0]);
            var amount = int.Parse(instruction.Split()[1]);

            for (int i = 0; i < amount; i++)
            {
                switch (direction)
                {
                    case Direction.U:
                        _knots[0].y += 1;
                        break;
                    case Direction.D:
                        _knots[0].y -= 1;
                        break;
                    case Direction.L:
                        _knots[0].x -= 1;
                        break;
                    case Direction.R:
                        _knots[0].x += 1;
                        break;
                }

                for (int knotIndex = 1; knotIndex < _knots.Length; knotIndex++)
                {
                    int deltaX = _knots[knotIndex - 1].x - _knots[knotIndex].x;
                    int deltaY = _knots[knotIndex - 1].y - _knots[knotIndex].y;

                    if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
                    {
                        _knots[knotIndex].x += Math.Sign(deltaX);
                        _knots[knotIndex].y += Math.Sign(deltaY);
                    }
                }

                Console.WriteLine($"Drawing at {_knots.Last().x},{_knots.Last().y}");
                _grid[_knots.Last().x][_knots.Last().y] = '#';
            }
        }

        return _grid.SelectMany(x => x).Count(y => y.Equals('#'));
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var ropeBridge = new RopeBridge();
        ropeBridge.SolveProblem1("dummydata.txt").Should().Be(13);
        ropeBridge.SolveProblem2("dummydata2.txt").Should().Be(36);
        
        var solution1 = ropeBridge.SolveProblem1("data.txt");
        var solution2 = ropeBridge.SolveProblem2("data.txt");
        
        Console.WriteLine($"Solutions are {solution1} and {solution2}");
    }
}