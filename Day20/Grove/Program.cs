using FluentAssertions;

namespace Grove;

public class Grove
{
    private readonly long _decryptionKey = 811589153;
    private List<long> _positions = new();
    private List<long> _numbers = new();
    
    public long SolveProblem(string file, bool secondExercise = false)
    {
        Initialize(file, secondExercise);
        MovePositions(secondExercise ? 10 : 1);
        
        var posZero = _positions[_numbers.IndexOf(0)];
        return _numbers[_positions.IndexOf(mod(posZero + 1000, _positions.Count))] +
               _numbers[_positions.IndexOf(mod(posZero + 2000, _positions.Count))] +
               _numbers[_positions.IndexOf(mod(posZero + 3000, _positions.Count))];
    }

    private long mod(long num, long modulo) {
        return (num%modulo + modulo)%modulo;
    }

    private void MovePositions(int totalRepetitions = 1)
    {
        for (int repetitions = 0; repetitions != totalRepetitions; repetitions++)
        {
            for (var originalIndex = 0; originalIndex != _positions.Count; originalIndex++)
            {
                var amountOfSteps = _numbers[originalIndex];
                var currentPos = _positions[originalIndex];

                switch (amountOfSteps)
                {
                    case > 0 when currentPos + amountOfSteps < _positions.Count - 1:
                    {
                        MoveRight(currentPos, amountOfSteps, originalIndex);
                        break;
                    }
                    case > 0:
                    {
                        MoveRightWithModulo(currentPos, amountOfSteps, originalIndex);
                        break;
                    }
                    case < 0 when currentPos + amountOfSteps > 0:
                    {
                        MoveLeft(currentPos, amountOfSteps, originalIndex);
                        break;
                    }
                    case < 0:
                    {
                        MoveLeftWithModulo(currentPos, amountOfSteps, originalIndex);
                        break;
                    }
                }
            }
        }
    }

    private void MoveLeftWithModulo(long currentPos, long amountOfSteps, int originalIndex)
    {
        var endPosition = currentPos + amountOfSteps == 0
            ? _positions.Count - 1
            : mod(currentPos + amountOfSteps, _positions.Count - 1);

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
    }

    private void MoveLeft(long currentPos, long amountOfSteps, int originalIndex)
    {
        // Note that amountOfSteps is a negative number here! 
        for (var moveRightIndex = currentPos - 1;
             moveRightIndex >= currentPos + amountOfSteps;
             moveRightIndex--)
        {
            _positions[_positions.IndexOf(moveRightIndex)] += 1;
        }

        _positions[originalIndex] = currentPos + amountOfSteps;
    }

    private void MoveRightWithModulo(long currentPos, long amountOfSteps, int originalIndex)
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
    }

    private void MoveRight(long currentPos, long amountOfSteps, int originalIndex)
    {
        for (var moveleftIndex = currentPos + 1;
             moveleftIndex <= currentPos + amountOfSteps;
             moveleftIndex++)
        {
            _positions[_positions.IndexOf(moveleftIndex)] -= 1;
        }

        _positions[originalIndex] = currentPos + amountOfSteps;
        return;
    }

    private void Initialize(string file, bool secondExercise)
    {
        _numbers.Clear();
        _positions.Clear();
        
        var idx = 0;
        foreach (var num in File.ReadLines(file))
        {
            if (!secondExercise)
            {
                _numbers.Add(int.Parse(num));
            }
            else
            {
                _numbers.Add(int.Parse(num) * _decryptionKey);
            }
            _positions.Add(idx++);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var grove = new Grove();

        grove.SolveProblem("dummydata.txt").Should().Be(3);
        grove.SolveProblem("dummydata.txt", true).Should().Be(1623178306);
        var firstAnswer = grove.SolveProblem("data.txt");
        var secondAnswer = grove.SolveProblem("data.txt", true);
        Console.WriteLine($"Answer = {firstAnswer} and {secondAnswer}");
    }
}