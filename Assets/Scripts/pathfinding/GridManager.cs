using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

    private static GridManager s_Instance = null;

    public static GridManager instance {
        get {
            if (s_Instance == null) {
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager object. You have to have exactly one GridManager in the scene.");
            }
            return s_Instance;
        }
    }

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    public bool allowDiagonal = true;
    public Color gridColor = Color.blue;
    
    // Make origin public and set to arena's position
    // public Vector3 origin = new Vector3(749f, 16.74f, 1316.8f); 
    private Vector3 origin = new Vector3();
	public Node[] gateNodes = new Node[4];  // Store your four gate nodes

    List<Vector3> obstacleBlockList;

    public Node[,] nodes { get; set; }
	public Vector3 Origin {
		get { return origin; }
	}

    void Awake() {
		origin = new Vector3(743.56f, 15.98f, 1286.52f);
        obstacleBlockList = new List<Vector3>();

  		CalculateObstacles();
		MarkFourNodeOpening();
    }


	// Find all the obstacles on the map
	void CalculateObstacles() {
		nodes = new Node[numOfColumns, numOfRows];
		int index = 0;
		for (int i = 0; i < numOfRows; i++) {
			for (int j = 0; j < numOfColumns; j++) {
				Vector3 cellPos = GetGridCellCenter(index);
				Node node = new Node(cellPos);
				nodes[j, i] = node;

				// cast a ray from above the center of each cell
				RaycastHit hit;
				if (Physics.Raycast(cellPos + new Vector3(0, 10f, 0), -Vector3.up, out hit, 10f))
				{
					// if it collides with an object tagged 'Obstacle' mark the cell
					// as being an obstacle
					if (hit.collider.gameObject.tag == "Obstacle")
					{
						nodes[j, i].MarkAsObstacle();
						obstacleBlockList.Add(cellPos);
					}
				}
				index++;
			}
		}
	}

	void MarkFourNodeOpening()
    {
     //positions for the 4 arena gates
        gateNodes[0] = nodes[22, 20];//row col
    	gateNodes[1] = nodes[22, 26];
    	gateNodes[2] = nodes[22, 35];
    	gateNodes[3] = nodes[22, 44];

        // Mark these nodes as part of the 4-node opening
        gateNodes[0].isamong4gates = true;
        gateNodes[1].isamong4gates = true;
        gateNodes[2].isamong4gates = true;
        gateNodes[3].isamong4gates = true;

		gateNodes[0].walkable = true;  // Ensure they are walkable
    	gateNodes[1].walkable = true;
    	gateNodes[2].walkable = true;
    	gateNodes[3].walkable = true;

		gateNodes[3].isTheFourthNode = true;  // New property to identify the 4th node
    }

	public Vector3 GetGridCellCenter(int index) {
		Vector3 cellPosition = GetGridCellPosition(index);
		cellPosition.x += (gridCellSize / 2.0f);
		cellPosition.z += (gridCellSize / 2.0f);
		return cellPosition;
	}

	public Vector3 GetGridCellPosition(int index) {
		int row = GetRow(index);
		int col = GetColumn(index);
		float xPosInGrid = col * gridCellSize;
		float zPosInGrid = row * gridCellSize;
		return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
	}

	public int GetGridIndex(Vector3 pos) {
		if (!IsInBounds(pos)) {
			return -1;
		}
		pos -= Origin;
		int col = (int)(pos.x / gridCellSize);
		int row = (int)(pos.z / gridCellSize);
		return (row * numOfColumns + col);
	}

	public bool IsInBounds(Vector3 pos) {
		float width = numOfColumns * gridCellSize;
		float height = numOfRows* gridCellSize;

		return ((pos.x >= Origin.x) && (pos.x <= Origin.x + width) && (pos.z <= Origin.z + height) && (pos.z >= Origin.z));
	}

	public int GetRow(int index) {
		int row = index / numOfColumns;
		return row;
	}

	public int GetColumn(int index) {
		int col = index % numOfColumns;
		return col;
	}

	public void GetNeighbours(Node node, ArrayList neighbors) {
		Vector3 neighborPos = node.position;
		int neighborIndex = GetGridIndex(neighborPos);

		int row = GetRow(neighborIndex);
		int column = GetColumn(neighborIndex);

		if (allowDiagonal) {
			for (int i=row-1; i<=row+1; i++) {
				for (int j=column-1; j<=column+1; j++) {
					if (!((i==row) && (j==column))) {
						AssignNeighbour(i, j, neighbors);
					}
				}
			}
		}
		else {
			AssignNeighbour(row-1, column, neighbors);
			AssignNeighbour(row+1, column, neighbors);
			AssignNeighbour(row, column-1, neighbors);
			AssignNeighbour(row, column+1, neighbors);
		}
	}

	void AssignNeighbour(int row, int column, ArrayList neighbors) {
		if (row != -1 && column != -1 &&
		    row < numOfRows && column < numOfColumns) {

			Node nodeToAdd = nodes[column, row];
			if (!nodeToAdd.bObstacle) {
				neighbors.Add(nodeToAdd);
			}
		}
	}

	void OnDrawGizmos() {
		if (showGrid) {
			DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, gridColor);
		}

		if (showObstacleBlocks) {
			Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
			Gizmos.color = new Color(1, 0, 0, 0.5f);
			if (obstacleBlockList != null) {
				foreach (Vector3 data in obstacleBlockList) {
					Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data)), cellSize);
				}
			}
		}
		// Debug
		 if (nodes != null) {
        Gizmos.color = Color.red;
        for (int i = 0; i < numOfRows; i++) {
            for (int j = 0; j < numOfColumns; j++) {
                if (nodes[j, i].bObstacle) {
                    Gizmos.color = Color.red;
                } else {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawCube(nodes[j, i].position, Vector3.one * 0.5f);
            }
        }
    }
	
	}

	public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color) {
    float width = numCols * cellSize;
    float height = numRows * cellSize;

    // Draw the horizontal grid lines
    for (int i = 0; i <= numRows; i++) {
        Vector3 startPos = origin + new Vector3(0.0f, 0.05f, i * cellSize);
        Vector3 endPos = startPos + new Vector3(width, 0.0f, 0.0f);
        Debug.DrawLine(startPos, endPos, color);
    }

    // Draw the vertical grid lines
    for (int i = 0; i <= numCols; i++) {
        Vector3 startPos = origin + new Vector3(i * cellSize, 0.05f, 0.0f);
        Vector3 endPos = startPos + new Vector3(0.0f, 0.0f, height);
        Debug.DrawLine(startPos, endPos, color);
    }
}


	public Node NodeFromWorldPoint(Vector3 worldPosition) {
     // Check if the position is within bounds
    if (!IsInBounds(worldPosition)) {
        Debug.Log("Position is out of bounds: " + worldPosition);  // Debug message
        return null; // Return null if out of bounds
    }
    
    // Calculate the index based on the world position
    int index = GetGridIndex(worldPosition);
    
    if(index == -1) {
        Debug.Log("Invalid grid index for world position: " + worldPosition);  // Debug message
        return null; // Invalid index, return null
    }
    
    // Return the corresponding node from the grid
    return nodes[GetColumn(index), GetRow(index)];

}

}

