string[] fullRucksackContent = File.ReadAllLines("data.txt");

var matchedList = new List<char>();

var rucksackContent = new List<char>();
var contentOfRucksackGroup = new List<List<char>>(); // Filled with three rucksackContents; then emptied again
var uniqueForGroupList = new List<char>();

for (var rucksackIndex = 0; rucksackIndex < fullRucksackContent.Length; rucksackIndex++)
{
    var content = fullRucksackContent[rucksackIndex];
    rucksackContent.AddRange(content);
    var contentLength = content.Length;

    var firstCharList = new List<char>();
    var secondCharList = new List<char>();
    firstCharList.AddRange(content.Substring(0, contentLength / 2).ToCharArray().ToList());
    secondCharList.AddRange(content.Substring(contentLength / 2, contentLength / 2).ToCharArray().ToList());

    var similarEntries = firstCharList.Where(c => secondCharList.Contains(c)).Distinct().ToList();
    matchedList.AddRange(similarEntries);
    
    contentOfRucksackGroup.Add(rucksackContent);
    
    if (rucksackIndex % 3 == 2)
    {
        // Find unique entry
        var similarEntry = contentOfRucksackGroup[0]
            .Where(c => 
                contentOfRucksackGroup[1].Contains(c) && 
                contentOfRucksackGroup[2].Contains(c)
                )
            .Distinct()
            .First();

        uniqueForGroupList.Add(similarEntry);
        contentOfRucksackGroup.Clear();
    }

    rucksackContent.Clear(); // Reuse rucksackContent
}

var totalScore = matchedList.Sum(match => AddScoreForChar(match));
var totalGroupScore = uniqueForGroupList.Sum(match => AddScoreForChar(match));

Console.WriteLine($"Result for part 1 = {totalScore} and result for part 2 = {totalGroupScore}");

int AddScoreForChar(char match1)
{
    return char.IsUpper(match1) ? match1 - 38 : match1 - 96;
}