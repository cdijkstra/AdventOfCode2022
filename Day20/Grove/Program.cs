using FluentAssertions;

namespace Grove;

public class Grove
{
    public List<int> _positions = new();
    // Dummy data translates to this;
    // 0123456
    // 1023456
    // 0213456
    // 0142356
    // 0135246
    // 0124635
    // 0125643
    
    public List<int> _numbers = new();
    
    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        MovePositions();
        
        var posZero = _positions[_numbers.IndexOf(0)];

        Console.WriteLine($"Found {_numbers[_positions.IndexOf(mod(posZero + 1000, _positions.Count))]}");
        Console.WriteLine($"Found {_numbers[_positions.IndexOf(mod(posZero + 2000, _positions.Count))]}");
        Console.WriteLine($"Found {_numbers[_positions.IndexOf(mod(posZero + 3000, _positions.Count))]}");

        return _numbers[_positions.IndexOf(mod(posZero + 1000, _positions.Count))] +
               _numbers[_positions.IndexOf(mod(posZero + 2000, _positions.Count))] +
               _numbers[_positions.IndexOf(mod(posZero + 3000, _positions.Count))];
    }
    
    private int mod(int num, int modulo) {
        return (num%modulo + modulo)%modulo;
    }

    private void MovePositions()
    {
        for (var originalIndex = 0; originalIndex != _positions.Count; originalIndex++)
        {
            var moves = _numbers[originalIndex];
            var position = _positions[originalIndex];
            Console.WriteLine($"Taking steps {moves} from {position} at index {originalIndex}");

            switch (moves)
            {
                case > 0 when position + moves < _positions.Count - 1:
                {
                    // "moves" amount of steps moves left at right of original position
                    foreach (var pos in Enumerable.Range(position + 1, moves))
                    {
                        _positions[_positions.IndexOf(pos)] -= 1;
                    }
                    
                    _positions[originalIndex] = position + moves;
                    break;
                }
                case > 0:
                {
                    // Modulo Length - 1 because one extra step is done at right side.
                    var endPosition = mod(position + moves, _positions.Count - 1);
                    if (endPosition < position)
                    {
                        for (var moveRightIndex = endPosition; moveRightIndex < position; moveRightIndex++)
                        {
                            _positions[_positions.IndexOf(moveRightIndex)] += 1;
                        }
                    }
                    else if (endPosition > position)
                    {
                        for (var moveRightIndex = position + 1; moveRightIndex <= endPosition; moveRightIndex++)
                        {
                            _positions[_positions.IndexOf(moveRightIndex)] -= 1;
                        }
                    }

                    _positions[originalIndex] = endPosition;
                    break;
                }
                case < 0 when position + moves > 0:
                {
                    foreach (var pos in Enumerable.Range(position + moves, Math.Abs(moves)))
                    {
                        _positions[_positions.IndexOf(pos)] += 1;
                    }

                    _positions[originalIndex] = position + moves;
                    break;
                }
                case < 0:
                {
                    var endPosition = position + moves == 0 ? 
                        _positions.Count - 1 : 
                        mod(position + moves, _positions.Count - 1);

                    if (endPosition > position)
                    {
                        for (var moveLeftIndex = position + 1; moveLeftIndex <= endPosition; moveLeftIndex++)
                        {
                            _positions[_positions.IndexOf(moveLeftIndex)] -= 1;
                        }
                    }
                    else if (endPosition < position)
                    {
                        for (var newRightIndex = endPosition; newRightIndex < position; newRightIndex++)
                        {
                            _positions[_positions.IndexOf(newRightIndex)] += 1;
                        }
                    }

                    _positions[originalIndex] = endPosition;
                    break;
                }
            }
        }
    }

    private void Initialize(string file)
    {
        _numbers.Clear();
        _positions.Clear();
        
        var idx = 0;
        foreach (var num in File.ReadLines(file))
        {
            _numbers.Add(int.Parse(num));
            _positions.Add(idx++);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var grove = new Grove();

        grove.SolveProblem1("dummydata.txt", 1).Should().Be(3);
        var answer = grove.SolveProblem1("data.txt", 1);
        Console.WriteLine($"Answer = {answer}");
    }
}