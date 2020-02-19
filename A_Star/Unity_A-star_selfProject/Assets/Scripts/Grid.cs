using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour{
	[SerializeField]
	private LayerMask unwalkableMask;
	[SerializeField]
	private Vector2 gridWorldSize;
	[SerializeField]
	private float nodeRadius;

	[HideInInspector]
	public Node[,] grid;
	private float nodeDiameter;

	[HideInInspector]
	public Vector2Int gridSize;

	private void Start() {
		nodeDiameter = nodeRadius * 2;
		gridSize.x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSize.y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrit();
	}

	private void CreateGrit() {
		grid = new Node[gridSize.x, gridSize.y];
		Vector3 worldBottomLeft = transform.position - 
			(Vector3.right * gridWorldSize.x * 0.5f) - (Vector3.forward * gridWorldSize.y * 0.5f);

		for(int x = 0; x < gridSize.x; x++) {
			for(int y = 0; y < gridSize.y; y++) {
				Vector3 worldPoint = worldBottomLeft +
					Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckBox(worldPoint, Vector3.one * nodeRadius, Quaternion.identity, unwalkableMask));
				grid[x, y] = new Node(worldPoint, walkable, new Vector2Int(x, y));
			}
		}
	}

	public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
		Vector2 percentage = new Vector2(
			(worldPosition.x + gridWorldSize.x *0.5f) / gridWorldSize.x,
			(worldPosition.z + gridWorldSize.y * 0.5f) / gridWorldSize.y);

		percentage.x = Mathf.Clamp01(percentage.x);
		percentage.y = Mathf.Clamp01(percentage.y);

		int x = Mathf.RoundToInt((gridSize.x - 1) * percentage.x);
		int y = Mathf.RoundToInt((gridSize.y - 1) * percentage.y);

		return grid[x, y];
	}

	public List<Node> path;
	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if(grid != null) {
			foreach(Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				if(path != null) {
					if(path.Contains(n))
						Gizmos.color = Color.black;
				}
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}
