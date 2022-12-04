using FluentAssertions;

namespace Camp;

class Camp
{
    private int _completelyOverlappingCount;
    private int _partiallyOverlappingCount;

    public int FindCompletelyOverlappingSections(string file)
    {
        _completelyOverlappingCount = 0;
        var sections = File.ReadAllLines(file);
        foreach (var section in sections)
        {
            FillCampLists(section, out List<int> firstSection, out List<int> secondSection);
            if (firstSection.All(x => secondSection.Contains(x)) || secondSection.All(x => firstSection.Contains(x)))
            {
                _completelyOverlappingCount++;
            }
        }

        return _completelyOverlappingCount;
    }

    public int FindPartiallyOverlappingSections(string file)
    {
        _partiallyOverlappingCount = 0;
        var sections = File.ReadAllLines(file);
        foreach (var section in sections)
        {
            FillCampLists(section, out List<int> firstSection, out List<int> secondSection);
            if (firstSection.Any(x => secondSection.Contains(x)) || secondSection.Any(x => firstSection.Contains(x)))
            {
                _partiallyOverlappingCount++;
            }
        }

        return _partiallyOverlappingCount;
    }
    
    private void FillCampLists(string section, out List<int> firstSection, out List<int> secondSection)
    {
        firstSection = new();
        secondSection = new();
        var twoSections = section.Split(',');

        var rangeNumbersFirstCamp = twoSections[0].Split('-');
        var numberOfEntriesFirstCamp = int.Parse(rangeNumbersFirstCamp[1]) - int.Parse(rangeNumbersFirstCamp[0]) + 1;

        foreach (int value in Enumerable.Range(int.Parse(rangeNumbersFirstCamp[0]), numberOfEntriesFirstCamp))
        {
            firstSection.Add(value);
        }

        var rangeNumbersSecondCamp = twoSections[1].Split('-');
        var numberOfEntriesSecondCamp = int.Parse(rangeNumbersSecondCamp[1]) - int.Parse(rangeNumbersSecondCamp[0]) + 1;
        foreach (int value in Enumerable.Range(int.Parse(rangeNumbersSecondCamp[0]), numberOfEntriesSecondCamp))
        {
            secondSection.Add(value);
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        var camp = new Camp();
        camp.FindCompletelyOverlappingSections("dummydata.txt").Should().Be(2);
        camp.FindPartiallyOverlappingSections("dummydata.txt").Should().Be(4);
        
        var firstResult = camp.FindCompletelyOverlappingSections("data.txt");
        var secondResult = camp.FindPartiallyOverlappingSections("data.txt");
        Console.WriteLine($"Results are {firstResult} and {secondResult}");
    }
}