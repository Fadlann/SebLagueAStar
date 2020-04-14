using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    public LayerMask unwalkableMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public List<Node> path;

    Node[,] grid;

    float nodeDiameter;
    int nodeCountX, nodeCountY;

    private void Start() 
    {
        nodeDiameter = nodeRadius * 2;

        nodeCountX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        nodeCountY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[nodeCountY, nodeCountX];

        Vector3 worldBottomLeft = transform.position - Vector3.right*gridSize.x/2f - Vector3.forward*gridSize.y/2f;
        
        for (int y = 0; y < nodeCountY; y++)
        {
            for (int x = 0; x < nodeCountX; x++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right*(nodeDiameter*x + nodeRadius) 
                    + Vector3.forward*(nodeDiameter*y + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                grid[y, x] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if( grid != null ) 
        {
            foreach(Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Debug.Log("changing path color");
                        Gizmos.color = Color.black;
                    }
                }
                Gizmos.DrawCube(node.worldPoint, Vector3.one*(nodeDiameter-.1f));
            }
        }   
    }

    public Node NodeFromWorldPoint(Vector3 worldPoint)
    {
        float percentX = (worldPoint.x + gridSize.x/2f) / gridSize.x;
        float percentY = (worldPoint.z + gridSize.y/2f) / gridSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt( (nodeCountX - 1) * percentX );
        int y = Mathf.RoundToInt( (nodeCountY - 1) * percentY );

        return grid[y, x];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if ( x == 0 && y == 0)
                    continue;
                
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if ( checkX > -1 && checkY > -1 && checkX < gridSize.x && checkY < gridSize.y )
                    neighbors.Add(grid[checkY, checkX]);
            }
        }

        return neighbors;
    }
}