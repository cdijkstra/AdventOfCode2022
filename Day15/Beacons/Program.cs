using FluentAssertions;

namespace Space;

public class Beacons
{
    private List<(int x, int y, int xBeacon, int yBeacon, int distanceBeacon)> _data = new();
    private List<(int x, int y)> _excludedPointsRow = new();
    HashSet<(int x, int y)> _toCheck = new();
    
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
    
    private void FillGrid()
    {
        foreach (var valueTuple in _data)
        {
            // Get coordinates next to Manhattan circle
            var left = (valueTuple.x - valueTuple.distanceBeacon - 1, valueTuple.y);
            var right = (valueTuple.x + valueTuple.distanceBeacon + 1, valueTuple.y);
            var down = (valueTuple.x, valueTuple.y - valueTuple.distanceBeacon - 1);
            var up = (valueTuple.x, valueTuple.y + valueTuple.distanceBeacon + 1);

            List<(int x, int y)> coordinates = new();
            coordinates
                .Concat(GetCoordinatesBetween(left, up))
                .Concat(GetCoordinatesBetween(up, right))
                .Concat(GetCoordinatesBetween(right, down))
                .Concat(GetCoordinatesBetween(down, left))
                .ToList();
            
            foreach (var coordinate in coordinates)
            {
                
            }
        }
    }
    
    private static IEnumerable<(int x, int y)> GetCoordinatesBetween((int x, int y) start, (int x, int y) end)
    {
        var (left, right) = start.x <= end.x ? (start, end) : (end, start);
        var coordinates = new List<(int x, int y)>();

        if (left.y <= end.y)
        {
            for (var offset = 0; offset <= right.x - left.x; ++offset)
            {
                coordinates.Add((left.x + offset, left.y + offset));
            }
        }
        else
        {
            for (var offset = 0; offset <= right.x - left.x; ++offset)
            {
                coordinates.Add((left.x + offset, left.y - offset));
            }
        }

        return coordinates;
    }
    
    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        FillRow(row);

        return _excludedPointsRow.Distinct().Count();
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file);
        FillGrid();

        return _toCheck.Count();
    }

    private void Initialize(string file)
    {
        _data.Clear();
        _excludedPointsRow.Clear();
        _data.Clear();

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
            
            _data.Add(newEntry);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var beacon = new Beacons();
        // beacon.SolveProblem1("dummydata.txt", 10).Should().Be(26);
        var answer = beacon.SolveProblem1("data.txt", 2000000);
        Console.WriteLine(answer);
        beacon.SolveProblem2("data.txt");
    }
}