using System.Text.RegularExpressions;
using FluentAssertions;

namespace Space;

public class Space
{
    private List<(string name, int depth, int size)> _directories = new() { ("/", 0, 0) };
    private int _depth = 0;
    private string _currentDirectory = "/";

    public int SolveProblems(string file, bool secondExercise)
    {
        var commands = File.ReadLines(file);
        foreach (var command in commands)
        {
            if (command.StartsWith("$ cd"))
            {
                var dir = command.Split()[1];
                if (dir == "/")
                {
                    _depth = 0;
                    _currentDirectory = dir;
                }
                else if (dir == "..")
                {
                    _depth--;
                }
                else
                {
                    _currentDirectory = dir;
                    _depth++;
                    if (_directories.Count(x => x.name == dir) > 0)
                    {
                        _directories.Add((dir, _depth, 0));
                    }
                }
            }
            
            if (Regex.IsMatch(command, @"dir [a-z]+"))
            {
                var newDir = command.Split()[1];
                if (!_directories.Select(x => x.name == newDir).Any())
                {
                    _directories.Add((newDir, _depth + 1, 0));
                }
            }
             
            if (Regex.IsMatch(command, @"[0-9]+ [a-z]+"))
            {
                
            }
            
            var entry = _directories[_depth].Single(x => x.name == dir);
            entry.size += 

            
        }
    }
}


internal static class Program
{
    static void Main(string[] args)
    {
        var space = new Space();
        space.SolveProblems("dummydata.txt", false).Should().Be(7);
        // tuning.SolveProblems("dummydata.txt", true).Should().Be(19);
        //
        // var firstResult = tuning.SolveProblems("data.txt", false);
        // var secondResult = tuning.SolveProblems("data.txt", true);
        // Console.WriteLine($"{firstResult} and {secondResult}");
    }
}