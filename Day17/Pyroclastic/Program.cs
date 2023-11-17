using FluentAssertions;

namespace Space;

public class Pyroclastic
{
    private readonly List<List<char[]>> _rocks = new();
    private List<List<char>> _cave = new();
    private List<char> _jetPattern = new();
    private int _currentIndex = 0;
    
    private List<char> _bottom = new()
    {
        '+', '-', '-', '-', '-', '-', '-', '-', '+'
    };
    
    private  List<char> _emptyEntry = new()
    {
        '|', '.', '.', '.', '.', '.', '.', '.', '|'
    };


    public int SolveProblem1(string file, int row)
    {
        Initialize(file);
        
        for (var repeats = 0; repeats != row; repeats++)
        {
            PlaceRock(_rocks[repeats % _rocks.Count]);
            ApplyJetAndFallUntilFinished(_rocks[repeats % _rocks.Count]);
        }

        Console.WriteLine($"Answer is {_cave.Count - 1}");
        return _cave.Count - 1;
    }
    
    private int FindLevelHighestRock(bool moving)
    {
        int highestRow = 1;
        var findChar = moving ? '@' : '#';
        for (var idx = _cave.Count - 1; idx > 0; idx--)
        {
            if (!_cave[idx].Contains(findChar)) continue;
            highestRow = idx;
            break;  // No need to continue searching in this level once '#' is found
        }
        
        return highestRow;
    }

    
    private void PlaceRock(List<char[]> rock)
    {
        var totalHeight = _cave.Count;
        var highestRockLevel = FindLevelHighestRock(false);
        Console.WriteLine($"{totalHeight} and {highestRockLevel}");
        var requiredEmptyRows = 3;
        
        _cave.AddRange(Enumerable.Repeat(_emptyEntry, requiredEmptyRows));

        foreach (var rockPart in rock.Reverse<char[]>())
        {
            int numberOfDotsAfterRock = 5 - rockPart.Length;
            
            var newEntry = new List<char> { '|', '.', '.' }
                .Concat(rockPart)
                .Concat(Enumerable.Repeat('.', numberOfDotsAfterRock))
                .Concat(new List<char> { '|' })
                .ToList();     
            _cave.Add(newEntry);
        }

        // foreach (var caveEntry in _cave.Reverse<List<char>>())
        // {
        //     caveEntry.ForEach(x => Console.Write(x));
        //     Console.WriteLine();
        // }
        // Console.WriteLine("Rock placed");
    }

    private void ApplyJetAndFallUntilFinished(List<char[]> rock)
    {
        var verticalRockEntries = rock.Count;
        
        bool continueFalling = true;
        while (continueFalling == true)
        {
            var movementDirection = _jetPattern[_currentIndex++ % _jetPattern.Count];

            var highestEntry = FindLevelHighestRock(true);
            var lowestEntry = highestEntry - (verticalRockEntries - 1);
            
            // Applying JET
            ApplyJet(movementDirection, verticalRockEntries, highestEntry);
            // Console.WriteLine($"Jet applied to {movementDirection}");
            // foreach (var caveEntry in _cave.Reverse<List<char>>())
            // {
            //     caveEntry.ForEach(x => Console.Write(x));
            //     Console.WriteLine();
            // }
            
            // Falling
            if (Enumerable.Range(0, verticalRockEntries)
                .All(verticalEntry => CanMoveDown(FindLevelHighestRock(true) - verticalRockEntries + 1, verticalEntry)))
            {
                MoveRockRowDown(lowestEntry, verticalRockEntries);
                
                // Console.WriteLine("Rock moved down");
                // foreach (var caveEntry in _cave.Reverse<List<char>>())
                // {
                //     caveEntry.ForEach(x => Console.Write(x));
                //     Console.WriteLine();
                // }
            }
            else
            {
                continueFalling = false;
                foreach (var idx in Enumerable.Range(0, verticalRockEntries))
                {
                    List<char> row = _cave[highestEntry - idx];
                    // Find indices of '@' in the row
                    List<int> atIndexList = Enumerable.Range(0, row.Count)
                        .Where(i => row[i] == '@')
                        .ToList();
                    
                    atIndexList.ForEach(i => row[i] = '#');
                }

                // Console.WriteLine("Rock stopped falling");
                // foreach (var caveEntry in _cave.Reverse<List<char>>())
                // {
                //     caveEntry.ForEach(x => Console.Write(x));
                //     Console.WriteLine();
                // }
            }
        }
    }

    private void ApplyJet(char movementDirection, int verticalRockEntries, int highestEntry)
    {
        switch (movementDirection)
        {
            case '<':
            {
                if (Enumerable.Range(0, verticalRockEntries)
                    .All(row => CanMoveLeft(highestEntry - row)))
                {
                    MoveRockRowLeft(highestEntry, verticalRockEntries);
                }
                else
                {
                    Console.WriteLine("Could not move left");
                }

                break;
            }
            case '>':
            {
                if (Enumerable.Range(0, verticalRockEntries)
                    .All(row => CanMoveRight(highestEntry - row)))
                {
                    MoveRockRowRight(highestEntry, verticalRockEntries);
                }
                else
                {
                    Console.WriteLine("Could not move right");
                }

                break;
            }
            default:
                throw new Exception("No idea what this operator is");
        }
    }

