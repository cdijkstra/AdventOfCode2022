string[] rucksackContent = File.ReadAllLines("data.txt");

List<int> listOfCounts = new();
var matchedList = new List<char>();

var groupList = new List<char>();
var secondGroupList = new List<char>();
var thirdGroupList = new List<char>();

var uniqueForGroupList = new List<char>();

var moduloThree = 0;

foreach (var content in rucksackContent)
{
    var firstCharList = new List<char>();
    var secondCharList = new List<char>();
    
    var contentLength = content.Length;
    for (int idx = 0; idx < contentLength; idx++)
    {
        if (idx < contentLength / 2)
        {
            firstCharList.Add(content[idx]);
        }
        else
        {
            secondCharList.Add(content[idx]);
        }
        
        if (moduloThree % 3 == 0)
        {
            groupList.Add(content[idx]);
        }
        else if (moduloThree % 3 == 1)
        {
            secondGroupList.Add(content[idx]);
        }
        else
        {
            thirdGroupList.Add(content[idx]);
        }
    }

    var similarEntries = firstCharList.Where(c => secondCharList.Contains(c)).Distinct().ToList();
    matchedList.AddRange(similarEntries);
    
    if (moduloThree % 3 == 2)
    {
        // Find unique entry
        var similarEntry = groupList.Where(c => secondGroupList.Contains(c) && thirdGroupList.Contains(c))
            .Distinct().First();

        uniqueForGroupList.Add(similarEntry);
        groupList = new();
        secondGroupList = new();
        thirdGroupList = new();
    }
    
    moduloThree++;
}

var totalScore = 0;
var totalGroupScore = 0;

foreach (var match in matchedList)
{
    totalScore += char.IsUpper(match) ? match - 38 : match - 96;
}

foreach (var match in uniqueForGroupList)
{
    totalGroupScore += char.IsUpper(match) ? match - 38 : match - 96;
}

Console.WriteLine($"Result for part 1 = {totalScore} and result for part 2 = {totalGroupScore}");
