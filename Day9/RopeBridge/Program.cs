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
                        // MoveUp(amount);
                        break;
                    case Direction.D:
                        _knots[0].y -= 1;
                        // MoveDown(amount);
                        break;
                    case Direction.L:
                        _knots[0].x -= 1;
                        // MoveLeft(amount);
                        break;
                    case Direction.R:
                        _knots[0].x += 1;
                        // MoveRight(amount);
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

    private void MoveRight(int amount)
    {
        var headX = _knots[0].x;
        var headY = _knots[0].y;
        
        for (var steps = 1; steps <= amount; steps++)
        {
            headX++;
            var newPositions = MoveIfPossible(headX, headY);
            // UpdatePositions(newPositions);
        }
    }
    
    private void MoveLeft(int amount)
    {
        var headX = _knots[0].x;
        var headY = _knots[0].y;
        
        for (var steps = 1; steps <= amount; steps++)
        {
            headX--;
            var newPositions = MoveIfPossible(headX, headY);
            // UpdatePositions(newPositions);
        }
    }

    private void MoveDown(int amount)
    {
        var headX = _knots[0].x;
        var headY = _knots[0].y;
        
        for (var steps = 1; steps <= amount; steps++)
        {
            headY--;
            var newPositions = MoveIfPossible(headX, headY);
            // UpdatePositions(newPositions);
        }
    }

    private void MoveUp(int amount)
    {
        var headX = _knots[0].x;
        var headY = _knots[0].y;
        
        for (var steps = 1; steps <= amount; steps++)
        {
            headY++;
            var newPositions = MoveIfPossible(headX, headY);
            // UpdatePositions(newPositions);
        }
    }

    private List<(int x, int y)> MoveIfPossible(int headX, int headY)
    {
        List<(int x, int y)> newPositions = new() { (headX, headY) };
        for (var knotIndex = 1; knotIndex < _knots.Length; knotIndex++)
        {
            if (MoveEntry(newPositions[knotIndex - 1], _knots[knotIndex]))
            {
                newPositions = Move(newPositions, knotIndex);
            }
            else
            {
                newPositions.Add((_knots[knotIndex].x, _knots[knotIndex].y));
            }
        }

        return newPositions;
    }

    private List<(int x, int y)> Move(List<(int x, int y)> newPositions, int knotIndex)
    {
        var newX = 0;
        var newY = 0;
        if (Math.Abs(newPositions[knotIndex - 1].x - _knots[knotIndex].x) > 1)
        {
            // Console.WriteLine($"X- {newPositions[knotIndex - 1].x} and {_knots[knotIndex].x}");
            // Console.WriteLine($"Y- {newPositions[knotIndex - 1].y} and {_knots[knotIndex].y}");
            if (newPositions[knotIndex - 1].x < _knots[knotIndex].x)
            {
                // Move left
                newX = _knots[knotIndex].x - 1;
                newY = _knots[knotIndex].y;
                switch (newPositions[knotIndex - 1].y - _knots[knotIndex].y)
                {
                    case 1:
                        newY++;
                        break;
                    case -1:
                        newY--;
                        break;
                }
                // Console.WriteLine($"LEFT - {newX},{newY}");
            }
            else
            {
                // Move right
                newX = _knots[knotIndex].x + 1;
                newY = _knots[knotIndex].y;
                switch (newPositions[knotIndex - 1].y - _knots[knotIndex].y)
                {
                    case 1:
                        newY++;
                        break;
                    case -1:
                        newY--;
                        break;
                }
                
                // Console.WriteLine($"RIGHT - {newX},{newY}");
            }
        }
        else if (Math.Abs(newPositions[knotIndex - 1].y - _knots[knotIndex].y) > 1)
        {
            if (newPositions[knotIndex - 1].y - _knots[knotIndex].y > 1)
            {
                // Move up 
                Console.WriteLine($"UP - {_knots[knotIndex].x},{_knots[knotIndex].y} versus {newPositions[knotIndex - 1].x},{newPositions[knotIndex - 1].y}");
                newX = _knots[knotIndex].x;
                switch (newPositions[knotIndex - 1].x - _knots[knotIndex].x)
                {
                    case 1:
                        newX++;
                        break;
                    case -1:
                        newX--;
                        break;
                }
                newY = _knots[knotIndex].y + 1;
                Console.WriteLine($"UP - {newX},{newY}");
            }
            else
            {
                // Move down
                newX = _knots[knotIndex].x;
                switch (newPositions[knotIndex - 1].x - _knots[knotIndex].x)
                {
                    case 1:
                        newX++;
                        break;
                    case -1:
                        newX--;
                        break;
                }
                newY = _knots[knotIndex].y - 1;
                // Console.WriteLine($"DOWN - {newX},{newY}");
            }
        }
        else
        {
            throw new Exception("Unexpected input received");
        }

        // Always do this
        (int x, int y) newPos = (newX, newY);
        newPositions.Add(newPos);

        PaintTail(knotIndex, newX, newY);
        return newPositions;
    }

    // private void UpdatePositions(List<(int x, int y)> newPositions)
    // {
    //     _knots = newPositions;
    // }

    private bool MoveEntry((int x, int y) head, (int x, int y) tail)
    {
        return Math.Sqrt(Math.Pow(head.x - tail.x, 2) + Math.Pow(head.y - tail.y, 2)) > Math.Sqrt(2);
    }

    private void PaintTail(int knotIndex, int newX, int newY)
    {
        if (knotIndex == _knots.Length - 1)
        {
            // This is the tail, so paint
            _grid[newX][newY] = '#';
        }
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