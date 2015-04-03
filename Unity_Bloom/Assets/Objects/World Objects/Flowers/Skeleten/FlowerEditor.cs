using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FlowerEditor : MonoBehaviour {

	private bool savedEnabledState = true;

	public Transform genericSprite;
	
	public Transform stem;
	public int stemWidth = 5;
	public int stemHeight = 100;
	public Color stemColor = Vector4.one;

	public Transform flowerHead;

	public Transform eye;
	public int eyeDiameter = 30;
	public Color eyeColor = Vector4.one;

	public Transform eyeRing;
	public int eyeRingThickness = 5;
	public Color eyeRingColor = Vector4.one;

	public Sprite petalSprite;
	public int petalScale = 40;
	public int petalCount = 8;
	public int petalOffset = 20;
	public Color petalColor = Vector4.one;
	public List<Transform> petals = new List<Transform>();

	public Sprite petalStripeSprite;
	public int petalStripeScale = 20;
	public int petalStripeOffset = 0;
	public Color petalStripeColor = Vector4.one;
	public List<Transform> petalStripes = new List<Transform>();

	public Transform leafTrans;
	public Sprite leafSprite;
	public int leafPairCount = 3;
	public int leafLowScale = 45;
	public int leafHighScale = 25;
	public int leafLowHeight = 90;
	public int leafHighHeight = 180;
	public float leafAngle = 20.0f;
	public float leafSwayFrequency = 2.0f;
	public float leafSwayAmplitude = 10.0f;
	public float leafSwayOffset = 0.0f;
	private float currentLeafSwayOffset = 0.0f;
	public Color leafColor = Vector4.one;
	public List<Transform> leafPairs = new List<Transform>();

	void Start () {
		// Play Mode:
		if (Application.isPlaying) {
			// Turn off the editor so it doesn't muck anything up.
			Debug.Log("FlowerEditor: Play Mode");
			savedEnabledState = this.enabled;
			this.enabled = false;
		}
		// Editor:
		else {
			Debug.Log("FlowerEditor: Editor Mode");
			this.enabled = savedEnabledState;
		}
	}


	// Update is called once per frame
	void Update () {
		GenerateFlower();
	}

	void GenerateFlower() {
		UpdateStem();
		UpdateEye();
		UpdatePetals();
		UpdateLeaves();
	}

	void UpdateStem() {
		stem.localScale = new Vector3(stemWidth, stemHeight, 1.0f);
		stem.transform.localPosition = new Vector3(0.0f, stemHeight / 2, 0.0f);
		stem.renderer.sharedMaterial.color = stemColor;

		// Move any other parts.
		flowerHead.localPosition = new Vector3(0.0f, stemHeight, 0.0f);
	}

	void UpdateEye() {
		eye.localScale = new Vector3(eyeDiameter, eyeDiameter, 1.0f);
		eye.renderer.sharedMaterial.color = eyeColor;

		int ringScale = eyeDiameter + (eyeRingThickness * 2);
		eyeRing.localScale = new Vector3(ringScale, ringScale, 1.0f);
		eyeRing.renderer.sharedMaterial.color = eyeRingColor;
	}

	void UpdatePetals() {
		UpdatePetalCount();

		if (petals.Count > 0) {
			float deltaAngle = (Mathf.PI * 2.0f) / petals.Count;
			float angle = Mathf.PI / 2;
			for (int i = 0; i < petals.Count; i++) {

				// Grab the petal.
				Transform petal = petals[i] as Transform;

				// Scale the petal.
				petal.localScale = new Vector3(petalScale, petalScale, 1.0f);
				
				// Color the petal.
				SpriteRenderer petalRenerer = petal.GetComponent<SpriteRenderer>();
				petalRenerer.color = petalColor;
				
				// Rotate the petal.
				petal.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, (angle * Mathf.Rad2Deg) - 90));
				
				// Position the petal.
				petal.localPosition = new Vector3(Mathf.Cos(angle) * petalOffset,
				                                  Mathf.Sin(angle) * petalOffset,
				                                  0.0f);

				// Layer it properly.
				petalRenerer.sortingLayerName = "Flower";

				//--------------------------------------------

				// Grab the stripe.
				Transform stripe = petalStripes[i] as Transform;
				
				// Scale the stripe.
				stripe.localScale = new Vector3(petalStripeScale, petalStripeScale, 1.0f);
				
				// Color the stripe.
				SpriteRenderer stripeRenerer = stripe.GetComponent<SpriteRenderer>();
				stripeRenerer.color = petalStripeColor;
				
				// Rotate the stripe.
				stripe.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, (angle * Mathf.Rad2Deg) - 90));
				
				// Position the stripe.
				stripe.localPosition = new Vector3(Mathf.Cos(angle) * petalStripeOffset,
				                                   Mathf.Sin(angle) * petalStripeOffset,
				                                   0.0f);

				// Layer it properly.
				stripeRenerer.sortingLayerName = "Flower";

				//--------------------------------------------

				// Update necessary loop values.
				angle += deltaAngle;
			}

			/*foreach (Transform petal in petals) {
				// Scale it.
				petal.localScale = new Vector3(petalScale, petalScale, 1.0f);

				// Color it.
				SpriteRenderer petalRenerer = petal.GetComponent<SpriteRenderer>();
				petalRenerer.color = petalColor;

				// Rotate it.
				petal.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, (angle * Mathf.Rad2Deg) - 90));

				// Position it.
				petal.localPosition = new Vector3(Mathf.Cos(angle) * petalOffset,
				                                  Mathf.Sin(angle) * petalOffset,
				                                  0.0f);

				// Update necessary loop values.
				angle += deltaAngle;
			}*/
		}
	}

	void UpdatePetalCount() {
		if (petals.Count < petalCount) {
			// Add petals.
			for (int i = petals.Count; i < petalCount; i++) {

				// Petals
				Transform newPetal = Instantiate(genericSprite, 
				                                 flowerHead.position, 
				                                 Quaternion.Euler(Vector3.zero)) as Transform;
				newPetal.name = "Petal";
				newPetal.transform.parent = flowerHead;
				SpriteRenderer petalRenderer = newPetal.GetComponent<SpriteRenderer>();
				petalRenderer.sprite = petalSprite;
				petals.Add(newPetal);

				// Stripes
				Transform newStripe = Instantiate(genericSprite, 
				                                  flowerHead.position, 
				                                  Quaternion.Euler(Vector3.zero)) as Transform;
				newStripe.name = "Stripe";
				newStripe.transform.parent = flowerHead;
				SpriteRenderer stripeRenderer = newStripe.GetComponent<SpriteRenderer>();
				stripeRenderer.sprite = petalStripeSprite;
				stripeRenderer.sortingOrder = 1;
				petalStripes.Add(newStripe);
			}
		}
		else if (petals.Count > petalCount) {
			// Remove petals.
			for (int i = petals.Count - 1; petals.Count > petalCount; i--) {

				// Petals
				Transform oldPetal = petals[i] as Transform;
				petals.RemoveAt(i);
				if (oldPetal) {
					DestroyImmediate(oldPetal.gameObject);
				}

				// Stripes
				Transform oldStripe = petalStripes[i] as Transform;
				petalStripes.RemoveAt(i);
				if (oldStripe) {
					DestroyImmediate(oldStripe.gameObject);
				}
			}
		}
	}

	void UpdateLeaves () {
	  /*
	    public Sprite leafSprite;
		public int leafPairCount = 3;
		public int leafLowScale = 45;
		public int leafHighScale = 25;
		public int leafLowHeight = 90;
		public int leafHighHeight = 180;
	    public float leafAngle = 20.0f;
	    public Color leafColor = Vector4.one;
	  */

		UpdateLeafCount();


		if (leafPairs.Count > 0) {
			float deltaHeight = (leafHighHeight - leafLowHeight) / leafPairs.Count;
			float deltaScale = (leafHighScale - leafLowScale) / leafPairs.Count;
			currentLeafSwayOffset = 0.0f;

			for (int i = 0; i < leafPairs.Count; i++) {

				// Grab the leaf pair.
				Transform leafPair = leafPairs[i] as Transform;

				// Scale the leaves.
				float currentScale = leafLowScale + (deltaScale * i);
				leafPair.localScale = new Vector3(currentScale, currentScale, 1.0f);



				// Grab the two leaves.
				Transform leftLeaf = leafPair.Find("Left Leaf");
				Transform rightLeaf = leafPair.Find("Right Leaf");

				// Position them.
				float yPos = leafLowHeight + (deltaHeight * i);
				leafPair.localPosition = new Vector3(0.0f, yPos, 0.0f);
				float xPos = (stemWidth - 1) / 2 / currentScale;
				leftLeaf.localPosition = new Vector3(-xPos, 0.0f, 0.0f);
				rightLeaf.localPosition = new Vector3(xPos, 0.0f, 0.0f);

				// Rotate them.
				leftLeaf.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -leafAngle));
				rightLeaf.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, leafAngle));

				// Color them.
				SpriteRenderer leftLeafRenderer = leftLeaf.GetComponent<SpriteRenderer>();
				//leftLeafRenderer.material.color = leafColor;
				leftLeafRenderer.sharedMaterial.color = leafColor;
				SpriteRenderer rightLeafRenderer = rightLeaf.GetComponent<SpriteRenderer>();
				//rightLeafRenderer.material.color = leafColor;
				rightLeafRenderer.sharedMaterial.color = leafColor;

				// Sway them.
				SwayController swayController = leftLeaf.GetComponent<SwayController>();
				swayController.isLeftLeaf = true;
				swayController.restAngle = -leafAngle;
				swayController.frequency = leafSwayFrequency;
				swayController.amplitude = leafSwayAmplitude;
				swayController.swayOffset = currentLeafSwayOffset;

				swayController = rightLeaf.GetComponent<SwayController>();
				swayController.isLeftLeaf = false;
				swayController.restAngle = leafAngle;
				swayController.frequency = leafSwayFrequency;
				swayController.amplitude = leafSwayAmplitude;
				swayController.swayOffset = currentLeafSwayOffset;

				currentLeafSwayOffset += leafSwayOffset;





				/*
				// Scale the petal.
				petal.localScale = new Vector3(petalScale, petalScale, 1.0f);
				
				// Color the petal.
				SpriteRenderer petalRenerer = petal.GetComponent<SpriteRenderer>();
				petalRenerer.color = petalColor;
				
				// Rotate the petal.
				petal.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, (angle * Mathf.Rad2Deg) - 90));
				
				// Position the petal.
				petal.localPosition = new Vector3(Mathf.Cos(angle) * petalOffset,
				                                  Mathf.Sin(angle) * petalOffset,
				                                  0.0f);
				*/
			}
		}
	}

	void UpdateLeafCount () {
		if (leafPairs.Count < leafPairCount) {
			// Add leaves.
			for (int i = leafPairs.Count; i < leafPairCount; i++) {

				// Create the parent transform.
				GameObject leafPairObject = new GameObject();
				leafPairObject.transform.position = Vector3.zero;
				leafPairObject.name = "Leaf Pair";
				leafPairObject.transform.parent = transform;


				// Create the left leaf.
				Transform leftLeaf = Instantiate(leafTrans, 
				                                 Vector3.zero, 
				                                 Quaternion.Euler(new Vector3(0.0f, 0.0f, -leafAngle))) as Transform;
				leftLeaf.name = "Left Leaf";
				leftLeaf.transform.parent = leafPairObject.transform;
				SpriteRenderer leftLeafRenderer = leftLeaf.GetComponent<SpriteRenderer>();
				leftLeafRenderer.sprite = leafSprite;
				// Flip the left leaf over.
				leftLeaf.localScale = new Vector3(-1, 1, 1);


				// Create the right leaf.
				Transform rightLeaf = Instantiate(leafTrans, 
				                                  Vector3.zero, 
				                                  Quaternion.Euler(new Vector3(0.0f, 0.0f, leafAngle))) as Transform;
				rightLeaf.name = "Right Leaf";
				rightLeaf.transform.parent = leafPairObject.transform;
				SpriteRenderer rightLeafRenderer = rightLeaf.GetComponent<SpriteRenderer>();
				rightLeafRenderer.sprite = leafSprite;
				rightLeaf.localScale = new Vector3(1, 1, 1);

				// Add the pair to the list.
				leafPairs.Add(leafPairObject.transform);
			}
		}
		else if (leafPairs.Count > leafPairCount) {
			// Remove leaves.
			for (int i = leafPairs.Count - 1; leafPairs.Count > leafPairCount; i--) {
				Transform oldLeafPair = leafPairs[i] as Transform;
				leafPairs.RemoveAt(i);
				if (oldLeafPair) {
					DestroyImmediate(oldLeafPair.gameObject);
				}
			}
		}
	}
}













