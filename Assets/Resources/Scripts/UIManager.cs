using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

	public static UIManager mInstance; 

	public static UIManager Instance {
		get {
			if (mInstance)
				return mInstance;
			else {
				mInstance = GameObject.Find ("GameUI").GetComponent<UIManager> ();
				return mInstance;
			}
		}
	}



	public Image circleTimer; 

	public GameObject WaveInfo; 

	public Text TeleportHelper; 

	public Text DefendOrb; 

	public Text OrbHP;

	public Text CubePickUp;

	public Text CubeRelease; 

	public Text WaveStarting;

	public Text Score; 

	public GameObject waves; 

	// Use this for initialization
	void Start () {
		SetCircleTimerVal (0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCircleTimerVal(float val) {
		circleTimer.fillAmount = val;
	}

	public void TeleportedForFirstTime() {
		TeleportHelper.enabled = false;
		DefendOrb.enabled = false;
		CubePickUp.enabled = true;
	}

	public void PickedUpForFirstTime() {
		CubePickUp.enabled = false;
		CubeRelease.enabled = true;
	}

	public void ThrewForFirstTime() {
		CubeRelease.enabled = false;
		WaveStarting.enabled = false;
		waves.SetActive (true);
	}

	public void ShowScore() {
		OrbHP.enabled = false;
		Score.text += WaveManager.score;
		Score.enabled = true;
	}
}
