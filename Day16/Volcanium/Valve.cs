namespace Valve;

public class Valve
{
    public string ConnectedValvesString { get; set; }

    public string Name { get; set; }
    public bool Open { get; set; } = false;
    
    public int Flow { get; set; }

    public int Pressure { get; set; } = 0;
}