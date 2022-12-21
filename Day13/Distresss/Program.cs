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
                Console.WriteLine($"Concluded TRUE for {index + 1} with {left.CompareTo(right)}");
                _indicesRightOrder += (index + 1);
            }
            else
            {
                Console.WriteLine($"Concluded FALSE for {index + 1} with {left.CompareTo(right)}");
            }
        }

        return _indicesRightOrder;
    }

    public int SolveProblem2(string file)
    {
        Initialize(file);
        // Start with the two divider packets
        var divider1 = new PacketValue(new[] { new PacketValue(new[] { new PacketValue(2) }) }); // [[2]]
        var divider2 = new PacketValue(new[] { new PacketValue(new[] { new PacketValue(6) }) }); // [[6]]
        var packets = new List<PacketValue> { divider1, divider2 };
        
        // Add each packet from input
        foreach (var numberSet in _numbers)
        {
            var left = JsonSerializer.Deserialize(numberSet.left, PacketValueContext.Default.PacketValue);
            var right = JsonSerializer.Deserialize(numberSet.right, PacketValueContext.Default.PacketValue);
            packets.Add(left);
            packets.Add(right);
        }
        
        packets.Sort();
        // Find divider packets
        var divider1Index = packets.IndexOf(divider1) + 1;
        var divider2Index = packets.IndexOf(divider2) + 1;
        var decoderKey = divider1Index * divider2Index;
        return decoderKey;
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
            distress.SolveProblem2("dummydata.txt").Should().Be(140);
            var answer1 = distress.SolveProblem1("data.txt");
            Console.WriteLine(answer1);
        }
    }
}