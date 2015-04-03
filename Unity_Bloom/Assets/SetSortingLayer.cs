using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour {

	public LayerMask sortingLayer;

	// Use this for initialization
	void Start () {
		particleSystem.renderer.sortingLayerName = sortingLayer.ToString();
	}
}
