using FluentAssertions;

namespace Obsidian;

public class Obsidian
{
    private List<(int x, int y, int z)> _cubes = new ();
    private List<(int x, int y, int z)> _airTrapped = new();
    private int _surfaceArea = 0;
    
    public int SolveProblem(string file, bool secondExercise = false)
    {
        Initialize(file);
        
        _surfaceArea += 6; // i = 0 adds + 6, loop starts from i = 1 due to sampling.
        for (var i = 1; i != _cubes.Count; i++)
        {
            var considerCube = _cubes[i];
            var compareWith = _cubes.GetRange(0, i);

            _surfaceArea += FindSurfaceAreaForCube(considerCube, compareWith);
        }

        if (secondExercise)
        {
            FindContainedAirCubes();
            for (var i = 0; i != _airTrapped.Count; i++)
            {
                var considerAir = _airTrapped[i];
                var compareWith = _airTrapped.GetRange(0, i);
                _surfaceArea -= FindSurfaceAreaForCube(considerAir, compareWith);;
            }
        }
        
        return _surfaceArea;
    }

    private void FindContainedAirCubes()
    {
        var xs = _cubes.Select(cube => cube.x);
        var ys = _cubes.Select(cube => cube.y);
        var zs = _cubes.Select(cube => cube.z);

        for (var x = xs.Min() + 1; x < xs.Max(); x++)
        {
            for (var y = ys.Min() + 1; y < ys.Max(); y++)
            {
                for (var z = zs.Min() + 1; z < zs.Max(); z++)
                {
                    var xCheck = _cubes.Where(entry => entry.y == y && entry.z == z);
                    var minX = xCheck.Any() ? xCheck.Select(entry => entry.x).Min() : 0;
                    var maxX = xCheck.Any() ? xCheck.Select(entry => entry.x).Max() : 0;
                    var yCheck = _cubes.Where(entry => entry.x == x && entry.z == z);
                    var minY = yCheck.Any() ? yCheck.Select(entry => entry.y).Min() : 0;
                    var maxY = yCheck.Any() ? yCheck.Select(entry => entry.y).Max() : 0;
                    var zCheck = _cubes.Where(entry => entry.x == x && entry.y == y);
                    var minZ = zCheck.Any() ? zCheck.Select(entry => entry.z).Min() : 0;
                    var maxZ = zCheck.Any() ? zCheck.Select(entry => entry.z).Max() : 0;

                    if (!_cubes.Contains((x, y, z)) && IsWithin(x, minX, maxX) && IsWithin(y, minY, maxY) &&
                        IsWithin(z, minZ, maxZ))
                    {
                        _airTrapped.Add((x, y, z));
                    }
                }
            }
        }
    }

    private static int FindSurfaceAreaForCube((int x, int y, int z) considerCube, List<(int x, int y, int z)> compareWith)
    {
        int surfaceArea = 6;
        if (compareWith.Any(
                entry => entry.x == considerCube.x + 1 && entry.y == considerCube.y && entry.z == considerCube.z))
        {
            surfaceArea -= 2;
        }

        if (compareWith.Any(
                entry => entry.x == considerCube.x - 1 && entry.y == considerCube.y && entry.z == considerCube.z))
        {
            surfaceArea -= 2;
        }

        if (compareWith.Any(
                entry => entry.y == considerCube.y + 1 && entry.x == considerCube.x && entry.z == considerCube.z))
        {
            surfaceArea -= 2;
        }

        if (compareWith.Any(
                entry => entry.y == considerCube.y - 1 && entry.x == considerCube.x && entry.z == considerCube.z))
        {
            surfaceArea -= 2;
        }

        if (compareWith.Any(
                entry => entry.z == considerCube.z + 1 && entry.x == considerCube.x && entry.y == considerCube.y))
        {
            surfaceArea -= 2;
        }

        if (compareWith.Any(
                entry => entry.z == considerCube.z - 1 && entry.x == considerCube.x && entry.y == considerCube.y))
        {
            surfaceArea -= 2;
        }

        return surfaceArea;
    }

    public static bool IsWithin(int value, int minimum, int maximum)
    {
        return value > minimum && value < maximum;
    }

    private void Initialize(string file)
    {
        _cubes.Clear();
        _airTrapped.Clear();
        _surfaceArea = 0;
        foreach (var line in File.ReadLines(file))
        {
            var coordinates = Array.ConvertAll(line.Split(","), s => int.Parse(s));;
            _cubes.Add((coordinates[0], coordinates[1], coordinates[2]));
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var obsidian = new Obsidian();
        obsidian.SolveProblem("firstdata.txt").Should().Be(10);
        obsidian.SolveProblem("dummydata.txt").Should().Be(64);
        obsidian.SolveProblem("dummydata.txt", true).Should().Be(58);
        var answer1 = obsidian.SolveProblem("data.txt");
        var answer2 = obsidian.SolveProblem("data.txt", true);
        Console.WriteLine($"Answers = {answer1} and {answer2}");
    }
}