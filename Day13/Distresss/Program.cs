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
    private List<List<(List<int> left, List<int> right)>> _numbers = new();
    private int _indicesRightOrder = 0;

    public int SolveProblem1(string file)
    {
        Initialize(file);

        for (var index = 0; index < _numbers.Count; index++)
        {
            var numberPair = _numbers[index];
            var rightOrder = true;

            if (numberPair.left.Count == 0)
            {
                rightOrder = true;
            }
            else if (numberPair.left.Count > numberPair.right.Count)
            {
                for (var idx = 0; idx != numberPair.right.Count; idx++)
                {
                    if (numberPair.left[idx] > numberPair.right[idx])
                    {
                        // Console.WriteLine($"2 - False for {index}");
                        rightOrder = false;
                    }
                }
            }
            else if (numberPair.left.Count <= numberPair.right.Count)
            {
                for (var idx = 0; idx != numberPair.left.Count; idx++)
                {
                    if (numberPair.left[idx] > numberPair.right[idx])
                    {
                        // Console.WriteLine($"2 - False for {index}");
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
            Console.Write($" Array = ");
            foreach (var c in numbersArray[idx].Filter(charsToFilter).ToCharArray())
            {
                Console.Write(c);
            }
            Console.WriteLine();
            
            List<int> leftRow = Array.ConvertAll(
                numbersArray[idx].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            List<int> rightRow = Array.ConvertAll(
                numbersArray[idx + 1].Filter(charsToFilter).ToCharArray(), c => (int)Char.GetNumericValue(c)).ToList();
            (List<int> leftRow, List<int> rightRow) test = (leftRow, rightRow);
            
            _numbers.Add();
        }
    }
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