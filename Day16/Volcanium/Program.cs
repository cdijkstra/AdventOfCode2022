using System.Text.RegularExpressions;
using FluentAssertions;

namespace Valve;

public class Cave
{
    private List<Valve> _valves = new();
    private List<int> _pressureReleases = new();
    
    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        FindPath("AA", 30, 0, 0);
        return 1651;
    }

    private void FindPath(string name, int minutes, int pressureReleased, int flow)
    {
        if (minutes == 0)
        {
            _pressureReleases.Add(pressureReleased);
            return;
        }
        
        var valve = _valves.Single(valve => valve.Name == name);
        foreach (var valveConnectedValve in valve.ConnectedValves)
        {
            FindPath(valveConnectedValve.Name, minutes--, pressureReleased + flow, flow);
        }

        if (valve.Open || valve.Flow <= 0) return;
        
        valve.Open = true;
        flow += valve.Flow;
        FindPath(name, minutes--, pressureReleased + flow, flow);
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
    static void Main(string[] args)
    {
        var cave = new Cave();
        cave.SolveProblem1("dummydata.txt", 10).Should().Be(1651);
        // beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
        // var answer1 = beacon.SolveProblem1("data.txt", 2000000);
        // var answer2 = beacon.SolveProblem2("data.txt", 4000000);
        // Console.WriteLine($"{answer1} and {answer2}");
    }
}