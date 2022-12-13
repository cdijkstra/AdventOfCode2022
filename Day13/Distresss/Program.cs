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
            else if (numberOfArrays == 2)
            {
                var modifyEntry = numberPair.left;
                var rightboundArray = modifyEntry.IndexOf(']');
                var leftboundArray = modifyEntry.Substring(0, rightboundArray).LastIndexOf('[');
                var firstArray = modifyEntry.Substring(leftboundArray, rightboundArray - leftboundArray);
                
                Console.WriteLine(firstArray);

                var leftArray = Array.ConvertAll(
                    numberPair.left.Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                var rightArray = Array.ConvertAll(
                    numberPair.right.Filter(_charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();

                rightOrder = DetermineRightOrder(leftArray, rightOrder, rightArray, numberPair);
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
            // List<int> leftRow = Array.ConvertAll(
            //     numbersArray[idx].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            // List<int> rightRow = Array.ConvertAll(
            //     numbersArray[idx + 1].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            // (List<int> leftRow, List<int> rightRow) test = (leftRow, rightRow);

            _numbers.Add((numbersArray[idx], numbersArray[idx + 1]));
        }
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
                    // Console.WriteLine($"2 - False for {index}");
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