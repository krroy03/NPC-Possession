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
		TeleportHelper.gameObject.SetActive (true);
		DefendOrb.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCircleTimerVal(float val) {
		circleTimer.fillAmount = val;
	}

	public void TeleportedForFirstTime() {
		TeleportHelper.gameObject.SetActive (false);
		DefendOrb.gameObject.SetActive (false);
		OrbHP.gameObject.SetActive (true);
		CubePickUp.gameObject.SetActive (true);
	}

	public void PickedUpForFirstTime() {
		CubePickUp.gameObject.SetActive (false);
		CubeRelease.gameObject.SetActive (true);
	}

	public void ThrewForFirstTime() {
		CubeRelease.gameObject.SetActive (false);
		WaveStarting.gameObject.SetActive (true);
		WaveInfo.SetActive (true);
		waves.SetActive (true);
	}

	public void ShowScore() {
		OrbHP.gameObject.SetActive (false);
		Score.text += WaveManager.score;
		Score.gameObject.SetActive (true);
	}
}
