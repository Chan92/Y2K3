using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{
	public Vector3 worldPosition;
	public bool walkable;
	public Vector2Int gridPosition;
	public Node parent;

	public int gCost;
	public int hCost;
	
	public Node(Vector3 worldPos, bool isWalkable, Vector2Int gridPos) {
		worldPosition = worldPos;
		walkable = isWalkable;
		gridPosition = gridPos;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

}
