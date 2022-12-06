using System.Collections;
using FluentAssertions;

namespace Supply;

class Supply
{
    private List<LinkedList<char>> _supplies = new();
    private string _message;
    
    public string Solution1(string file)
    {
        _supplies.Clear();
        _message = "";
        var lines = SetUpBlocks(file, out var amountOfStacks);

        var findLine = lines.First(x => x.StartsWith("move"));
        var indexOfLoop = lines.ToList().IndexOf(findLine);

        for (var idx = indexOfLoop; idx < lines.Count(); idx++)
        {
            MoveBlocksOneByOne(lines, idx);
        }
        
        // Construct message
        foreach (var stackNumber in Enumerable.Range(0, amountOfStacks))
        {
            var newLetter = _supplies[stackNumber].First.Value;
            _message += newLetter;
        }

        return _message;
    }
    
    public string Solution2(string file)
    {
        _supplies.Clear();
        _message = "";
        var lines = SetUpBlocks(file, out var amountOfStacks);

        var findLine = lines.First(x => x.StartsWith("move"));
        var indexOfLoop = lines.ToList().IndexOf(findLine);

        for (var idx = indexOfLoop; idx < lines.Count(); idx++)
        {
            MoveBlocksAtOnce(lines, idx);
        }
        
        // Construct message
        foreach (var stackNumber in Enumerable.Range(0, amountOfStacks))
        {
            var newLetter = _supplies[stackNumber].First.Value;
            _message += newLetter;
        }

        return _message;
    }

    private List<string> SetUpBlocks(string file, out int amountOfStacks)
    {
        _message = "";
        var lines = File.ReadLines(file).ToList();
        var amountOfChars = lines.First().ToCharArray().Length;
        amountOfStacks = (amountOfChars + 1) / 4;
        foreach (var stackNumber in Enumerable.Range(0, amountOfStacks))
        {
            _supplies.Add(new LinkedList<char>());
        }

        bool stop = false;
        var idy = 0;
        while (stop == false)
        {
            var stackNumber = 0;
            for (var idx = 1; idx < amountOfChars; idx += 4)
            {
                var foundChar = lines.ToList()[idy][idx];
                if (!char.IsWhiteSpace(foundChar))
                {
                    if (!char.IsNumber(foundChar))
                    {
                        _supplies[stackNumber].AddLast(foundChar);
                    }
                    else
                    {
                        stop = true;
                    }
                }

                stackNumber++;
            }

            idy++;
        }

        return lines;
    }

    private void MoveBlocksOneByOne(List<string> lines, int idx)
    {
        var lineContent = lines[idx];
        var amountToMove = int.Parse(lineContent.Split()[1]);
        var moveFrom = int.Parse(lineContent.Split()[3]) - 1;
        var moveTo = int.Parse(lineContent.Split()[5]) - 1;

        foreach (var repeats in Enumerable.Range(0, amountToMove))
        {
            var charToMove = _supplies[moveFrom].First.Value;
            _supplies[moveTo].AddFirst(charToMove);
            _supplies[moveFrom].RemoveFirst();
        }
    }
    
    private void MoveBlocksAtOnce(List<string> lines, int idx)
    {
        var lineContent = lines[idx];
        var amountToMove = int.Parse(lineContent.Split()[1]);
        var moveFrom = int.Parse(lineContent.Split()[3]) - 1;
        var moveTo = int.Parse(lineContent.Split()[5]) - 1;

        Stack<char> tempStack = new();
        foreach (var repeats in Enumerable.Range(0, amountToMove))
        {
            var charToMove = _supplies[moveFrom].First.Value;
            tempStack.Push(charToMove);
            _supplies[moveFrom].RemoveFirst();
        }

        var amountOfRepeats = tempStack.Count;
        foreach (var pops in Enumerable.Range(0, amountOfRepeats))
        {
            var charToMove = tempStack.Peek();
            _supplies[moveTo].AddFirst(charToMove);
            tempStack.Pop();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var supply = new Supply();
        supply.Solution1("dummydata.txt").Should().Be("CMZ");
        supply.Solution2("dummydata.txt").Should().Be("MCD");
        
        var firstResult = supply.Solution1("data.txt");
        var secondResult = supply.Solution2("data.txt");
        Console.WriteLine($"Results are {firstResult} and {secondResult}");
    }
}