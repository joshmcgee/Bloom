using UnityEngine;
using System.Collections;

public class SwayController : MonoBehaviour {

	public float frequency = 2.0f;
	public float amplitude = 10.0f;
	public float swayOffset = 0.0f;
	
	public bool isLeftLeaf = false;

	public float restAngle = 0.0f;
	private float currentOffset;
	private float currentAngle;


	// Use this for initialization
	void Start () {
		//restAngle = transform.localRotation.z * Mathf.Rad2Deg;
	}
	
	// Update is called once per frame
	void Update () {

		currentOffset = Mathf.Sin((Time.time + swayOffset) * frequency) * amplitude;
		if (isLeftLeaf) {
			currentOffset *= -1;
		}
		
		currentAngle = restAngle + currentOffset;

		transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, currentAngle));

		//Debug.Log("Current Angle => " + Mathf.Sin(Time.time));
	}
}




























