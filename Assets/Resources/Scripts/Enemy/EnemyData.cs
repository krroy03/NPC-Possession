using UnityEngine;
using System.Collections;

public class EnemyData : MonoBehaviour {

	// size can be 1-3, 
	public int size; 
	public GameColors.Colors color; 

	// hitpoints same as size, so bigger size means needs more cubes to destroy
	public int hitPoints;

	public SkinnedMeshRenderer bodyMesh;
	private Material thisMaterial;

	private int score = 0;
	// Use this for initialization
	void Awake () {
		thisMaterial = bodyMesh.material;
		RandomizeSize ();
		SetSize ();

		RandomizeColor ();
		SetColor ();

		hitPoints = size;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void CreateMaterial () {
		// Create a simple material asset
		//var material = new Material (Shader.Find("Diffuse"));
		//AssetDatabase.CreateAsset(material, "Assets/" + Selection.activeGameObject.name + ".mat");
	}

	private void RandomizeColor() {
		// randomize the color first 
		int randomColor = Random.Range(0,3);
		switch (randomColor) {
		case(0):
			color = GameColors.Colors.Red;
			break;
		case(1):
			color = GameColors.Colors.Blue;
			break;
		case(2):
			color = GameColors.Colors.Green;
			break;
		case(3):
			color = GameColors.Colors.Magenta;
			break;
		case(4):
			color = GameColors.Colors.Cyan;
			break;
		case(5):
			color = GameColors.Colors.Yellow;
			break;
		}
	}


	private void SetColor() {
		switch (color) {
		case(GameColors.Colors.Red):
			thisMaterial.color = Color.red;
			break;
		case(GameColors.Colors.Blue):
			thisMaterial.color = Color.blue;
			break;
		case(GameColors.Colors.Green):
			thisMaterial.color = Color.green;
			break;
		case(GameColors.Colors.Magenta):
			thisMaterial.color = Color.magenta;
			break;
		case(GameColors.Colors.Cyan):
			thisMaterial.color = Color.cyan;
			break;
		case(GameColors.Colors.Yellow):
			//thisMaterial.color = new Color (1, 1, 0, 1);
			thisMaterial.color = Color.yellow;
			break;
		}

		foreach (Material material in bodyMesh.materials) {
			material.color = thisMaterial.color;

		}
	}


	private void RandomizeSize() {
		size = Random.Range (1, 4);
		score = size; 
	}
	private void SetSize() {
		this.transform.localScale *= size;
	}

	private void DecreaseSize() {
		this.transform.localScale /= (size + 1);
		this.transform.localScale *= size;
	}

	private void CheckForDeath() {
		if (hitPoints <= 0) {
			Dead ();
		}
	}

	private void Dead() {
		WaveManager.score += score;
		GameObject.Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.layer == 8) {
			if (color == col.gameObject.GetComponent<ObjectStats> ().objColor) {
				size--;
				hitPoints--;
				CheckForDeath ();
				// if not dead yet, decrease size
				DecreaseSize ();
			}
			GameObject.Destroy (col.gameObject);
		}
	}
}
