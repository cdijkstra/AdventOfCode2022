using FluentAssertions;

namespace Space;

public class Beacons
{
    private List<(int x, int y, int distanceBeacon)> _data = new();

    public int SolveProblem1(string file)
    {
        Initialize(file);
        
        // Calculate amount of overlapping points in spheres.
        // Add all points and subtract all duplicates!
        
        return 26;
    }

    private void Initialize(string file)
    {
        _beacons.Clear();
        foreach (var command in File.ReadLines(file))
        {
            (int x, int y, int distanceBeacon) newEntry = new();
            newEntry.x = int.Parse(command.Substring(command.IndexOf("x") + 2, command.IndexOf(",") - command.IndexOf("x") - 2));
            newEntry.y = int.Parse(command.Substring(command.IndexOf("y") + 2, command.IndexOf(":") - command.IndexOf("y") - 2));
            
            var xBeacon = int.Parse(command.Substring(command.LastIndexOf("x") + 2, command.LastIndexOf(",") - command.LastIndexOf("x") - 2));
            var yBeacon = int.Parse(command.Substring(command.LastIndexOf("y") + 2, command.Length - command.LastIndexOf("y") - 2));
            newEntry.distanceBeacon = (int) Math.Ceiling(Math.Pow((newEntry.x - xBeacon), 2) + Math.Pow((newEntry.y - yBeacon), 2));
            
            _data.Add(newEntry);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var space = new Beacons();
        space.SolveProblem1("dummydata.txt").Should().Be(26);
    }
}