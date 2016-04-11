using UnityEngine;
using System.Collections;

public class ObjectColor : MonoBehaviour {

	public enum objColor
	{
		Red,
		Blue,
		Green,
	}
	private Color touchingColor = Color.cyan;
	public Color originalColor; 
	private Material thisMaterial; 
	public Color currentColor; 

	// Use this for initialization
	void Start () {
		thisMaterial = GetComponent<MeshRenderer> ().material;
		thisMaterial.color = originalColor;
		currentColor = originalColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TouchingHands() {
		ChangeColor (touchingColor);
	}

	public void PickedUp() {
		ChangeColor (originalColor);
	}

	public void NotTouchingHands() {
		ChangeColor (originalColor);
	}

	private void ChangeColor(Color newColor) {
		thisMaterial.color = newColor;
		currentColor = newColor;
	}
}
