using UnityEngine;
using System.Collections;

public class ObjectStats : MonoBehaviour {


	public GameColors.Colors objColor;
	private Color touchingColor = Color.gray;
	private Color originalColor; 
	private Material thisMaterial; 
	public Color currentColor; 

	// Use this for initialization
	void Start () {
		thisMaterial = GetComponent<MeshRenderer> ().material;
		switch (objColor) {
		case(GameColors.Colors.Red):
			originalColor = Color.red;
			break;
		case(GameColors.Colors.Blue):
			originalColor = Color.blue;
			break;
		case(GameColors.Colors.Green):
			originalColor = Color.green;
			break;
		case(GameColors.Colors.Magenta):
			originalColor = Color.magenta;
			break;
		case(GameColors.Colors.Cyan):
			originalColor = Color.cyan;
			break;
		case(GameColors.Colors.Yellow):
			//originalColor = new Color (1, 1, 0, 1);
			originalColor = Color.yellow;
			break;
		}

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
