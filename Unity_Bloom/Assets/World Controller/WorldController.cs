using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	private BackgroundGenerator bgGenerator;
	private GridGenerator gridGenerator;

	void Awake () {
		bgGenerator = transform.GetComponent<BackgroundGenerator>();
		gridGenerator = transform.GetComponent<GridGenerator>();

		if (Application.isPlaying) {
			bgGenerator.enabled = false;
			//gridGenerator.enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
		bgGenerator.GenerateBackground();
		gridGenerator.GenerateGrid();
	}

	void OnApplicationQuit () {

	}
}