    private void MoveRockRowDown(int lowestEntry, int verticalEntries)
    {
        foreach (var extraHeight in Enumerable.Range(0, verticalEntries))
        {
            var heightEntry = lowestEntry + extraHeight;
            
            List<int> indexes = Enumerable.Range(0, 8)
                .Where(i => _cave[heightEntry][i] == '@')
                .ToList();
    
            List<char> newRow = new List<char>(_cave[heightEntry - 1]);
            indexes.ForEach(idx => newRow[idx] = '@');
            _cave[heightEntry - 1] = newRow;

            indexes.ForEach(idx => _cave[heightEntry][idx] = '.');
        }
        
        var highestEntryMovingRock = lowestEntry + verticalEntries - 1;
        if (FindLevelHighestRock(false) < highestEntryMovingRock)
        {
            _cave.RemoveAt(highestEntryMovingRock);
        }
    }

    private bool CanMoveDown(int lowestEntry, int verticalEntry)
    {
        if (lowestEntry == 1)
        {
            return false; // Bottom
        }

        var heightEntry = lowestEntry + verticalEntry;
        
        List<int> blocksAtLowestHeight = Enumerable.Range(0, 8)
            .Where(i => _cave[heightEntry][i] == '@')
            .ToList();

        return blocksAtLowestHeight.All(entry => _cave[heightEntry - 1][entry] != '#' && _cave[heightEntry - 1][entry] != '-');
    }
    
    private void MoveRockRowLeft(int highestEntry, int verticalRowEntries)
    {
        foreach (var idx in Enumerable.Range(0, verticalRowEntries))
        {
            List<char> row = _cave[highestEntry - idx];
            // Find indices of '@' in the row
            List<int> atIndexList = Enumerable.Range(0, row.Count)
                .Where(i => row[i] == '@')
                .ToList();

            // Move each '@' one entry to the left
            foreach (int atIndex in atIndexList.Where(idx => idx > 0))
            {
                // Swap the positions of '@' and the character to its left
                (row[atIndex], row[atIndex - 1]) = (row[atIndex - 1], row[atIndex]);
            }
        }
    }

    private void MoveRockRowRight(int highestEntry, int verticalRockEntries)
    {
        foreach (var idx in Enumerable.Range(0, verticalRockEntries))
        {
            List<char> row = _cave[highestEntry - idx];
            // Find indices of '@' in the row
            List<int> atIndexList = Enumerable.Range(0, row.Count)
                .Where(i => row[i] == '@')
                .OrderByDescending(i => i)
                .ToList();

            // Move each '@' one entry to the left
            foreach (int atIndex in atIndexList)
            {
                (row[atIndex], row[atIndex + 1]) = (row[atIndex + 1], row[atIndex]);
            }
        }
    }

    private bool CanMoveLeft(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < _cave.Count)
        {
            var indexFirstRock = _cave[rowIndex].IndexOf('@');
            return _cave[rowIndex][indexFirstRock - 1] != '|' && _cave[rowIndex][indexFirstRock - 1] != '#';
        }

        // Handle the case where the row index is out of bounds
        return false;
    }
    
    private bool CanMoveRight(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < _cave.Count)
        {
            var indexLastRock = _cave[rowIndex].LastIndexOf('@');
            return _cave[rowIndex][indexLastRock + 1] != '|' && _cave[rowIndex][indexLastRock + 1] != '#';
        }

        // Handle the case where the row index is out of bounds
        return false;
    }

    private long Solvepuzzle2(int upperLimit)
    {
        return 0;
    }

    private void Initialize(string file)
    {
        _currentIndex = 0;
        _cave.Clear();
        _rocks.Clear();
        
        _jetPattern.Clear();
        _jetPattern = File.ReadAllText(file).ToList();
        
        var newRockEntry = new List<char>();
        var input = File.ReadLines("../../../input.txt");
        
        List<char[]> currentRock = new();
        foreach (var line in input)
        {
            // Check if the line is empty, indicating the end of the current rock
            if (string.IsNullOrWhiteSpace(line) && currentRock.Count > 0)
            {
                _rocks.Add(new List<char[]>(currentRock));
                currentRock.Clear();
            }
            else
            {
                // Add the line to the current rock
                currentRock.Add(line.ToCharArray());
            }
        }
        
        if (currentRock.Count > 0)
        {
            _rocks.Add(new List<char[]>(currentRock));
        }
        
        _cave.Add(_bottom);
    }
    
    
    internal static class Program
    {
        static void Main(string[] args)
        {
            var pyro = new Pyroclastic();
            pyro.SolveProblem1("../../../dummydata.txt", 10).Should().Be(17);
            pyro.SolveProblem1("../../../dummydata.txt", 2022).Should().Be(3068);
            pyro.SolveProblem1("../../../data.txt", 2022).Should().Be(3163);
        }
    }
}