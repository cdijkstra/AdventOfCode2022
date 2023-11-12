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

        var valves = _valves;
        var startValve = _valves.Single(x => x.Name == "AA");
        startValve.Pressure = 0;
        startValve.timeLeft = 30;

        // Implement Dijkstra's algorithm
        SimplePriorityQueue<Valve> pq = new SimplePriorityQueue<Valve>();
        pq.Enqueue(startValve, -startValve.Pressure);

        while (pq.Count > 0)
        {
            Valve currentValve = pq.Dequeue();
            if (!currentValve.Open && currentValve.Flow > 0)
            {
                var newValve = currentValve;
                newValve.timeLeft--;
                newValve.Pressure += newValve.Flow;
                pq.Enqueue(newValve, -newValve.Pressure);
            }
            
            foreach (var tunnel in currentValve.ConnectedValves)
            {
                tunnel.timeLeft = currentValve.timeLeft - 1;
                int newPressure = Math.Min(tunnel.Pressure, tunnel.timeLeft * currentValve.Flow * (30 - tunnel.timeLeft));
            }
        }
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

        foreach (var valve in _valves)
        {
            var neighborValves = valve.ConnectedValvesString.Split(", ");
            List<Valve> neighbors = new();
            
            foreach (var neighborValve in neighborValves)
            {
                neighbors.Add(_valves.Single(x => x.Name == neighborValve));
            }
            valve.ConnectedValves = neighbors;
        }
    }
}

internal static class Program
{
    static async Task Main(string[] args)
    {
        var cave = new Cave();
        var answer = await cave.SolveProblem1("dummydata.txt", 10);
        Console.WriteLine($"Answer = {answer}");
        answer.Should().Be(1651);
        // beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
        // var answer1 = beacon.SolveProblem1("data.txt", 2000000);
        // var answer2 = beacon.SolveProblem2("data.txt", 4000000);
        // Console.WriteLine($"{answer1} and {answer2}");
    }
}