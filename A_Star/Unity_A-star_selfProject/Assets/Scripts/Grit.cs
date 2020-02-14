using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grit : MonoBehaviour{
	[SerializeField]
	private LayerMask unwalkableMask;
	[SerializeField]
	private Vector2 gridWorldSize;
	[SerializeField]
	private float nodeRadius;

	private Node[,] grid;

	private void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
	}
}
