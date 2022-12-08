using System.Text.RegularExpressions;
using FluentAssertions;

namespace Space;

public class Directory
{
    public string Name { get; set; }
    
    public List<Directory> Children { get; set; }
    
    public int Size { get; set; }

    public int GetTotalSize()
    {
        return Size + Children.Sum(child => child.GetTotalSize());
    }
}

public class Space
{
    private List<Directory> _directories = new();
    private string _currentDirectory = "/";

    public int SolveProblem1(string file)
    {
        ParseInfo(file);
        List<int> totalSizes = _directories.Select(dir => dir.GetTotalSize()).ToList();
        return totalSizes.Where(x => x < 100000).Sum();
    }

    public int SolveProblem2(string file)
    {
        ParseInfo(file);
        var totalSize = _directories.Single(x => x.Name == "/").GetTotalSize();
        var sizeToRemove = totalSize - 40000000;
        var sizesToRemove = _directories.Select(x => x.GetTotalSize()).ToList();
        var dirToRemove = sizesToRemove.Where(x => x > sizeToRemove).Min();
        return dirToRemove;
    }

    private void ParseInfo(string file)
    {
        Initialize(file);
        foreach (var command in File.ReadLines(file).Where(x => !x.StartsWith("$ ls")))
        {
            processCdInput(command);
            var currentDir = _directories.SingleOrDefault(x => x.Name == _currentDirectory);
            processLsInput(command, currentDir);
        }
    }

    private void Initialize(string file)
    {
        _directories.Clear();
        _currentDirectory = "/";
        
        var directory = new Directory()
        {
            Name = "/",
            Children = new(),
            Size = 0
        };
        _directories.Add(directory);
    }

    private void processLsInput(string command, Directory? currentDir)
    {
        if (Regex.IsMatch(command, @"dir [a-z]+"))
        {
            var newDir = _currentDirectory.EndsWith("/")
                ? _currentDirectory + command.Split()[1]
                : _currentDirectory + "/" + command.Split()[1];

            var newEntry = new Directory()
            {
                Name = newDir,
                Children = new(),
                Size = 0
            };

            if (!_directories.Select(x => x.Name).Contains(newDir))
            {
                _directories.Add(newEntry);
            }

            if (!currentDir.Children.Any() || !currentDir.Children.Select(x => x.Name).Contains(newDir))
            {
                currentDir.Children.Add(newEntry);
            }
        }

        if (Regex.IsMatch(command, @"[0-9]+ [a-z]+"))
        {
            currentDir.Size += int.Parse(command.Split()[0]);
        }
    }

    private void processCdInput(string command)
    {
        if (command.StartsWith("$ cd"))
        {
            var dir = command.Split()[2];
            switch (dir)
            {
                case "/":
                    _currentDirectory = dir;
                    break;
                case "..":
                    _currentDirectory = _currentDirectory.Substring(0, _currentDirectory.LastIndexOf("/"));
                    break;
                default:
                {
                    var newDir = _currentDirectory.EndsWith("/")
                        ? _currentDirectory += dir
                        : _currentDirectory += "/" + dir;

                    // Add directory if it wasn't already added
                    if (!_directories.Select(x => x.Name).Contains(newDir))
                    {
                        _directories.Add(new Directory()
                        {
                            Name = newDir,
                            Children = new(),
                            Size = 0
                        });
                    }

                    break;
                }
            }
        }
    }
}

internal static class Program
{
    static void Main(string[] args)
    {
        var space = new Space();
        space.SolveProblem1("dummydata.txt").Should().Be(95437);
        space.SolveProblem2("dummydata.txt").Should().Be(24933642);
        var solution1 = space.SolveProblem1("data.txt");
        var solution2 = space.SolveProblem2("data.txt");
        
        Console.WriteLine($"Found solutions {solution1} and {solution2}");
    }
}