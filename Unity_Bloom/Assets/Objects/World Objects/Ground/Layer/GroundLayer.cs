using UnityEngine;
using System.Collections;

[System.Serializable]
public class GroundLayer : MonoBehaviour {

	[Range(0.0f, 1.0f)]
	public float percentScreenHeight = 0.6f;
	public int height = 0;
	public int heightOffset = 0;
	
	public Transform mesh;
	
	
	void Start () {
		CommonInit();
	}

	protected void CommonInit () {
		CalculateHeight();
		InitMesh();
	}
	
	public void InitMesh() {
		
		// Get the mesh child.
		mesh = transform.Find("Mesh");
		
		// Size it.
		transform.localScale = new Vector3(Screen.width,
		                                   height,
		                                   1.0f);
		
		// Place it.
		PlaceLayer();
	}

	public void PlaceLayer() {
		Debug.Log(name + ".y => " + transform.position.y);
		int yPos = (-Screen.height / 2) + (height / 2) + heightOffset;
		transform.position = new Vector3(0.0f, yPos, 10.0f);
	}
	
	void CalculateHeight () {
		
		// Convert percentScreenHeight into actual pixels.
		//   -If no percent is given, just use height.
		if (percentScreenHeight > 0) {
			height = (int)(Screen.height * percentScreenHeight);
		}
	}

}
