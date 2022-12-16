﻿using FluentAssertions;

namespace RopeBridge;


public class Regolith
{
    private List<List<char>> _grid = new ();
    private List<(int x, int y)> _sandCoordinates = new();

    private const int _xMax = 650;
    private const int _yMax = 600;
    
    public int SolveProblem1(string file)
    {
        Initialize(file);
        return SolvePuzzle(file);
    }
    
    public int SolveProblem2(string file)
    {
        Initialize(file);
        return SolvePuzzle(file);
    }
    
    private void Initialize(string file)
    {
        _sandCoordinates.Clear();
        _grid.Clear();
        foreach (var idx in Enumerable.Range(0, _yMax))
        {
            List<char> newRow = Enumerable.Repeat('.', _xMax).ToList();
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
                        var length = intEntries[lineIdx + 1].xs - intEntries[lineIdx].xs + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].xs, length))
                        {
                            _grid[intEntries[lineIdx].ys][newEntry] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[intEntries[lineIdx].ys][newEntry] = '#';
                        }
                    }
                }
                else if (intEntries[lineIdx].ys != intEntries[lineIdx + 1].ys)
                {
                    if (intEntries[lineIdx].ys < intEntries[lineIdx + 1].ys)
                    {
                        var length = intEntries[lineIdx + 1].ys - intEntries[lineIdx].ys + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx].ys, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].xs] = '#';
                        }
                    }
                    else
                    {
                        var length =  intEntries[lineIdx].xs - intEntries[lineIdx + 1].xs + 1;
                        foreach (var newEntry in Enumerable.Range(intEntries[lineIdx + 1].xs, length))
                        {
                            _grid[newEntry][intEntries[lineIdx].xs] = '#';
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
        var startPosition = (500, 0);
        (int x, int y) position = startPosition;
        
        for (var sand = 0; sand != 23; sand++)
        {
            Console.WriteLine($"x - {_grid.Count} and y - {_grid[0].Count}");
            
            // Find position where it falls down
            var sandCoordinates = GetFinalCoordinates(position);
            if (sandCoordinates.y != _yMax)
            {
                _sandCoordinates.Add(sandCoordinates);
                _grid[sandCoordinates.y][sandCoordinates.x] = 'o';
            }
            else
            {
                Console.WriteLine("Falling forever");
            }

            for (var idy = 0; idy != 10; idy++)
            {
                for (var idx = 494; idx != 504; idx++)
                {
                    Console.Write(_grid[idy][idx]);
                }
                Console.WriteLine();
            }
        }
        

        return _sandCoordinates.Count;
    }

    private (int x, int y) GetFinalCoordinates((int x, int y) position, bool falling = true)
    {
        if (falling)
        {
            while (_grid[position.y + 1][position.x] == '.')
            {
                position.y++;

                if (position.y + 1 == _yMax)
                {
                    return (position.x, _yMax);
                }
            }
        }

        if (_grid[position.y][position.x] == 'o' || _grid[position.y][position.x] == '#')
        {
            return (0, 0);
        }
        if (_grid[position.y + 1][position.x] == '#')
        {
            Console.WriteLine($"Finished at {position.y},{position.x}");
            // Finished
            return position;
        }
        if (_grid[position.y + 1][position.x] == 'o')
        {
            var toLeftDiagonal = GetFinalCoordinates((position.x - 1, position.y + 1));
            if (toLeftDiagonal != (0,0))
            {
                Console.WriteLine($"Finished LEFT at {position.y},{position.x}");
                return toLeftDiagonal;
            }
            
            var toRightDiagonal = GetFinalCoordinates((position.x + 1, position.y + 1));
            if (toRightDiagonal != (0,0))
            {
                Console.WriteLine($"Finished RIGHT at {position.y},{position.x}");
                return toRightDiagonal;
            }

            if (toLeftDiagonal == (0, 0) && toRightDiagonal == (0, 0))
            {
                Console.WriteLine($"Finished UP at {position.y},{position.x}");
                return (position.x, position.y);
            }
        }

        return (0, 0);
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var regolith = new Regolith();
        regolith.SolveProblem1("dummydata.txt");
    }
}