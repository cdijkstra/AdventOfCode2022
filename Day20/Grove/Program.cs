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
            var amountOfSteps = _numbers[originalIndex];
            var currentPos = _positions[originalIndex];

            switch (amountOfSteps)
            {
                case > 0 when currentPos + amountOfSteps < _positions.Count - 1:
                {
                    for (var moveleftIndex = currentPos + 1; moveleftIndex <= currentPos + amountOfSteps; moveleftIndex++)
                    {
                        _positions[_positions.IndexOf(moveleftIndex)] -= 1;
                    }
                    
                    _positions[originalIndex] = currentPos + amountOfSteps;
                    break;
                }
                case > 0:
                {
                    // Modulo Length - 1 because one extra step is done at right side.
                    var endPosition = mod(currentPos + amountOfSteps, _positions.Count - 1);
                    if (endPosition < currentPos)
                    {
                        for (var moveRightIndex = currentPos - 1; moveRightIndex >= endPosition; moveRightIndex--)
                        {
                            _positions[_positions.IndexOf(moveRightIndex)] += 1;
                        }
                    }
                    else if (endPosition > currentPos)
                    {
                        for (var moveRightIndex = currentPos + 1; moveRightIndex <= endPosition; moveRightIndex++)
                        {
                            _positions[_positions.IndexOf(moveRightIndex)] -= 1;
                        }
                    }

                    _positions[originalIndex] = endPosition;
                    break;
                }
                case < 0 when currentPos + amountOfSteps > 0:
                {
                    // Note that amountOfSteps is a negative number here! 
                    for (var moveRightIndex = currentPos - 1; moveRightIndex >= currentPos + amountOfSteps; moveRightIndex--)
                    {
                        _positions[_positions.IndexOf(moveRightIndex)] += 1;
                    }

                    _positions[originalIndex] = currentPos + amountOfSteps;
                    break;
                }
                case < 0:
                {
                    var endPosition = currentPos + amountOfSteps == 0 ? 
                        _positions.Count - 1 : mod(currentPos + amountOfSteps, _positions.Count - 1);

                    if (endPosition > currentPos)
                    {
                        for (var moveLeftIndex = currentPos + 1; moveLeftIndex <= endPosition; moveLeftIndex++)
                        {
                            _positions[_positions.IndexOf(moveLeftIndex)] -= 1;
                        }
                    }
                    else if (endPosition < currentPos)
                    {
                        for (var newRightIndex = currentPos - 1; newRightIndex >= endPosition; newRightIndex--)
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

        // grove.SolveProblem1("dummydata.txt", 1).Should().Be(3);
        // grove.SolveProblem1("dummydata2.txt", 1).Should().Be(3);
        var answer = grove.SolveProblem1("data.txt", 1);
        Console.WriteLine($"Answer = {answer}");
    }
}