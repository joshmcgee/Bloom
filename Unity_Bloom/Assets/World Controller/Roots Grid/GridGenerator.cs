using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour {

	public Transform dirtTrans;

	public int sideMarginWidth = 10;
	public int topMarginWidth = 0;
	public int bottomMarginWidth = 10;

	public bool displayGridArea = false;
	public float gridWidth;
	public float gridHeight;
	private Transform gridTrans;
	private Vector3 topLeftPos;

	public bool displayGridNodes = false;
	public GameObject nodeObject;
	public int nodesAcross = 9;
	public int nodesDown = 10;
	public List<Transform> gridNodes = new List<Transform>();
	private float nodeXDistance;
	private float nodeYDistance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		GenerateGrid();
	}

	public void GenerateGrid() {
		// If we don't know dirt, we don't know anything.
		if (GetDirt()) {
			if (GetGridTrans()) {
				UpdateGridDimensions();
				UpdateGridNodes();
			}
		}
	}

	bool GetDirt () {
		if (!dirtTrans) {
			GameObject dirtObject = GameObject.Find("Dirt");
			if (dirtObject) {
				dirtTrans = dirtObject.transform;
				return true;
			}
			return false;
		}
		return true;
	}

	bool GetGridTrans () {
		if (!gridTrans) {
			GameObject gridObject = GameObject.Find("Grid");
			if (gridObject) {
				gridTrans = gridObject.transform;
				return true;
			}
			return false;
		}
		return true;
	}

	void UpdateGridDimensions () {
		CalculateGridCenter();
		CalculateGridSize();

		topLeftPos = new Vector3(gridTrans.position.x - (gridWidth / 2),
		                         gridTrans.position.y + (gridHeight / 2),
		                         gridTrans.position.z);
	}

	void CalculateGridCenter () {
		float gridYCenter = dirtTrans.position.y + ((bottomMarginWidth - topMarginWidth) / 2);
		gridTrans.position = new Vector3(0.0f, 
		                                 gridYCenter, 
		                                 -10.0f);
	}

	void CalculateGridSize () {
		gridWidth = dirtTrans.localScale.x - (sideMarginWidth * 2);
		gridHeight = dirtTrans.localScale.y - topMarginWidth - bottomMarginWidth;
	}

	void UpdateGridNodes () {
		// Start Fresh.
		ClearNodeList();

		nodeXDistance = gridWidth / (nodesAcross - 1);
		nodeYDistance = gridHeight / (nodesDown - 1);

		bool rowIsOffset = false;
		int rowLength;
		float xPos = 0;
		float yPos = 0;

		// Loop through the rows.
		for (int row = 0; row < nodesDown; row ++) {

			// An offset row has one less node.
			if (rowIsOffset) {
				rowLength = nodesAcross - 1;
			}
			else {
				rowLength = nodesAcross;
			}

			// Loop through the collumns.
			for (int col = 0; col < rowLength; col++) {
				// Get the position of this new node.
				yPos = topLeftPos.y - (nodeYDistance * row);
				xPos = topLeftPos.x + (nodeXDistance * col);
				if (rowIsOffset) {
					xPos += nodeXDistance / 2;
				}

				// Create it.
				GameObject newNode = Instantiate(nodeObject, 
				                                 new Vector3(xPos, yPos, gridTrans.position.z), 
				                                 Quaternion.Euler(Vector3.zero)) as GameObject;
				newNode.name = "Grid Node (" + xPos + ", " + yPos + ")";
				newNode.transform.parent = gridTrans;
				gridNodes.Add(newNode.transform);

				// Show / Hide it
				SpriteRenderer nodeRenderer = newNode.transform.GetComponent<SpriteRenderer>();
				nodeRenderer.enabled = displayGridNodes;
			}

			// Toggle the row offset.
			rowIsOffset = !rowIsOffset;
		}
	}

	void ClearNodeList() {
		for (int i = gridNodes.Count - 1; i >= 0; i--) {
			Transform node = gridNodes[i] as Transform;
			gridNodes.RemoveAt(i);
			DestroyImmediate(node.gameObject);
		}
	}






	void OnDrawGizmos () {

		// Grid Rect
		if (displayGridArea && gridTrans) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(gridTrans.position, new Vector3(gridWidth, gridHeight, 1));
		}
	}
}














