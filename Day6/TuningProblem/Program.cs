using FluentAssertions;

namespace Tuning;

public class Tuning
{
    public int SolveProblems(string file, bool secondExercise)
    {
        var charLength = secondExercise ? 14 : 4;
        var signal = File.ReadAllText(file);
        for (var idx = charLength; idx < signal.Length; idx++)
        {
            List<char> previousEntries = signal.Substring(idx - charLength, charLength).ToList();
            if (previousEntries.Distinct().Count() == charLength)
            {
                return idx;
            }
        }

        return 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var tuning = new Tuning();
        tuning.SolveProblems("dummydata.txt", false).Should().Be(7);
        tuning.SolveProblems("dummydata.txt", true).Should().Be(19);
        // tuning.FindPartiallyOverlappingSections("dummydata.txt").Should().Be(4);

        var firstResult = tuning.SolveProblems("data.txt", false);
        var secondResult = tuning.SolveProblems("data.txt", true);
        Console.WriteLine($"{firstResult} and {secondResult}");
    }
}