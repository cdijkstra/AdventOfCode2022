using System.Text.RegularExpressions;
using FluentAssertions;

namespace Distress;

public static class Extensions
{
    public static string Filter(this string str, List<char> charsToRemove)
    {
        foreach (char c in charsToRemove) {
            str = str.Replace(c.ToString(), String.Empty);
        }
 
        return str;
    }
    
    public static int GetNthIndex(this string s, char t, int n)
    {
        int count = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == t)
            {
                count++;
                if (count == n)
                {
                    return i;
                }
            }
        }
        return -1;
    }
}

public class Distress
{
    private List<(string left, string right)> _numbers = new();
    private int _indicesRightOrder = 0;
    private readonly List<char> _charsToFilter = new() { '[', ',', ']' };

    public int SolveProblem1(string file)
    {
        Initialize(file);

        for (var index = 0; index < _numbers.Count; index++)
        {
            var numberPair = _numbers[index];
            var leftEntry = numberPair.left;
            var rightEntry = numberPair.right;
            var leftNumberOfArrays = leftEntry.Substring(0, leftEntry.IndexOf(']')).Count(x => x == '[');
            var rightNumberOfArrays = rightEntry.Substring(0, rightEntry.IndexOf(']')).Count(x => x == '[');

            var rightOrder = true;
            
            List<string> leftArrays = CreateArrays(leftEntry, leftNumberOfArrays, new List<string>());
            List<string> rightArrays = CreateArrays(rightEntry, rightNumberOfArrays, new List<string>());

            if (leftArrays.Count > rightArrays.Count)
            {
                rightOrder = false;
            }
            else if (leftArrays.Count == 1)
            {
                var leftArray = Array.ConvertAll(
                    leftArrays[0].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                var rightArray = Array.ConvertAll(
                    rightArrays[0].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                rightOrder = DetermineRightOrder(leftArray, rightArray);
            }
            else if (leftArrays.Count > 1)
            {
                for (var i = 0; i < leftArrays.Count; i++)
                {
                    var leftArray = Array.ConvertAll(
                            leftArrays[i].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c))
                        .ToList();

                    var rightArray = Array.ConvertAll(
                            rightArrays[i].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c))
                        .ToList();

                    if (DetermineRightOrder(leftArray, rightArray) == false)
                    {
                        rightOrder = false;
                    }
                }
            }
            
            Console.WriteLine($"Concluding {rightOrder} for round {index + 1}");
            
            if (rightOrder)
            {
                _indicesRightOrder += (index + 1);
            }
        }

        return _indicesRightOrder;
    }

    private void Initialize(string file)
    {
        _numbers.Clear();
        _indicesRightOrder = 0;
        var charsToFilter = (new List<char>() { '[', ',', ']' });

        var numbersArray = File.ReadLines(file).ToList();
        for (var idx = 0; idx < numbersArray.Count(); idx += 3)
        {
            _numbers.Add((numbersArray[idx], numbersArray[idx + 1]));
        }
    }

    public List<string> CreateArrays(string entry, int numberOfArrays, List<string> arrays, int depth = 1)
    {
        var rightIndex = entry.IndexOf(']');
        var leftIndex = entry.Substring(0, rightIndex).LastIndexOf('[');
        
        var length = rightIndex + 1 - leftIndex;
        // Console.WriteLine($"Using {entry}, {leftIndex}, {rightIndex}");
        var newArray = entry.Substring(leftIndex, length);
        
        arrays.Add(newArray.Filter( new List<char>() { ',' }));
        
        // Console.WriteLine($"Found {entry} and {newArray.Filter( new List<char>() { ',' })}");
        if (depth < numberOfArrays)
        {
            var newEntry = entry.Replace(newArray, "");
            // Console.WriteLine($"NewEntry = {newEntry} with {newArray} removed");
            CreateArrays(newEntry, numberOfArrays, arrays, ++depth);
        }

        return arrays;
    }
    
    private static bool DetermineRightOrder(List<int> leftArray, List<int> rightArray)
    {
        // foreach (var left in leftArray)
        // {
        //     Console.WriteLine($"LEFT-- {left}");
        // }
        //         
        // foreach (var right in rightArray)
        // {
        //     Console.WriteLine($"RIGHT-- {right}");
        // }

        bool rightOrder = true;
        if (leftArray.Count == 0)
        {
            rightOrder = true;
        }
        else if (leftArray.Count > rightArray.Count)
        {
            for (var idx = 0; idx != rightArray.Count; idx++)
            {
                if (leftArray[idx] > rightArray[idx])
                {
                    rightOrder = false;
                }
            }
        }
        else if (leftArray.Count <= rightArray.Count)
        {
            for (var idx = 0; idx != leftArray.Count; idx++)
            {
                if (leftArray[idx] > rightArray[idx])
                {
                    rightOrder = false;
                }
            }
        }

        return rightOrder;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var distress = new Distress();
            distress.CreateArrays("[1,1,3,1,1]", 1, new List<string>()).Should().BeEquivalentTo(new[] { "[11311]" });
            distress.CreateArrays("[[1],[2,3,4]]", 2, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[1]", "[234]" });
            distress.CreateArrays("[[1],4]", 2, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[1]", "[4]" });
            distress.CreateArrays("[[8,7,6]]", 2, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[876]", "[]" });
            distress.CreateArrays("[[4,4],4,4,4]", 2, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[44]", "[444]" });
            distress.CreateArrays("[]", 1, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[]" });
            distress.CreateArrays("[3]", 1, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[3]" });
            distress.CreateArrays("[1,[2,[3,[4,[5,6,7]]]],8,9]", 5, new List<string>()).Should().BeEquivalentTo(new[]
                { "[567]", "[4]", "[3]", "[2]", "[189]" });
            distress.CreateArrays("[[[]]]", 3, new List<string>()).Should()
                .BeEquivalentTo(new[] { "[]", "[]", "[]" });
            distress.CreateArrays("[[]]", 2, new List<string>()).Should().BeEquivalentTo(new[] { "[]", "[]" });
            
            distress.SolveProblem1("dummydata.txt").Should().Be(13);

            var solution1 = distress.SolveProblem1("data.txt");
            Console.WriteLine($"Solutions are {solution1}");
        }

    }
}