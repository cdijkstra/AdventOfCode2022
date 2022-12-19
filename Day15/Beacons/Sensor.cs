namespace Space;

public class Sensor
{
    public Coordinate Position { get; set; }

    public Beacon ClosestBeacon { get; set; } = new();
    
    public long DistanceToBeacon => DistanceTo(ClosestBeacon.Position);
    
    public long DistanceTo(Coordinate position) =>
        Math.Abs(Position.x - position.x) +
        Math.Abs(Position.y - position.y);
}