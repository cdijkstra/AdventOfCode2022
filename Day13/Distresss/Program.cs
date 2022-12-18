using System.Text.Json;
using FluentAssertions;

namespace Distress;

public class Distress
{
    private List<(string left, string right)> _numbers = new();
    private int _indicesRightOrder = 0;

    public int SolveProblem1(string file)
    {
        Initialize(file);
        for (var index = 0; index < _numbers.Count; index++)
        {
            var numberSet = _numbers[index];
            var left = JsonSerializer.Deserialize(numberSet.left, PacketValueContext.Default.PacketValue);
            var right = JsonSerializer.Deserialize(numberSet.right, PacketValueContext.Default.PacketValue);

            // Check if its in the right order
            if (left.CompareTo(right) < 0)
            {
                Console.WriteLine($"Concluded TRUE for {index + 1}");
                _indicesRightOrder += index;
            }
            else
            {
                Console.WriteLine($"Concluded FALSE for {index + 1}");
            }
        }

        return _indicesRightOrder;
    }

    private void Initialize(string file)
    {
        _numbers.Clear();
        _indicesRightOrder = 0;

        var numbersArray = File.ReadLines(file).ToList();
        for (var idx = 0; idx < numbersArray.Count(); idx += 3)
        {
            _numbers.Add((numbersArray[idx], numbersArray[idx + 1]));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var distress = new Distress();
            distress.SolveProblem1("dummydata.txt").Should().Be(13);
        }
    }
}