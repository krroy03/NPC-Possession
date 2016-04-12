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
				mInstance = GameObject.Find ("PlayerUI").GetComponent<UIManager> ();
				return mInstance;
			}
		}
	}



	public Image circleTimer; 

	public GameObject WaveInfo; 

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
}
