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
            var rightOrder = true;

            var numberOfArrays = numberPair.left.Count(x => x == '[');
            if (numberOfArrays == 1)
            {
                var leftArray = Array.ConvertAll(
                    numberPair.left.Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                var rightArray = Array.ConvertAll(
                    numberPair.right.Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                rightOrder = DetermineRightOrder(leftArray, rightOrder, rightArray, numberPair);
            }
            else if (numberOfArrays > 1)
            {
                var leftEntry = numberPair.left;
                var rightEntry = numberPair.right;
                List<string> leftArrays = CreateArrays(leftEntry, new());
                List<string> rightArrays = CreateArrays(rightEntry, new());
                
                Console.WriteLine("Left ---- ");
                leftArrays.ForEach(x => Console.WriteLine(x));
                
                Console.WriteLine("Right ---- ");
                rightArrays.ForEach(x => Console.WriteLine(x));

                if (leftArrays.Count != rightArrays.Count)
                {
                    Console.WriteLine($"Left = {leftArrays.Count} and right = {rightArrays.Count}");
                    throw new Exception("WHUUT");
                }
                
                for (var i = 0; i < leftArrays.Count; i++)
                {
                    var leftArray = Array.ConvertAll(
                            leftArrays[i].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c))
                        .ToList();

                    var rightArray = Array.ConvertAll(
                            rightArrays[i].Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c))
                        .ToList();

                    if (!DetermineRightOrder(leftArray, rightOrder, rightArray, numberPair))
                    {
                        rightOrder = false;
                    }
                }
            }

            Console.WriteLine($"Checking for {index} - CONCLUSION {rightOrder}");
            if (rightOrder)
            {
                _indicesRightOrder += index;
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

    private List<string> CreateArrays(string entry, List<string> arrays)
    {
        var rightboundArray = entry.IndexOf(']');
        var leftboundArray = entry.Substring(0, rightboundArray).LastIndexOf('[') + 1;
        var firstArray = entry.Substring(leftboundArray, rightboundArray - leftboundArray);
        
        arrays.Add(firstArray.Filter(_charsToFilter));

        var remainingStringLeft = entry.Substring(0, leftboundArray);
        var remainingStringRight = entry.Substring(rightboundArray + 1, entry.Length - rightboundArray - 1);
        var remainingString = string.Concat(remainingStringLeft, remainingStringRight);
        if (remainingString.Contains(']'))
        {
            CreateArrays(remainingStringRight, arrays);
        }

        return arrays;
    }
    
    private static bool DetermineRightOrder(List<int> leftArray, bool rightOrder, List<int> rightArray,
        (string left, string right) numberPair)
    {
        if (leftArray.Count == 0)
        {
            rightOrder = true;
        }
        else if (leftArray.Count > rightArray.Count)
        {
            for (var idx = 0; idx != rightArray.Count; idx++)
            {
                if (numberPair.left[idx] > numberPair.right[idx])
                {
                    rightOrder = false;
                }
            }
        }
        else if (leftArray.Count <= rightArray.Count)
        {
            for (var idx = 0; idx != leftArray.Count; idx++)
            {
                if (numberPair.left[idx] > numberPair.right[idx])
                {
                    rightOrder = false;
                }
            }
        }

        return rightOrder;
    }


    internal static class Program
    {
        static void Main(string[] args)
        {
            var treetop = new Distress();
            treetop.SolveProblem1("dummydata.txt").Should().Be(13);

            // Console.WriteLine($"Solutions are {solution1} and {solution2}");
        }

    }
}

// List<int> leftRow = Array.ConvertAll(
//     numbersArray[idx].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
// List<int> rightRow = Array.ConvertAll(
//     numbersArray[idx + 1].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
// (List<int> leftRow, List<int> rightRow) test = (leftRow, rightRow);
