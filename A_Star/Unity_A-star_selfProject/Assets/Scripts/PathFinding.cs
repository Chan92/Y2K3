using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding:MonoBehaviour {
	private Grid myGrid;
	List<Node> openSet = new List<Node>();
	

	public Transform seeker, target;

	private void Awake() {
		myGrid = GetComponent<Grid>();
	}

	private void Update() {
		if(Input.GetKeyDown(KeyCode.Space))
			FindPath(seeker.position, target.position);
	}

	private void FindPath (Vector3 startPos, Vector3 targetPos){
		//print("found path, A:" + startPos + " - B:" + targetPos);
		Node startNode = myGrid.GetNodeFromWorldPoint(startPos);
		Node targetNode = myGrid.GetNodeFromWorldPoint(targetPos);
		HashSet<Node> closedSet = new HashSet<Node>();

		openSet.Add(startNode);

		while(openSet.Count > 0) {
			Node currentNode = GetCurrentNode();
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);

			if(currentNode == targetNode) {
				RetracePath(startNode, targetNode);
				return;
			}

			foreach(Node neighbour in GetNeighbours(currentNode)) {
				if(!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				CheckNeighbours(currentNode, neighbour, targetNode);
			}
		}
	}

	private Node GetCurrentNode() {
		Node curNode = openSet[0];

		for(int i = 1; i < openSet.Count; i++) {
			if(openSet[i].fCost < curNode.fCost ||
				(openSet[i].fCost == curNode.fCost && openSet[i].hCost < curNode.hCost)) {
				curNode = openSet[i];
			}
		}

		return curNode;
	}

	private List<Node> GetNeighbours(Node node) {
		List<Node> neigbours = new List<Node>();

		for(int x = -1; x <= 1; x++) {
			for(int y = -1; y <= 1; y++) {
				//blocks self and diagonal
				if(Mathf.Abs(x) == Mathf.Abs(y)) {
					continue;
				}

				int checkPosX = node.gridPosition.x + x;
				int checkPosY = node.gridPosition.y + y;

				if(checkPosX >= 0 && checkPosX < myGrid.gridSize.x &&
					checkPosY >= 0 && checkPosY < myGrid.gridSize.y) {
					neigbours.Add(myGrid.grid[checkPosX, checkPosY]);
				}
			}
		}

		return neigbours;
	}

	private int GetDistance(Node nodeA, Node nodeB) {
		int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
		int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

		return distX + distY * 10;
	}

	private void CheckNeighbours(Node currentNode, Node neighbour, Node targetNode) {
		int newMoveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
		if(newMoveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
			neighbour.gCost = newMoveCostToNeighbour;
			neighbour.hCost = GetDistance(neighbour, targetNode);
			neighbour.parent = currentNode;

			if(!openSet.Contains(neighbour)) {
				openSet.Add(neighbour);
			}
		}
	}

	private void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while(currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse();
		myGrid.path = path;

		//StopCoroutine(FollowPath(path));
		//StartCoroutine(FollowPath(path));
	}

	//testing
	private IEnumerator FollowPath(List<Node> path) {
		while(path.Count > 1) {
			seeker.position = path[0].worldPosition;
			path.RemoveAt(0);
			yield return new WaitForSeconds(0.7f);
		}
	}
}
