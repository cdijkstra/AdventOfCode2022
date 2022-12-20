using System.Text.RegularExpressions;
using FluentAssertions;

namespace Grove;

public class Grove
{
    private List<int> _positions = new();
    private List<int> _numbers = new();

    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        for (var originalIndex = 0; originalIndex != _numbers.Count; originalIndex++)
        {
            var move = _numbers[originalIndex];
            var newPosition = _positions[originalIndex];
            Console.WriteLine($"Moving {move} at {newPosition}");
            if (move > 0)
            {
                foreach (var pos in Enumerable.Range(newPosition + 1, move))
                {
                    Console.WriteLine($"Searching index {pos}, {_positions[_positions.IndexOf(pos)]}");
                    if (_positions[_positions.IndexOf(pos)] > 0)
                    {
                        _positions[_positions.IndexOf(pos)] -= 1;
                    }
                }
            }

            if (move < 0)
            {
                foreach (var pos in Enumerable.Range(newPosition - move, move))
                {
                    _positions[_positions.IndexOf(pos)] += 1 % _positions.Count;;
                }
                // Add complicated trick due to cyclicity
            }
            _positions[originalIndex] += move % _positions.Count;
            _positions.ForEach(x => Console.Write(x));
            Console.WriteLine();
        }

        return 1;
    }

    private void Initialize(string file)
    {
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
        var answer = grove.SolveProblem1("dummydata.txt", 1);
        Console.WriteLine($"Answer = {answer}");
        answer.Should().Be(1651);
        // beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
        // var answer1 = beacon.SolveProblem1("data.txt", 2000000);
        // var answer2 = beacon.SolveProblem2("data.txt", 4000000);
        // Console.WriteLine($"{answer1} and {answer2}");
    }
}