using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable {
	
    public float f;
    public float g;
    public float h;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;
    public bool walkable; // Added walkable property

    public bool isamong4gates; // Mark whether the node is part of the 4-node opening
    public bool isTheFourthNode;  // New property for node number 4

    public Node() {
        this.h = 0.0f;
        this.g = 1.0f;
        this.f = this.h + this.g;
        this.bObstacle = false;
        this.walkable = true; // Default value
        this.parent = null;
    }

    public Node(Vector3 pos) {
        this.position = pos;
        this.h = 0.0f;
        this.g = 1.0f;
        this.f = this.h + this.g;
        this.bObstacle = false;
        this.walkable = true; // Default value
        this.parent = null;
        
        isamong4gates = false; // Default to false
        isTheFourthNode = false;  // Initialize as false
        
    }

    public void MarkAsObstacle() {
        this.bObstacle = true;
        this.walkable = false; // If it's an obstacle, it's not walkable
    }

    public int CompareTo(object obj) {
        Node node = (Node)obj;
        if (this.h < node.h) return -1;
        if (this.h > node.h) return 1;
        return 0;
    }
}
