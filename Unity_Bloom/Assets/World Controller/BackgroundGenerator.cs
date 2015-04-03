using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BackgroundGenerator : MonoBehaviour {

	// Generic
	public Transform genericQuad;

	// Flower
	public Transform flower;
	public int flowerDepth = 0;

	// Dirt
	public Texture dirtTexture;
	[Range(0.0f, 1.0f)]
	public float dirtScreenHeight = 0.6f;
	public int dirtDepth = 10;
	public Transform dirtObject;

	// Grass
	public Texture grassTexture;
	[Range(0.0f, 1.0f)]
	public float grassScreenHeight = 0.1f;
	public float grassDepth = 10;
	public Transform grassObject;

	// Fence
	public Transform fenceObject;
	public int fenceDepth = 10;
	public int fencePostScale = 80;
	private int fencePostCount = 0;
	public int fenceOffset = 15;
	public List<Transform> fencePosts = new List<Transform>();


	void Start () {
		//GenerateBackground();
	}


	// Update is called once per frame
	void Update () {
		GenerateBackground();
	}

	public void GenerateBackground () {
		UpdateDirt();
		UpdateFlower();
		UpdateGrass();
		UpdateFence();
	}

	void UpdateFlower() {
		int yPos = (int)(dirtObject.position.y + (dirtObject.localScale.y /2));
		flower.position = new Vector3(0.0f, yPos, 0.0f);
	}

	void UpdateDirt () {
		// Make sure there's dirt to work on.
		if (!dirtObject) {
			// Search the scene for some dirt.
			dirtObject = GameObject.Find("Dirt").transform;
			if (!dirtObject) {
				// There's no dirt in the scene, so make some.
				dirtObject = Instantiate(genericQuad, Vector3.zero, Quaternion.Euler(Vector3.zero)) as Transform;
				dirtObject.renderer.material.mainTexture = dirtTexture;
				dirtObject.name = "Dirt";
			}
		}

		// Scale it.
		dirtObject.localScale = new Vector3(Screen.width,
		                                    ScreenPercentToPixels(dirtScreenHeight),
		                                    1.0f);

		// Move it.
		float yPos = (int)(-(Screen.height / 2) + (dirtObject.localScale.y / 2));
		dirtObject.position = new Vector3(0.0f, yPos, dirtDepth);
	}

	void UpdateGrass () {
		// Make sure there's dirt to work on.
		if (!grassObject) {
			grassObject = Instantiate(genericQuad, Vector3.zero, Quaternion.Euler(Vector3.zero)) as Transform;
			grassObject.renderer.material.mainTexture = grassTexture;
			grassObject.name = "Grass";
		}
		
		// Scale it.
		grassObject.localScale = new Vector3(Screen.width,
		                                    ScreenPercentToPixels(grassScreenHeight),
		                                    1.0f);
		
		// Move it.
		int yPos = (int)(-(Screen.height / 2) + (grassObject.localScale.y / 2) + dirtObject.localScale.y);
		grassObject.position = new Vector3(0.0f, yPos, grassDepth);
	}

	void UpdateFence () {

		RemoveAllFencePosts();

		// We need a reference post to do some calculations.
		GameObject referencePost = fenceObject.gameObject;
		SpriteRenderer referenceRenderer = referencePost.GetComponent<SpriteRenderer>();

		// Get the base dimensions from the reference.
		float scaleRatio = (fencePostScale / referenceRenderer.sprite.textureRect.width);
		float postWidth = referenceRenderer.sprite.textureRect.width * scaleRatio;
		float postHeight = referenceRenderer.sprite.textureRect.height * scaleRatio;

		// Calculate how many posts needed to cover the screen.
		fencePostCount = Mathf.CeilToInt((Screen.width + fenceOffset) / postWidth) + 1;

		UpdateFenceCount();



		float xPos = -(Screen.width / 2) + (postWidth / 2) + fenceOffset;
		float YPos = ((grassObject.localScale.y / 2) + grassObject.position.y);
		for (int i = 0; i < fencePosts.Count; i++) {
			// Grab it.
			Transform post = fencePosts[i] as Transform;

			// Scale it.
			post.localScale = new Vector3(fencePostScale, fencePostScale, 1.0f);

			// Move it.
			post.position = new Vector3(xPos, YPos + (postHeight / 2), fenceDepth);

			// Update necessary loop values.
			xPos += fencePostScale;
		}
	}

	void UpdateFenceCount () {
		if (fencePosts.Count < fencePostCount) {
			// Add posts.
			for (int i = fencePosts.Count; i < fencePostCount; i++) {
				Transform newPost = Instantiate(fenceObject, 
				                                Vector3.zero, 
				                                Quaternion.Euler(Vector3.zero)) as Transform;
				newPost.name = "Post";
				newPost.tag = "Fence Post";
				fencePosts.Add(newPost);
			}
		}
		else if (fencePosts.Count > fencePostCount) {
			// Remove posts.
			for (int i = fencePosts.Count - 1; fencePosts.Count > fencePostCount; i--) {
				Transform oldPost = fencePosts[i] as Transform;
				fencePosts.RemoveAt(i);
				if (oldPost) {
					DestroyImmediate(oldPost.gameObject);
				}
			}
		}
	}

	void RemoveAllFencePosts() {
		foreach (GameObject post in GameObject.FindGameObjectsWithTag("Fence Post")) {
			DestroyImmediate(post);
		}

		fencePosts = new List<Transform>();
	}


	// Utility

	int ScreenPercentToPixels(float percent) {
		return (int)(Screen.height * percent);
	}
}















