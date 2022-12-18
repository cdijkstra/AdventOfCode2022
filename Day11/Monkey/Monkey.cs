using System.Numerics;

namespace Monkey;

class Monkey
{
    public int Number { get; set; } = new();

    public Func<long, long> Operation { get; set; }

    public List<long> Items { get; set; } = new();
    
    public long DivisibleBy { get; set; } = new();

    public List<int> ThrowToMonkey { get; set; } = new();

    public long Inspections { get; set; } = 0;
}