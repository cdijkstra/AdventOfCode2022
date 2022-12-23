using FluentAssertions;

namespace Space;

public class Pyroclastic
{
    private List<List<char>> _cave = new();
    private List<List<char>> _rocks = new();
    private List<char> _jetPattern = new();
    private int _currentIndex = 0;

    public int SolveProblem1(string file, int row)
    {
        Initialize(file);

        foreach (var rock in _rocks)
        {
            PlaceRock(rock);
            ApplyJetAndFall();
        }

        return 1;
    }

    private void ApplyJetAndFall()
    {
        // Fall down while possible
        bool continueFalling = true;
        while (continueFalling)
        {
            // Apply jet and fall
            ApplyJet();
            continueFalling = RockFalling();
            
            for (var idx = 0; idx != _cave.Count(); idx++)
            {
                _cave[idx].ForEach(x => Console.Write(x));
                Console.WriteLine("");
            }
        }
    }

    private void ConvertToNotFallingRock()
    {
        Console.WriteLine("Done");
        for (var idx = 0; idx != _cave.Count(); idx++)
        {
            // Find first match
            if (!_cave[idx].Contains('@')) continue;
            List<char> newEntry = new();
            foreach (var ch in _cave[idx])
            {
                newEntry.Add(ch == '@' ? '#' : ch);
            }

            var updatedEntry = newEntry;

            _cave[idx] = updatedEntry;
        }
    }

    private List<int> FindLowestXPositionsRock()
    {
        var matches = new List<char>()
        {
            '@'
        };
        
        List<int> match = new();
        for (var idx = 0; idx != _cave.Count(); idx++)
        {
            if (!matches.Any(match => _cave[idx].Contains(match))) continue;
            // First occurrence is lowest position of rock
            
            match = Enumerable.Range(0, _cave[idx].Count)
                .Where(i => matches.Contains(_cave[idx][i]))
                .ToList();

            Console.WriteLine($"MATCH ROCK at {idx}: ");
            match.ForEach(x => Console.Write(x));
            Console.WriteLine("DONE");
            
            return match;
        }

        return match;
    }
    
    private List<int> FindXPositionsFloor()
    {
        var matches = new List<char>()
        {
            '-', '#'
        };

        List<int> match = new();
        for (var idx = _cave.Count() - 1; idx >= 0; idx--)
        {
            if (!matches.Any(match => _cave[idx].Contains(match) && !_cave[idx].Contains('@'))) continue;
            // First occurrence is lowest position of rock
            
            match = Enumerable.Range(0, _cave[idx].Count)
                .Where(i => matches.Contains(_cave[idx][i]))
                .ToList();
            
            Console.WriteLine("MATCH FLOOR: ");
            match.ForEach(x => Console.Write(x));
            Console.WriteLine("DONE");
            
            break;
        }

        return match;
    }
    
    private int FindLowestRockYPosition()
    {
        int lowestHeightRock = 0;
        for (var idx = 0; idx != _cave.Count(); idx++)
        {
            if (!_cave[idx].Contains('@')) continue;
            // First occurrence is lowest position of rock
            lowestHeightRock = idx;
            break;
        }

        return lowestHeightRock;
    }

    private void ApplyJet()
    {
        // Find most left and most right entry of '@'
        List<int> leftIndices = new();
        List<int> rightIndices = new();
        foreach (var entry in _cave.Where(entry => entry.Contains('@')))
        {
            leftIndices.Add(entry.IndexOf('@'));
            rightIndices.Add(entry.LastIndexOf('@'));
        }

        var left = leftIndices.Min();
        var right = rightIndices.Max();
        
        Console.WriteLine($"Move = {_jetPattern[_currentIndex]} at {_currentIndex}, {left}, {right}");
        
        if (_jetPattern[_currentIndex] == '<' && left != 1)
        {
            bool applyMove = true;
            for (var idy = 0; idy != _cave.Count(); idy++)
            {
                applyMove = _cave[idy].IndexOf('@') - 1 != '#';
            }

            if (applyMove)
            {
                foreach (var entry in _cave.Where(entry => entry.Contains('@')))
                {
                    if (entry.IndexOf('@') - 1 == '#') break;
                    entry[entry.IndexOf('@') - 1] = '@';
                    entry[entry.LastIndexOf('@')] = '.';
                }
            }
        }
        else if (_jetPattern[_currentIndex] == '>' && right != 7)
        {
            foreach (var entry in _cave.Where(entry => entry.Contains('@')))
            {
                if (entry[entry.LastIndexOf('@') + 1] == '#') break;
                entry[entry.LastIndexOf('@') + 1] = '@';
                entry[entry.IndexOf('@')] = '.';
            }
        }

        _currentIndex = (_currentIndex + 1) % _jetPattern.Count();
    }

