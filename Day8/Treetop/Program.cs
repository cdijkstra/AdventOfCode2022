using System.Text.RegularExpressions;
using System.Xml;
using FluentAssertions;

namespace Treetop;

public class Treetop
{
    private List<List<int>> _trees = new();
    private int _count;
    private List<int> _senticScores = new();

    public int SolveProblem1(string file)
    {
        Initialize(file);

        _count += (2 * (_trees.Count + _trees[0].Count) - 4);
        for (int idx = 1; idx < _trees.Count - 1; idx++)
        {
            for (int idy = 1; idy < _trees[idx].Count - 1; idy++)
            {
                var treeValue = _trees[idx][idy];

                if (_trees[idx].GetRange(0, idy).All(x => x < treeValue) ||
                    _trees[idx].GetRange(idy + 1, _trees[idx].Count - idy - 1).All(x => x < treeValue) ||
                    _trees.Select(x => x[idy]).ToList().GetRange(0, idx).All(y => y < treeValue) ||
                    _trees.Select(x => x[idy]).ToList().GetRange(idx + 1, _trees.Count() - idx - 1).All(y => y < treeValue))
                {
                    Console.WriteLine($"Added {_trees[idx][idy]} at {idx}{idy}");
                    _count++;
                }
            }
        }

        return _count;
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file);
        
        for (var idx = 1; idx < _trees.Count - 1; idx++)
        {
            for (var idy = 1; idy < _trees[idx].Count - 1; idy++)
            {
                var treeHeight = _trees[idx][idy];
                var xScoreLeft = 0;
                for (var yLeft = idy - 1; yLeft >= 0; yLeft--)
                {
                    xScoreLeft += 1;
                    if (_trees[idx][yLeft] >= treeHeight)
                        break;

                }
                
                var xScoreRight = 0;
                for (var yRight = idy + 1; yRight < _trees[idx].Count; yRight++)
                {
                    xScoreRight += 1;
                    if (_trees[idx][yRight] >= treeHeight)
                        break;

                }
                
                var yUp = 0;
                for (var xLeft = idx - 1; xLeft >= 0; xLeft--)
                {
                    yUp += 1;
                    if (idx == 3 && idy == 2)
                    {
                        Console.WriteLine($"Found {_trees[xLeft][idy]}");
                    }
                    
                    if (_trees[xLeft][idy] >= treeHeight)
                        break;

                }
                
                var yDown = 0;
                for (var xRight = idx + 1; xRight < _trees.Count; xRight++)
                {
                    yDown += 1;
                    if (_trees[xRight][idy] >= treeHeight)
                        break;

                }

                var entry = xScoreLeft * xScoreRight * yUp * yDown;
                _senticScores.Add(entry);
            }
        }

        return _senticScores.Max();
    }

    private void Initialize(string file)
    {
        _trees.Clear();
        _senticScores.Clear();
        _count = 0;
        var treeMap = File.ReadLines(file).ToList();
        for (var idx = 0; idx < treeMap.Count(); idx++)
        {
            List<int> newRow = Array.ConvertAll(treeMap[idx].ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            _trees.Add(newRow);
        }
    }
}


internal static class Program
{
    static void Main(string[] args)
    {
        var treetop = new Treetop();
        treetop.SolveProblem1("dummydata.txt").Should().Be(21);
        treetop.SolveProblem2("dummydata.txt").Should().Be(8);

        var solution1 = treetop.SolveProblem1("data.txt");
        var solution2 = treetop.SolveProblem2("data.txt");
        
        Console.WriteLine($"Solutions are {solution1} and {solution2}");
    }
}