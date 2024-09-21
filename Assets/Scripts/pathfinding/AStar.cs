using UnityEngine;
using System.Collections;

public class AStar {

	public static List closedList, openList;

	private static float CalculateEuclideanCost(Node startNode, Node endNode) {
		// find vector between positions
		Vector3 vecCost = startNode.position - endNode.position;
		// return the magnitude
		return vecCost.magnitude;
	}


	public static ArrayList FindPath(Node start, Node goal) {

		// create the closed list of nodes, initially empty
		closedList = new List();

		// create the open list of nodes
		openList = new List();

		// A. add the start node to the open list
		openList.Add(start);
	    

		// B. Calculate h, g and f of start square/node
		// g value is the movement cost along the path to the start node.
		start.g = 0.0f;
		//h value is distance from the start box A to B as the destination
		//for the h value, heuristic, we will use the Euclidean distance returned by a function 
		start.h = CalculateEuclideanCost(start,goal);
		//f = g+h
		start.f = start.h + start.g;


	
	    
		Node currentNode = null;

		// while (we have not reached our goal (openList.Length != 0))
		while (openList.Length != 0) {

			// C. consider the best node in the open list (the node with the lowest f value) - call it the current node

		    
            
			// if the current node is the goal then we're done
			if (currentNode.position == goal.position) {
				return CalculatePath(currentNode);
			}

			// D. Move the current node to the closed list (& remove it from the open list)
            

			// get all of the current nodes neighbors
			ArrayList neighbours = new ArrayList();
			GridManager.instance.GetNeighbours(currentNode, neighbours);
	
			// for each neighbor node
			for (int i = 0; i < neighbours.Count; i++) {
				
				Node neighbourNode = (Node)neighbours[i];

				// if the neighbour node is in the closed list then ignore it (continue)
				if (closedList.Contains(neighbourNode)) {
					continue;
				}

				// if the neighbour node is not on the open list add it
				if (!openList.Contains(neighbourNode)) {
					// E. Set the parent of the neighbour node to be the current node
				    

				    // F. Calculate g, h and f of neighbourNode
				    
                    
				    // G. Add neighbourNode to open list
                    
				}
				// if it's already on the open list, check to see if this path to the node is better
				else {

					float gTentative = 0f;

					// H. calculate what the g value would be if we go through the current node
				    

					// if the path is shorter
					if (gTentative < neighbourNode.g) {
                        // I. set the parent of neighbourNode to be the currentNode
					    

                        // J. Set g value of neighbourNode to gTentative, recalculate h and f for neighbourNode
                        
                        
                        // sort the list to put best f value at start
                        openList.Sort();
					}
				}
			}
		}

		if ((currentNode == null) || (currentNode.position != goal.position)) {
			Debug.LogError("Goal Not Found");
			return null;
		}

		return CalculatePath(currentNode);
	}
	

	private static ArrayList CalculatePath(Node node) {
		ArrayList list = new ArrayList();
		while (node != null) {
			list.Add(node);
			node = node.parent;
		}
		list.Reverse();
		return list;
	}
}
