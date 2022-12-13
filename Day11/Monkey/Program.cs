using System.Numerics;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace Monkey;

public class Puzzle
{
    private List<Monkey> _monkeys = new();

    public int SolveProblem(string file, int rounds, bool secondExercise)
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
                    .Where(x => UInt64.TryParse(x, out _))
                    .Select(UInt64.Parse)
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
                    var number = UInt64.Parse(num);
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
                monkey.DivisibleBy = UInt64.Parse(info.TrimStart().Split()[3]);
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

    private int InspectItems(int rounds)
    {
        var totalDivisbleBy = _monkeys.Select(x => x.DivisibleBy).Aggregate((x, y) => x * y);
        Console.WriteLine($"TotalDivisible = {totalDivisbleBy}");
        foreach (var currentRound in Enumerable.Range(0, rounds))
        {
            foreach (var monkey in _monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    checked
                    {
                        if (monkey.Number == 2)
                        {
                            Console.WriteLine($"Item before = {monkey.Items.First()}");
                        }
                        var worryItem = monkey.Operation(monkey.Items.First());
                        if (monkey.Number == 2)
                        {
                            Console.WriteLine($"Item after = {worryItem}");
                        }
                        
                        
                        monkey.Inspections++;
                        if (worryItem % totalDivisbleBy == 0)
                        {
                            Console.WriteLine("OOOOK");
                            worryItem /= totalDivisbleBy;
                        }

                        if (worryItem % monkey.DivisibleBy == 0)
                        {
                            var newMonkey = monkey.ThrowToMonkey.First();
                            _monkeys.Single(x => x.Number == newMonkey).Items.Add(worryItem);
                            monkey.Items.RemoveAt(0);
                            if (monkey.Number == 2)
                            {
                                Console.WriteLine($"{monkey.Number} throws {worryItem} to {newMonkey} in round {currentRound}");
                            }
                        }
                        else
                        {
                            var newMonkey = monkey.ThrowToMonkey.Last();
                            _monkeys.Single(x => x.Number == newMonkey).Items.Add(worryItem);
                            monkey.Items.RemoveAt(0);
                            if (monkey.Number == 2)
                            {
                                Console.WriteLine($"{monkey.Number} throws {worryItem} to {newMonkey} in round {currentRound}");
                            }                        }
                    }
                }
            }
        }

        var bla = _monkeys.Select(mon => mon.Inspections)
            .OrderByDescending(x => x)
            .Take(4);

        foreach (var en in bla)
        {
            Console.Write(en + ",");
            Console.WriteLine();
        }

        foreach (var monkey in _monkeys)
        {
            foreach (var item in monkey.Items)
            {
                Console.Write($"Item = {item}");
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
        // puzzle.SolveProblem("dummydata.txt", 20, false).Should().Be(10605);
        
        // var answer = puzzle.SolveProblem("data.txt", 20, false);
        // Console.WriteLine(answer);
        puzzle.SolveProblem("dummydata.txt", 20, true);

        // var solution1 = puzzle.SolveProblem1("data.txt");
        // var solution2 = puzzle.SolveProblem2("data.txt");

        // Console.WriteLine($"Solutions are {solution1}");
    }
}