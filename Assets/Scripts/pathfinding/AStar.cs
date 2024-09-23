using System.Collections;
using UnityEngine;

public class AStar 
{
    public static List closedList, openList;

    private static float CalculateEuclideanCost(Node startNode, Node endNode) 
    {
        Vector3 vecCost = startNode.position - endNode.position;
        return vecCost.magnitude;
    }

    public static ArrayList FindPath(Node start, Node goal) 
    {
        if (start == null || goal == null)
    {
        Debug.LogError("Start or Goal node is null. Check if the positions are within the grid bounds");
        return null;
    }
        // Initialize closed and open lists
        closedList = new List();
        openList = new List();

        // A. Add the start node to the open list
        openList.Add(start);

        // B. Calculate h, g, and f of start node
        Debug.Log("Start Node: " + start);
        Debug.Log("Goal Node: " + goal);

        start.g = 0.0f;
        start.h = CalculateEuclideanCost(start, goal);
        start.f = start.g + start.h;

        Node currentNode = null;

        // While there are nodes to process
        while (openList.Length != 0) 
        {
            // C. Consider the best node in the open list (node with lowest f)
            currentNode = openList.First(); // Use your custom First() method
            for (int i = 1; i < openList.Length; i++)
            {
                Node candidate = openList.Get(i);
                if (candidate.f < currentNode.f || (candidate.f == currentNode.f && candidate.h < currentNode.h))
                {
                    currentNode = candidate;
                }
            }

            // If the current node is the goal, we've found the path
            if (currentNode.position == goal.position) 
            {
                return CalculatePath(currentNode);
            }

            // D. Move the current node to the closed list and remove from open list
            closedList.Add(currentNode);
            openList.Remove(currentNode);

            // Get all neighbors of the current node
            ArrayList neighbours = new ArrayList();
            GridManager.instance.GetNeighbours(currentNode, neighbours);

            // Iterate through each neighbor
            foreach (Node neighbourNode in neighbours) 
            {
                // If the neighbor is not walkable or already in closed list, skip
                if (!neighbourNode.walkable || closedList.Contains(neighbourNode)) 
                {
                    continue;
                }

                // Calculate new movement cost to neighbor
                float newMovementCostToNeighbour = currentNode.g + CalculateEuclideanCost(currentNode, neighbourNode);

                // If the new path to neighbour is shorter or neighbour not in open list
                if (newMovementCostToNeighbour < neighbourNode.g || !openList.Contains(neighbourNode)) 
                {
                    // E. Set parent to current node
                    neighbourNode.parent = currentNode;

                    // F. Update g, h, and f costs
                    neighbourNode.g = newMovementCostToNeighbour;
                    neighbourNode.h = CalculateEuclideanCost(neighbourNode, goal);
                    neighbourNode.f = neighbourNode.g + neighbourNode.h;

                    // G. Add neighbour to open list if not already there
                    if (!openList.Contains(neighbourNode)) 
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // If we exit the loop without finding the goal
        Debug.LogError("Goal Not Found");
        return null;
    }

    private static ArrayList CalculatePath(Node node) 
    {
        ArrayList path = new ArrayList();
        Node currentNode = node;
        while (currentNode != null) 
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
}
