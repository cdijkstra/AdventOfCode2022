using System.Text.RegularExpressions;
using FluentAssertions;

namespace Monkey;

public class Puzzle
{
    private List<Monkey> _monkeys = new();

    public long SolveProblem(string file, int rounds, bool secondExercise)
    {
        Initialize(file, secondExercise);
        return InspectItems(rounds);
    }

    private void Initialize(string file, bool secondExercise)
    {
        _monkeys.Clear();

        var information = File.ReadLines(file).ToList();
        var monkey = new Monkey();
        foreach (var info in information)
        {
            if (info.StartsWith("Monkey"))
            {
                monkey.Number = int.Parse(info.Split()[1].Trim(':'));
            }
            else if (info.TrimStart().StartsWith("Starting"))
            {
                var numbersInText = Regex.Match(info, "[0-9]+[, [0-9]+]?").Value;
                var numbers = numbersInText
                    .Split(',')
                    .Where(x => long.TryParse(x, out _))
                    .Select(long.Parse)
                    .ToList();
                
                monkey.Items = numbers;
            }
            else if (info.TrimStart().StartsWith("Operation"))
            {
                var num = info.TrimStart().Split()[5];
                if (num == "old")
                {
                    monkey.Operation = secondExercise ? (x) => (x * x) : (x) => (x * x) / 3;
                }
                else
                {
                    var number = long.Parse(num);
                    var operation = char.Parse(info.TrimStart().Split()[4]);
                    monkey.Operation = operation switch
                    {
                        '*' => secondExercise ? (x) => (x * number) : (x) => (x * number) / 3,
                        '+' => secondExercise ? (x) => (x + number) : (x) => (x + number) / 3,
                        _ => monkey.Operation
                    };
                }
            }
            else if (info.TrimStart().StartsWith("Test"))
            {
                monkey.DivisibleBy = long.Parse(info.TrimStart().Split()[3]);
            }
            else if (info.TrimStart().StartsWith("If true"))
            {
                var throwTo = int.Parse(info.TrimStart().Split()[5]);
                monkey.ThrowToMonkey.Add(throwTo);
            }
            else if (info.TrimStart().StartsWith("If false"))
            {
                var throwTo = int.Parse(info.TrimStart().Split()[5]);
                monkey.ThrowToMonkey.Add(throwTo);
            }
            else
            {
                _monkeys.Add(monkey);
                monkey = new Monkey();
            }
        }
        
        _monkeys.Add(monkey);
    }

    private long InspectItems(int rounds)
    {
        var totalDivisbleBy = _monkeys.Select(x => x.DivisibleBy).Aggregate((x, y) => x * y);
        // The modulo trick works for both part 1 and part 2
        foreach (var currentRound in Enumerable.Range(0, rounds))
        {
            foreach (var monkey in _monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    var worryItem = monkey.Operation(monkey.Items.First()) % totalDivisbleBy;
                    monkey.Inspections++;

                    if (worryItem % monkey.DivisibleBy == 0)
                    {
                        var newMonkey = monkey.ThrowToMonkey.First();
                        _monkeys.Single(x => x.Number == newMonkey).Items.Add(worryItem);
                        monkey.Items.RemoveAt(0);
                    }
                    else
                    {
                        var newMonkey = monkey.ThrowToMonkey.Last();
                        _monkeys.Single(x => x.Number == newMonkey).Items.Add(worryItem);
                        monkey.Items.RemoveAt(0);
                    }
                }
            }
        }

        return _monkeys.Select(mon => mon.Inspections)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((x, y) => x * y);
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var puzzle = new Puzzle();
        puzzle.SolveProblem("dummydata.txt", 20, false).Should().Be(10605);
        puzzle.SolveProblem("dummydata.txt", 10000, true).Should().Be(2713310158);
        
        var solution1 = puzzle.SolveProblem("data.txt", 20, false);
        var solution2 = puzzle.SolveProblem("data.txt", 10000, true);

        Console.WriteLine($"Solutions are {solution1} and {solution2}");
    }
}