using FluentAssertions;

namespace Space;

public class Beacons
{
    private List<(int x, int y, int xBeacon, int yBeacon, int distanceBeacon)> _data = new();
    private List<(int x, int y)> _excludedPointsRow = new();

    // private int countPointsInManhattanCircle(int sphere)
    // {
    //     if (sphere == 0)
    //         return 1;
    //     
    //     return 4 * sphere + countPointsInManhattanCircle(sphere - 1);
    // }
    
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
        
        // Calculate amount of overlapping points in spheres.
        // Add all points and subtract all duplicates!
        // int excludedPlaces = 0;
        // foreach (var valueTuple in _data)
        // {
        //     excludedPlaces += countPointsInManhattanCircle(valueTuple.distanceBeacon);
        // }

        // int overlappingPoints = 0;
        // int row10overlap = 0;
        // // Find all possible pairs in the list and find overlap
        // for (int i = 0; i < _data.Count - 1; i++)
        // {
        //     for (int j = i + 1; j < _data.Count; j++)
        //     {
        //         // Console.WriteLine($"Considering {_data[i].x},{_data[i].y} and {_data[j].x},{_data[j].y}");
        //         // First check if they have any overlap
        //         if (Math.Abs(_data[i].x - _data[j].x) + Math.Abs(_data[i].y - _data[j].y) <= _data[i].distanceBeacon + _data[j].distanceBeacon)
        //         {
        //             // Move left to (distancebeacon, distancebeacon), i.e. subtract vector (_data[i].x, _data[i].y) so concentric sphere can be fully in first quadrant
        //             (int x, int y, int distanceBeacon) newLeft = (_data[i].distanceBeacon >= _data[j].distanceBeacon) ? 
        //                 (_data[i].distanceBeacon, _data[i].distanceBeacon, _data[i].distanceBeacon) :
        //                 (_data[j].distanceBeacon, _data[j].distanceBeacon, _data[j].distanceBeacon);
        //             // Move right to (distancebeacon + dx, distancebeacon + dy)
        //             (int x, int y, int distanceBeacon) newRight = (_data[i].distanceBeacon >= _data[j].distanceBeacon) ? 
        //                 (_data[j].x - _data[i].x + _data[i].distanceBeacon, _data[j].y - _data[i].y + _data[i].distanceBeacon, _data[j].distanceBeacon) :
        //                 (_data[i].x - _data[j].x + _data[j].distanceBeacon, _data[i].y - _data[j].y + _data[j].distanceBeacon, _data[i].distanceBeacon);
        //
        //             var totalDistance = 2 * newLeft.distanceBeacon + 1;
        //             bool[,] bla = new bool[totalDistance, totalDistance];
        //
        //             for (var idx = -newLeft.distanceBeacon; idx <= newLeft.distanceBeacon; idx++)
        //             {
        //                 for (var idy = -newLeft.distanceBeacon + Math.Abs(idx); idy <= newLeft.distanceBeacon - Math.Abs(idx); idy++)
        //                 {
        //                     if (Math.Abs(newRight.x - (newLeft.x + idx)) + Math.Abs(newRight.y - (newLeft.y + idy)) <= newRight.distanceBeacon)
        //                     {
        //                         overlappingPoints++;
        //                         if (_data[i].distanceBeacon >= _data[j].distanceBeacon &&
        //                             newLeft.x + idx + _data[i].x == 10 ||
        //                             _data[i].distanceBeacon < _data[j].distanceBeacon &&
        //                             newLeft.x + idx + _data[j].x == 10)
        //                         {
        //                             row10overlap++;
        //                         }
        //                     }
        //                 }
        //             }
        //             
        //             // Console.WriteLine($"Found {newLeft.x},{newLeft.y} - D={newLeft.distanceBeacon} and {newRight.x},{newRight.y} - D={newRight.distanceBeacon}");
        //         }
        //     }
        // }
        
        return _excludedPointsRow.Distinct().Count();
    }

    private void Initialize(string file)
    {
        _data.Clear();
        _excludedPointsRow.Clear();

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