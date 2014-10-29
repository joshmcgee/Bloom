using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MainCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {

		// Size the camera to match the screen.
		// (This makes the image to screen pixel ratio 1:1.
		camera.orthographicSize = Screen.height / 2;
	}

	void Update () {
		camera.orthographicSize = Screen.height / 2;
	}
}
