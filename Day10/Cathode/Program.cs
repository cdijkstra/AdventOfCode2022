using System.Reflection.Metadata;
using FluentAssertions;

namespace Tube;

class Tube
{
    private List<(int cycle, int signal)> _signals = new();
    private List<char> _image = new();
    
    public int Solution1(string file)
    {
        var currentSignal = 1;
        var currentCycle = 1;
        _signals.Clear();
        _signals.Add((0, 0)); // So we can use index 1 instead of 0 ;)

        var instructions = File.ReadLines(file);
        foreach (var instruction in instructions)
        {
            (int cycle, int signal) firstEntry = (currentCycle, currentSignal);
            _signals.Add(firstEntry);

            if (instruction.StartsWith("addx"))
            {
                var addSignal = int.Parse(instruction.Split()[1]);
                (int cycle, int signal) secondEntry = (++currentCycle, currentSignal += addSignal);
                _signals.Add(secondEntry);
            }

            currentCycle++;
        }

        var answer = 0;
        for (var idx = 20; idx <= 240; idx += 40)
        {
            var newEntry = (_signals[idx - 1].signal * _signals[idx].cycle);
            answer += newEntry;
        }

        return answer;
    }
    
    public List<char> Solution2(string file)
    {
        var currentSignal = 1;
        var currentCycle = 1;
        _image = new();
        
        _signals.Clear();
        _signals.Add((0, 0)); // So we can use index 1 instead of 0 ;)

        var instructions = File.ReadLines(file);
        foreach (var instruction in instructions)
        {
            (int cycle, int signal) firstEntry = (currentCycle, currentSignal);
            _signals.Add(firstEntry);

            var addCharToImage = Math.Abs(currentCycle % 40 - currentSignal - 1) <= 1 ? '#' : '.';
            _image.Add(addCharToImage);

            if (instruction.StartsWith("addx"))
            {
                ++currentCycle;
                var addToImage = Math.Abs(currentCycle % 40 - currentSignal - 1) <= 1 ? '#' : '.';
                var addSignal = int.Parse(instruction.Split()[1]);
                _image.Add(addToImage);
                
                (int cycle, int signal) secondEntry = (currentCycle, currentSignal += addSignal);
                _signals.Add(secondEntry);
            }

            currentCycle++;
        }

        return _image;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var tube = new Tube();
        tube.Solution1("dummydata.txt").Should().Be(13140);
        var expectedResult = "##..##..##..##..##..##..##..##..##..##..###...###...###...###...###...###...###.####....####....####....####....####....#####.....#####.....#####.....#####.....######......######......######......###.#######.......#######.......#######.....".ToCharArray().ToList();
        tube.Solution2("dummydata.txt").Should().BeEquivalentTo(expectedResult);
        
        var answer1 = tube.Solution1("data.txt");
        var answer2 = tube.Solution2("data.txt");
        Console.WriteLine($"First answer = {answer1} and the second answer is below;");
        for (var idx = 0; idx != answer2.Count; idx++)
        {
            Console.Write(answer2[idx]);
            if (idx >= 30 && (idx + 1) % 40 == 0)
            {
                Console.WriteLine("");
            }
        }
    }
}