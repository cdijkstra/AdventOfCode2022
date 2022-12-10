using FluentAssertions;

namespace Tube;

class Tube
{
    private List<(int cycle, int signal)> _signals = new();
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
                
                var addSecondPixel = Math.Abs(currentCycle - currentSignal) <= 1 ? '#' : '.';
                Console.WriteLine($"Adding {addSignal} ({addSecondPixel}) to {currentSignal} at {currentCycle}");
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
}

class Program
{
    static void Main(string[] args)
    {
        var tube = new Tube();
        tube.Solution1("dummydata.txt").Should().Be(13140);
        // tube.Solution2("dummydata.txt").Should().Be("MCD");
        //
        // var firstResult = tube.Solution1("data.txt");
        // var secondResult = supply.Solution2("data.txt");
        // Console.WriteLine($"Results are {firstResult}");
    }
}