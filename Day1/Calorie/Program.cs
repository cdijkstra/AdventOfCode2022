string[] lines = File.ReadAllLines("testinput.txt");
var lineEntries = lines.Length;

List<int> listOfCounts = new();
var count = 0;
foreach (var calory in lines)
{
    if (!String.IsNullOrEmpty(calory))
    {
        count += int.Parse(calory);
    }
    else
    {
        listOfCounts.Add(count);
        count = 0;
    }
}
listOfCounts.Add(count);

var answer = listOfCounts.OrderByDescending(x => x).Take(3).Sum();
Console.WriteLine(answer);