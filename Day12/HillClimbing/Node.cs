namespace HillClimbing;

class Node : IComparable<Node>
{
    public int Row { get; set; }
    public int Col { get; set; }
    public char Height { get; set; }
    public int Cost { get; set; }

    public Node(int row, int col, char height) {
        Row = row; // x coordinate of node on the heightmap
        Col = col; // y coordinate of node on the heightmap
        Height = height;
        Cost = int.MaxValue;
    }
    
    public int CompareTo(Node other)
    {
        return Cost.CompareTo(other.Cost);
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }

        Node other = (Node)obj;
        return Row == other.Row && this.Col == other.Col && this.Height == other.Height;
    }

    public override int GetHashCode()
    {
        return Row.GetHashCode() ^ Col.GetHashCode() ^ Height.GetHashCode();
    }
}