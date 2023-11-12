using System.Text.RegularExpressions;
using Priority_Queue;

namespace Valve;

public class Cave
{
    private List<Valve> _valves = new();
    private List<int> _pressureReleases = new();

    public int SolveProblem1(string file, int row)
    {
        Initialize(file);

        var startValve = _valves.Single(x => x.Name == "AA");
        Queue<(string valveName, int totalFlow)> queue = new();
        queue.Enqueue((startValve.Name, 0));

        while (queue.Count > 0)
        {
            
        }

        return 1;
    }

    private void Initialize(string file)
    {
        foreach (var command in File.ReadLines(file))
        {
            var flow = int.Parse(new Regex(@"\d+").Match(command).Value);
            var currentCave = new Regex(@"Valve [A-Za-z]{2} has").Match(command).Value.Substring(6, 2);
            var valvesRegex = new Regex(@"valve(s)? [A-Za-z]{2}((, [A-Za-z]{2})?)+$").Match(command).Value;
            var valves = valvesRegex.Substring(valvesRegex.IndexOf(" ") + 1);
            _valves.Add(new Valve
            {
                Name = currentCave,
                Open = false,
                Flow = flow,
                ConnectedValvesString = valves
            });
        }
    }
}

internal static class Program
{
    static async Task Main(string[] args)
    {
        var cave = new Cave();
        var answer = cave.SolveProblem1("../../../dummydata.txt", 10);
        // Console.WriteLine($"Answer = {answer}");
        // answer.Should().Be(1651);
        // beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
        // var answer1 = beacon.SolveProblem1("data.txt", 2000000);
        // var answer2 = beacon.SolveProblem2("data.txt", 4000000);
        // Console.WriteLine($"{answer1} and {answer2}");
    }
}