namespace Valve;

public class Valve
{
    // This we can get rid of if we use a dictionary instead of a List<Valve> structure
    public string Name { get; set; }
    public string ConnectedValvesString { get; set; }
    public List<Valve> ConnectedValves { get; set; } = new();
    public bool Open { get; set; } = false;

    public int timeLeft { get; set; } = 30;
    public int Flow { get; set; }

    public int Pressure { get; set; } = Int32.MinValue;
}