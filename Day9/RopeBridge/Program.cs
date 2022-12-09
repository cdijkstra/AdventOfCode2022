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
    private (int x, int y) _positionHead = (50, 50);
    private (int x, int y) _postitionTail = (50, 50);
    
    private const int _xMax = 100;
    private const int _yMax = 100;
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

        foreach (var entry in _grid)
        {
            entry.ForEach(x => Console.Write(x));
            Console.WriteLine();
        }
        
        return _grid.SelectMany(x => x).Count(y => y.Equals('#'));
    }

    private void MoveRight(int amount)
    {
        int moveTailRightBy = CalculateStepsRight(amount);
        
        if (moveTailRightBy > 0)
        {
            switch (_positionHead.y - _postitionTail.y)
            {
                case 1:
                    _postitionTail.y++;
                    break;
                case -1:
                    _postitionTail.y--;
                    break;
            }

            foreach (var newVisitedPlace in Enumerable.Range(_postitionTail.x, moveTailRightBy))
            {
                _grid[newVisitedPlace][_postitionTail.y] = '#';
            }
        }

        _postitionTail.x += moveTailRightBy;
        _positionHead.x += amount;
    }

    private void MoveLeft(int amount)
    {
        int moveTailLeftBy = CalculateStepsLeft(amount);
        if (moveTailLeftBy > 0)
        {
            switch (_positionHead.y - _postitionTail.y)
            {
                case 1:
                    _postitionTail.y++;
                    break;
                case -1:
                    _postitionTail.y--;
                    break;
            }

            foreach (var newVisitedPlace in Enumerable.Range(_postitionTail.x - moveTailLeftBy,
                         moveTailLeftBy))
            {
                _grid[newVisitedPlace][_postitionTail.y] = '#';
            }
        }

        _postitionTail.x -= moveTailLeftBy;
        _positionHead.x -= amount;
    }

    private void MoveDown(int amount)
    {
        int moveTailDownBy = CalculateStepsDown(amount);
        if (moveTailDownBy > 0)
        {
            switch (_positionHead.x - _postitionTail.x)
            {
                case 1:
                    _postitionTail.x++;
                    break;
                case -1:
                    _postitionTail.x--;
                    break;
            }

            foreach (var newVisitedPlace in Enumerable.Range(_postitionTail.y - moveTailDownBy,
                         moveTailDownBy))
            {
                _grid[_postitionTail.x][newVisitedPlace] = '#';
            }
        }

        _postitionTail.y -= moveTailDownBy;
        _positionHead.y -= amount;
    }

    private void MoveUp(int amount)
    {
        int moveTailUpBy = CalculateStepsUp(amount);
        if (moveTailUpBy > 0)
        {
            switch (_positionHead.y - _postitionTail.y)
            {
                case 1:
                    _postitionTail.y++;
                    break;
                case -1:
                    _postitionTail.y--;
                    break;
            }

            foreach (var newVisitedPlace in Enumerable.Range(_postitionTail.y, moveTailUpBy))
            {
                _grid[_postitionTail.x][newVisitedPlace] = '#';
            }
        }

        _postitionTail.y += moveTailUpBy;
        _positionHead.y += amount;
    }

    public int SolveProblem2(string file)
    {
        Initialize(file);
        
        return 0;
    }

    private void Initialize(string file)
    {
        for (var idx = 0; idx < _xMax; idx++)
        {
            List<char> row = Enumerable.Repeat('.', _yMax).ToList();
            _grid.Add(row);
        }
        _grid[_positionHead.x][_positionHead.y] = '#';
    }
    
    private int CalculateStepsUp(int amount)
    {
        return (_positionHead.y - _postitionTail.y) switch
        {
            1 => Math.Max(amount, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount - 2, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsRight(int amount)
    {
        return (_positionHead.x - _postitionTail.x) switch
        {
            1 => Math.Max(amount, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount - 2, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsLeft(int amount)
    {
        return (_positionHead.x - _postitionTail.x) switch
        {
            1 => Math.Max(amount - 2, 0),
            0 => Math.Max(amount - 1, 0),
            -1 => Math.Max(amount, 0),
            _ => 0
        };
    }
    
    private int CalculateStepsDown(int amount)
    {
        return (_positionHead.y - _postitionTail.y) switch
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
        // var solution1 = treetop.SolveProblem1("data.txt");
        // var solution2 = treetop.SolveProblem2("data.txt");
        //
        // Console.WriteLine($"Solutions are {solution1} and {solution2}");
    }
}