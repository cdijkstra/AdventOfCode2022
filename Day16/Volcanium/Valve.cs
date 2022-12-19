namespace Valve;

public class Valve
{
    public string Name { get; set; }
    public bool Open { get; set; } = false;

    public int Flow { get; set; }

    public string ConnectedValvesString { get; set; }
    public List<Valve> ConnectedValves { get; set; } = new();
}