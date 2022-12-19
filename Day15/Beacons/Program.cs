using FluentAssertions;

namespace Space;

public class Beacons
{
    private List<(int x, int y, int xBeacon, int yBeacon, int distanceBeacon)> _data = new();
    private List<(int x, int y)> _excludedPointsRow = new();
    private List<List<bool>> _beacons = new();
    
    private void FillRow(int row)
    {
        foreach (var valueTuple in _data)
        {
            for (var idy = -valueTuple.distanceBeacon; idy <= valueTuple.distanceBeacon; idy++)
            {
                if (valueTuple.y + idy == row)
                {
                    for (var idx = -valueTuple.distanceBeacon + Math.Abs(idy); idx <= valueTuple.distanceBeacon - Math.Abs(idy); idx++)
                    {
                        _excludedPointsRow.Add((valueTuple.x + idx, valueTuple.y + idy));
                    }
                }
            }
        }

        foreach (var valueTuple in _data)
        {
            var beacon = (valueTuple.xBeacon, valueTuple.yBeacon);
            if (_excludedPointsRow.Contains(beacon))
            {
                _excludedPointsRow.Remove(beacon);
            }
        }
    }
    
    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        FillRow(row);

        return _excludedPointsRow.Distinct().Count();
    }

    private void Initialize(string file)
    {
        _data.Clear();
        _excludedPointsRow.Clear();
        for (var idx = 0; idx < 4000000; idx++)
        {
            _beacons.Add(Enumerable.Repeat(false, 4000000).ToList());
        }
        
        Console.WriteLine("Still here");


        foreach (var command in File.ReadLines(file))
        {
            (int x, int y, int xBeacon, int yBeacon, int distanceBeacon) newEntry = new();
            newEntry.x = int.Parse(command.Substring(command.IndexOf("x") + 2, command.IndexOf(",") - command.IndexOf("x") - 2));
            newEntry.y = int.Parse(command.Substring(command.IndexOf("y") + 2, command.IndexOf(":") - command.IndexOf("y") - 2));
            
            var xBeacon = int.Parse(command.Substring(command.LastIndexOf("x") + 2, command.LastIndexOf(",") - command.LastIndexOf("x") - 2));
            var yBeacon = int.Parse(command.Substring(command.LastIndexOf("y") + 2, command.Length - command.LastIndexOf("y") - 2));
            newEntry.xBeacon = xBeacon;
            newEntry.yBeacon = yBeacon;
            newEntry.distanceBeacon = Math.Abs(newEntry.x - xBeacon) + Math.Abs(newEntry.y - yBeacon);
            
            // Console.WriteLine($"Concluded that {xBeacon},{yBeacon} and {newEntry.distanceBeacon}");
            
            _data.Add(newEntry);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var beacon = new Beacons();
        beacon.SolveProblem1("dummydata.txt", 10).Should().Be(26);
        var answer = beacon.SolveProblem1("data.txt", 2000000);
        Console.WriteLine(answer);
    }
}