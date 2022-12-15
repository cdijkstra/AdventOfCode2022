using FluentAssertions;

namespace RopeBridge;


public class Regolith
{
    private List<List<char>> _grid = new ();

    private const int _xMax = 600;
    private const int _yMax = 600;
    
    public int SolveProblem1(string file)
    {
        Initialize(file, 2);
        return SolvePuzzle(file);
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file, 10);
        return SolvePuzzle(file);
    }
    
    private void Initialize(string file)
    {
        _grid.Clear();
        foreach (var idx in Enumerable.Range(0, _xMax))
        {
            List<char> newRow = Enumerable.Repeat('.', _yMax).ToList();
            _grid.Add(newRow);
        }
        
        foreach (var line in File.ReadLines(file))
        {
            var entries = line.Split(" -> ");
            List<(int xs, int ys)> intEntries = entries.Select(entry => (int.Parse(entry.Split(",")[0]), int.Parse(entry.Split(",")[1]))).ToList();
            for (var lineIdx = 0; lineIdx != intEntries.Count - 1; lineIdx++)
            {
                if (intEntries[lineIdx].xs != intEntries[lineIdx + 1].xs)
                {
                    if (intEntries[lineIdx].xs < intEntries[lineIdx + 1].xs)
                    {
                        var length = intEntries[lineIdx + 1].xs - intEntries[lineIdx].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].xs, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].ys] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].ys] = '#';
                        }
                    }
                }
                else if (intEntries[lineIdx].ys != intEntries[lineIdx + 1].ys)
                {
                    if (intEntries[lineIdx].ys < intEntries[lineIdx + 1].ys)
                    {
                        var length = intEntries[lineIdx + 1].ys - intEntries[lineIdx].ys;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].ys, length))
                        {
                            _grid[intEntries[lineIdx].xs][newEntry] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[intEntries[lineIdx].xs][newEntry] = '#';
                        }
                    }
                }
                else
                {
                    throw new Exception("QUEEEEE");
                }
            }
            
        }
    }

    private int SolvePuzzle(string file)
    {
        
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var ropeBridge = new RopeBridge();
        ropeBridge.SolveProblem1("dummydata.txt");

}