    private bool RockFalling()
    {
        Console.WriteLine("FALLDOWN");
        int lowestHeightRock = FindLowestRockYPosition();
        if (lowestHeightRock > FindBottomYPosition() + 1)
        {
            Console.WriteLine($"Bla {lowestHeightRock} and {FindBottomYPosition()}");
            _cave.RemoveAt(lowestHeightRock - 1);
            return true;
        }
        
        if (!FindLowestXPositionsRock().Intersect(FindXPositionsFloor()).Any())
        {
            // Fall down once more, but now more complex since it falls on floor
            Console.WriteLine("SPECIAL: CASE");
            var lowestRock = _cave[FindLowestRockYPosition()];
            var newBottom = new List<char>();
            for (var idx = 0; idx != _cave[FindBottomYPosition()].Count(); idx++)
            {
                newBottom.Add(lowestRock[idx] == '@' ? '@' : _cave[FindBottomYPosition()][idx]);
            }
            
            Console.WriteLine($"{FindBottomYPosition()} and {FindLowestRockYPosition()}");
            for (var idx = 0; idx != _cave.Count(); idx++)
            {
                _cave[idx].ForEach(x => Console.Write(x));
                Console.WriteLine("");
            }
            
            _cave.RemoveAt(FindLowestRockYPosition());
            for (var idx = 0; idx != _cave.Count(); idx++)
            {
                _cave[idx].ForEach(x => Console.Write(x));
                Console.WriteLine("");
            }
            
            _cave[FindBottomYPosition()] = newBottom;
            for (var idx = 0; idx != _cave.Count(); idx++)
            {
                _cave[idx].ForEach(x => Console.Write(x));
                Console.WriteLine("");
            }
            
            return true;
        }

        ConvertToNotFallingRock();
        return false;
    }

    private void PlaceRock(List<char> rock)
    {
        // Find bottom
        var bottom = FindBottomYPosition();
        // Find height rock
        var heightRock = rock.Count() / 4;
        var highestHeightInCave = bottom + 4 + heightRock;

        if (_cave.Count() < highestHeightInCave)
        {
            // Add new entries to cave
            var amountOfNewEntries = highestHeightInCave - _cave.Count();
            foreach (var repeats in Enumerable.Range(0, amountOfNewEntries))
            {
                Console.WriteLine($"Adding entry {repeats} and {highestHeightInCave}");
                List<char> newEntries = new()
                {
                    '|', '.', '.', '.', '.', '.', '.', '.', '|'
                };
                _cave.Add(newEntries);
            }
        }

        for (var rockPart = 0; rockPart <= heightRock - 1; rockPart++)
        {
            Console.WriteLine($"Adding rock with {heightRock}, {bottom}, {rockPart}, {_cave.Count()}");
            List<char> newEntry = new() { '|', '.', '.' };
            newEntry.AddRange(rock.GetRange(rockPart * 4, 4));
            newEntry.AddRange(new List<char>() { '.', '|' });
            _cave[bottom + 4 + rockPart] = newEntry;
        }
    }

    private int FindBottomYPosition()
    {
        for (var idx = _cave.Count() - 1; idx > 0; idx--)
        {
            List<char> rocks = new List<char>() { '#', '-' };
            if (_cave[idx].GetRange(1, 5).Any(rock => rocks.Contains(rock)))
            {
                return idx;
            }
        }

        return 0;
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
        var input = File.ReadLines("simpleinput.txt");

        for (var idx = 0; idx != input.Count(); idx++)
        {
            if (idx == input.Count() - 1)
            {
                var entry = input.ElementAt(idx).ToCharArray().ToList();
                if (entry.Count() < 4)
                {
                    entry.AddRange(Enumerable.Repeat('.', 4 - entry.Count()).ToList());
                }
                newRockEntry.AddRange(entry);
                _rocks.Add(newRockEntry);
            }
            else if (!string.IsNullOrEmpty(input.ElementAt(idx)))
            {
                var entry = input.ElementAt(idx).ToCharArray().ToList();
                if (entry.Count() < 4)
                {
                    entry.AddRange(Enumerable.Repeat('.', 4 - entry.Count()).ToList());
                }
                newRockEntry.AddRange(entry);
            }
            else
            {
                _rocks.Add(newRockEntry);
                newRockEntry = new();
            }
        }
        
        var bottom = new List<char>()
        {
            '+', '-', '-', '-', '-', '-', '-', '-', '+'
        };
        _cave.Add(bottom);
    }

    internal static class Program
    {
        static void Main(string[] args)
        {
            var pyro = new Pyroclastic();
            pyro.SolveProblem1("dummydata.txt", 10).Should().Be(26);
            // beacon.SolveProblem2("dummydata.txt", 20).Should().Be(56000011);
            // var answer1 = beacon.SolveProblem1("data.txt", 2000000);
            // var answer2 = beacon.SolveProblem2("data.txt", 4000000);
            // Console.WriteLine($"{answer1} and {answer2}");
        }
    }
}