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
            
            var rightOrder = true;
            
            List<string> leftArrays = CreateArrays(leftEntry, new List<string>());
            List<string> rightArrays = CreateArrays(rightEntry, new List<string>());
            // Console.WriteLine($"Concluding {leftArrays.Count},{rightArrays.Count} for round {index}");
            // foreach (var leftArray in leftArrays)
            // {
            //     Console.WriteLine(leftArray);
            // }
            // foreach (var rightArray in rightArrays)
            // {
            //     Console.WriteLine(rightArray);
            // }

            if (leftArrays.Count < rightArrays.Count)
            {
                rightOrder = false;
            }
            else if (leftArrays.Count == 1)
            {
                var leftArray = Array.ConvertAll(
                    leftArrays[0].ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                var rightArray = Array.ConvertAll(
                    rightArrays[0].ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                rightOrder = DetermineRightOrder(leftArray, rightArray);
            }
            else if (leftArrays.Count > 1)
            {
                for (var i = 0; i < leftArrays.Count; i++)
                {
                    var leftArray = Array.ConvertAll(
                            leftArrays[i].ToCharArray(), c => (int)Char.GetNumericValue(c))
                        .ToList();

                    var rightArray = Array.ConvertAll(
                            rightArrays[i].ToCharArray(), c => (int)Char.GetNumericValue(c))
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
        var charsToFilter = (new List<char>() { '[', ',', ']' });

        var numbersArray = File.ReadLines(file).ToList();
        for (var idx = 0; idx < numbersArray.Count(); idx += 3)
        {
            _numbers.Add((numbersArray[idx], numbersArray[idx + 1]));
        }
    }

    public List<string> CreateArrays(string entry, List<string> arrays)
    {
        var rightboundArray = entry.IndexOf(']');
        var leftboundArray = entry.Substring(0, rightboundArray).LastIndexOf('[') + 1;
        var firstArray = entry.Substring(leftboundArray, rightboundArray - leftboundArray);
        
        if (!string.IsNullOrEmpty(firstArray.Filter(_charsToFilter)))
            arrays.Add(firstArray.Filter(_charsToFilter));
    
        // Console.WriteLine($" Found {entry} => {firstArray} and {leftboundArray}, {rightboundArray}");
        if (leftboundArray != 0 && rightboundArray != 0)
        {
            var remainingStringLeft = entry.Substring(0, leftboundArray - 1);
            var remainingStringRight = entry.Substring(rightboundArray + 1, entry.Length - rightboundArray - 1);
            var remainingString = string.Concat(remainingStringLeft, remainingStringRight);
            
            // Console.WriteLine($" Examaning = {remainingString}");
            
            // ".*[.*[0-9]+]"
            if (Regex.IsMatch(remainingString, ".*]"))
            {
                CreateArrays(remainingString, arrays);
            }
        }

        return arrays;
    }
    
    private static bool DetermineRightOrder(List<int> leftArray, List<int> rightArray)
    {
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
                // Console.WriteLine($"Comparing {leftArray[idx]},{rightArray[idx]}");
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
            distress.CreateArrays("[1,1,3,1,1]", new List<string>()).Should().BeEquivalentTo(new[] { "11311" });
            distress.CreateArrays("[[1],[2,3,4]]", new List<string>()).Should().BeEquivalentTo(new[] { "1", "234" });
            distress.CreateArrays("[[8,7,6]]", new List<string>()).Should().BeEquivalentTo(new[] { "876" });
            distress.CreateArrays("[[4,4],4,4,4]", new List<string>()).Should().BeEquivalentTo(new[] { "44", "444" });
            distress.CreateArrays("[1,[2,[3,[4,[5,6,7]]]],8,9]", new List<string>()).Should().BeEquivalentTo(new[] { "567", "4", "3", "2", "189" });

            distress.SolveProblem1("dummydata.txt").Should().Be(13);

            // Console.WriteLine($"Solutions are {solution1} and {solution2}");
        }

    }
}

// List<int> leftRow = Array.ConvertAll(
//     numbersArray[idx].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
// List<int> rightRow = Array.ConvertAll(
//     numbersArray[idx + 1].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
// (List<int> leftRow, List<int> rightRow) test = (leftRow, rightRow);
