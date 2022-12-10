using System.Xml;
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
    private List<(int x, int y)> _knots = new(); // Exercise 2

    private const int _xMax = 5000;
    private const int _yMax = 5000;
    public int SolveProblem1(string file)
    {
        Initialize(file);
        var instructions = File.ReadLines(file).ToList();
        foreach (var instruction in instructions)
        {
            var direction = Enum.Parse(typeof(Direction), instruction.Split()[0]);
            var amount = int.Parse(instruction.Split()[1]);
            
            // If initial difference of +1
            // _positionHead += amount and _postitionTail += amount
            
            // If initial difference of 0 (overlapping)
            // _positionHead += amount and _postitionTail += amount - 1. Don't move tail if amount < 1

            // If initial difference of -1 (head in opposite direction of movement)
            // _positionHead += amount and _postitionTail += amount - 2. Don't move tail if amount < 2
            switch (direction)
            {
                case Direction.U:
                {
                    MoveUp(amount);
                    break;
                }
                case Direction.D:
                {
                    MoveDown(amount);
                    break;
                }
                case Direction.L:
                {
                    MoveLeft(amount);
                    break;
                }
                case Direction.R:
                {
                    MoveRight(amount);
                    break;
                }
            }
        }

        return _grid.SelectMany(x => x).Count(y => y.Equals('#'));
    }

    private void MoveRight(int amount)
    {
        int moveTailRightBy = CalculateStepsRight(amount);

        var newHeadX = _knots[0].x + amount;
        var newTailX = _knots[1].x + moveTailRightBy;
        var newTailY = _knots[1].y;
        if (moveTailRightBy > 0)
        {
            switch (_knots[0].y - _knots[1].y)
            {
                case 1:
                    newTailY++;
                    break;
                case -1:
                    newTailY--;
                    break;
            }

            for (var moveRight = 1; moveRight <= moveTailRightBy; moveRight++)
            {
                _grid[_knots[1].x + moveRight][newTailY] = '#';
            }
        }

        List<(int x, int y)> newKnots = new()
        {
            (newHeadX, _knots[0].y),
            (newTailX, newTailY),
        };

        _knots = newKnots;
    }

    private void MoveLeft(int amount)
    {
        int moveTailLeftBy = CalculateStepsLeft(amount);
        var newTailY = _knots[1].y;
        if (moveTailLeftBy > 0)
        {
            switch (_knots[0].y - _knots[1].y)
            {
                case 1:
                    newTailY++;
                    break;
                case -1:
                    newTailY--;
                    break;
            }

            for (var moveLeft = 1; moveLeft <= moveTailLeftBy; moveLeft++)
            {
                _grid[_knots[1].x - moveLeft][newTailY] = '#';
            }
        }
        
        var newTailX = _knots[1].x - moveTailLeftBy;
        var newHeadX = _knots[0].x - amount;

        List<(int x, int y)> newKnots = new()
        {
            (newHeadX, _knots[0].y),
            (newTailX, newTailY),
        };

        _knots = newKnots;
        
    }

    private void MoveDown(int amount)
    {
        int moveTailDownBy = CalculateStepsDown(amount);
        var newTailX = _knots[1].x;
        if (moveTailDownBy > 0)
        {
            switch (_knots[0].x - _knots[1].x)
            {
                case 1:
                    newTailX++;
                    break;
                case -1:
                    newTailX--;
                    break;
            }

            for (var moveDown = 1; moveDown <= moveTailDownBy; moveDown++)
            {
                _grid[newTailX][_knots[1].y - moveDown] = '#';
            }
        }

        var newTailY = _knots[1].y - moveTailDownBy;
        var newHeadY = _knots[0].y - amount;
        
        List<(int x, int y)> newKnots = new()
        {
            (_knots[0].x, newHeadY),
            (newTailX, newTailY),
        };
    }

    private void MoveUp(int amount)
    {
        int moveTailUpBy = CalculateStepsUp(amount);
        var newTailX = _knots[1].x;
        if (moveTailUpBy > 0)
        {
            switch (_knots[0].x - _knots[1].x)
            {
                case 1:
                    newTailX++;
                    break;
                case -1:
                    newTailX--;
                    break;
            }

            for (var moveUp = 1; moveUp <= moveTailUpBy; moveUp++)
            {
                _grid[newTailX][_knots[1].y + moveUp] = '#';
            }
        }

        var newTailY = _knots[1].y + moveTailUpBy;
        var newHeadY = _knots[0].y + amount;
        
        List<(int x, int y)> newKnots = new()
        {
            (_knots[0].x, newHeadY),
            (newTailX, newTailY),
        };
    }

    public int SolveProblem2(string file)
    {
        Initialize(file);

        return 1;
    }

    private int findElementOfFirstDuplicate()
    {
        for (var listIndex = 0; listIndex < _knots.Count - 1; listIndex++)
        {
            if (_knots[listIndex] == _knots[listIndex + 1])
                return listIndex;
        }

        return 10; // No duplicates
    }

    private void Initialize(string file)
    {
        _knots.Clear();
        var currentXPos = _xMax / 2 - 1;
        var currentYPos = _yMax / 2 - 1;
        var head = (currentXPos, currentYPos);
        var tail = (currentXPos, currentYPos);
        _knots.Add(head);
        _knots.Add(tail);
        _grid.Clear();

        for (var idx = 0; idx < _xMax; idx++)
        {
            List<char> row = Enumerable.Repeat('.', _yMax).ToList();
            _grid.Add(row);
        }
        
        _knots = Enumerable.Repeat( ( currentXPos, currentYPos ), 10).ToList();
        
        _grid[_knots[0].x][_knots[0].y] = '#';
    }
    
    private int CalculateStepsUp(int amount)
    {
        return (_knots[0].y - _knots[0].y) switch
        {
            1 => Math.Max(amount, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount - 2, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsRight(int amount)
    {
        return (_knots[0].x - _knots[1].x) switch
        {
            1 => Math.Max(amount, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount - 2, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsLeft(int amount)
    {
        return (_knots[0].x - _knots[1].x) switch
        {
            1 => Math.Max(amount - 2, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsDown(int amount)
    {
        return (_knots[0].y - _knots[1].y) switch
        {
            1 => Math.Max(amount - 2, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount, 0),
            _ => 0
        };
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var ropeBridge = new RopeBridge();
        ropeBridge.SolveProblem1("dummydata.txt").Should().Be(13);
        // treetop.SolveProblem2("dummydata.txt").Should().Be(8);
        //
        // var solution1 = ropeBridge.SolveProblem1("data.txt");
        // var solution2 = treetop.SolveProblem2("data.txt");
        //
        // Console.WriteLine($"Solutions are {solution1}");
    }
}