namespace HillClimbing;

class Node : IComparable<Node>
{
    public char label { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int distance { get; set; }
    
    public Node(char label, int x, int y, int distance) {
        this.label = label;
        this.x = x; // x coordinate of node on the heightmap
        this.y = y; // y coordinate of node on the heightmap
        this.distance = distance;
    }

    public int CompareTo(Node? other)
    {
        return (label == other.label && this.x == other.x && this.y == other.y && this.distance == other.distance)
            ? 1
            : 0;
    }
}