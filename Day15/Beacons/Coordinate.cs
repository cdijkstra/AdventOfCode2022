namespace Space;

public class Coordinate : IEquatable<Coordinate>
{
    public long x { get; set; }
    
    public long y { get; set; }

    public bool Equals(Coordinate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return x == other.x && y == other.y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }
}