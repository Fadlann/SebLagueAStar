using UnityEngine;

public class Node{
    public bool walkable;
    public Vector3 worldPoint;
    public int gridX, gridY;
    public Node parent;

    public Node(bool walkable, Vector3 worldPoint, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPoint = worldPoint;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int gCost;
    public int hCost;
    public int fCost{ get { return gCost + hCost; }}
}
