using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {
	public Text myText;

	// Use this for initialization
	void Start () {
		SetUI ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetUI() {
		myText.gameObject.GetComponent<RotateToLookAtCam> ().m_Camera = Camera.main.transform;
		EnemyData data = this.GetComponent<EnemyData> ();
		switch (data.color) {
		case(GameColors.Colors.Green):
			myText.text = "Hit With Green Cubes";
			break;
		case(GameColors.Colors.Blue):
			myText.text = "Hit With Blue Cubes";
			break;
		case(GameColors.Colors.Red):
			myText.text = "Hit With Red Cubes";
			break;

		}
	}
}
