using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {
    
    public Transform seeker, target;

    Grid grid;

    private void Awake() 
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        // getting the starting and the end node
        Node startingNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startingNode);

        while( openSet.Count() > 0 )
        {
            // searching for the lowest fCost for the current node
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count(); i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||  openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startingNode, endNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if ( !neighbor.walkable || closedSet.Contains(neighbor) )
                    continue;

                int newgCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                if( newgCost < neighbor.gCost || !openSet.Contains(neighbor) )
                {
                    neighbor.gCost = newgCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if ( !openSet.Contains(neighbor) )
                        openSet.Add(neighbor);
                }
            }
        }
    }

    void RetracePath( Node startNode, Node endNode )
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while ( currentNode != startNode )
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();

        grid.path = path;
    }

    // for additional information, see the documentation
    int GetDistance(Node nodeA, Node nodeB)
    {  
        /* 
        values
        diagonal = 14
        horizontal / vertical = 10
         */
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if( distX > distY )
            return 14*distY + 10*( distX - distY );
        else
            return 14*distX + 10*( distY - distX );
    }

}