using FluentAssertions;

namespace Space;

public class Beacons
{
    private List<Sensor> _sensors = new();
    private List<Coordinate> _excludedPointsRow = new();

    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        FillRow(row);

        return _excludedPointsRow.Distinct().Count();
    }
    
    public long SolveProblem2(string file, int upperlimit)
    {
        Initialize(file);
        return Solvepuzzle2(upperlimit);
    }
    
    private void FillRow(int row)
    {
        foreach (var valueTuple in _sensors)
        {
            for (var idy = -valueTuple.DistanceToBeacon; idy <= valueTuple.DistanceToBeacon; idy++)
            {
                if (valueTuple.Position.y + idy != row) continue;
                for (var idx = -valueTuple.DistanceToBeacon + Math.Abs(idy); idx <= valueTuple.DistanceToBeacon - Math.Abs(idy); idx++)
                {
                    _excludedPointsRow.Add(new Coordinate() { x = valueTuple.Position.x + idx, y = valueTuple.Position.y + idy });
                }
            }
        }

        foreach (var beacon in _sensors.Select(valueTuple => valueTuple.ClosestBeacon).Where(beacon => _excludedPointsRow.Contains(beacon.Position)))
        {
            _excludedPointsRow.Remove(beacon.Position);
        }
    }
    
    private long Solvepuzzle2(int upperLimit)
    {
        foreach (var sensor in _sensors)
        {
            foreach (var edgeCoordinate in RetrieveEdgeCoordinates(sensor))
            {
                if (InvalidRange(upperLimit, edgeCoordinate)) continue;
                
                if (CoordinatesNotWithinRange(edgeCoordinate))
                {
                    return TuningFrequency(edgeCoordinate);
                }
            }
        }
        
        return 0;
    }

    private static long TuningFrequency(Coordinate edgeCoordinate)
    {
        return edgeCoordinate.x * 4000000 + edgeCoordinate.y;
    }

    private bool CoordinatesNotWithinRange(Coordinate edgeCoordinate)
    {
        return !_sensors.Any(sens => sens.DistanceToBeacon >= sens.DistanceTo(edgeCoordinate));
    }

    private static bool InvalidRange(int upperLimit, Coordinate edgeCoordinate)
    {
        return edgeCoordinate.x < 0 || edgeCoordinate.x > upperLimit ||
               edgeCoordinate.y < 0 || edgeCoordinate.y > upperLimit;
    }

    private static List<Coordinate> RetrieveEdgeCoordinates(Sensor valueTuple)
    {
        // Get coordinates next to Manhattan circle
        var left = new Coordinate()
            { x = valueTuple.Position.x - valueTuple.DistanceToBeacon - 1, y = valueTuple.Position.y };
        var right = new Coordinate()
            { x = valueTuple.Position.x + valueTuple.DistanceToBeacon + 1, y = valueTuple.Position.y };
        var up = new Coordinate()
            { x = valueTuple.Position.x, y = valueTuple.Position.y + valueTuple.DistanceToBeacon + 1 };
        var down = new Coordinate()
            { x = valueTuple.Position.x, y = valueTuple.Position.y - valueTuple.DistanceToBeacon - 1 };

        var edgeCoordinates = new List<Coordinate>();
        edgeCoordinates.AddRange(GetCoordinatesBetween(left, up));
        edgeCoordinates.AddRange(GetCoordinatesBetween(down, right));
        edgeCoordinates.AddRange(GetCoordinatesBetween(up, right));
        edgeCoordinates.AddRange(GetCoordinatesBetween(left, down));
        return edgeCoordinates;
    }

    private static IEnumerable<Coordinate> GetCoordinatesBetween(Coordinate start, Coordinate end)
    {
        var (left, right) = start.x <= end.x ? (start, end) : (end, start);
        var coordinates = new List<Coordinate>();

        if (left.y <= end.y)
        {
            for (var offset = 0; offset <= right.x - left.x; ++offset)
            {
                coordinates.Add(new Coordinate() { x = left.x + offset, y = left.y + offset });
            }
        }
        else
        {
            for (var offset = 0; offset <= right.x - left.x; ++offset)
            {
                coordinates.Add(new Coordinate() { x = left.x + offset, y = left.y - offset });
            }
        }

        return coordinates;
    }

    private void Initialize(string file)
    {
        _sensors.Clear();
        _excludedPointsRow.Clear();
        _sensors.Clear();

        foreach (var command in File.ReadLines(file))
        {
            Beacon beacon = new()
            {
                Position =
                {
                    x = int.Parse(command.Substring(command.LastIndexOf("x") + 2,
                        command.LastIndexOf(",") - command.LastIndexOf("x") - 2)),
                    y = int.Parse(command.Substring(command.LastIndexOf("y") + 2,
                        command.Length - command.LastIndexOf("y") - 2))
                }
            };
            
            Sensor sensor = new()
            {
                Position =
                {
                    x = int.Parse(command.Substring(command.IndexOf("x") + 2, command.IndexOf(",") - command.IndexOf("x") - 2)),
                    y = int.Parse(command.Substring(command.IndexOf("y") + 2, command.IndexOf(":") - command.IndexOf("y") - 2))
                },
                ClosestBeacon = beacon
            };
            
            _sensors.Add(sensor);
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var beacon = new Beacons();
        beacon.SolveProblem1("dummydata.txt", 10).Should().Be(26);
        beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
        var answer1 = beacon.SolveProblem1("data.txt", 2000000);
        var answer2 = beacon.SolveProblem2("data.txt", 4000000);
        Console.WriteLine($"{answer1} and {answer2}");
    }
}