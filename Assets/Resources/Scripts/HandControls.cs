using UnityEngine;
using System.Collections;

public class HandControls : MonoBehaviour {
	private int deviceIndex; 
	private bool movingObj = false;
	private Transform currentObj;

	public bool left;

	// Use this for initialization
	void Start () {
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MoveObject() {

		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger))) {
			if (!movingObj) {
				Debug.Log("Trigger");
				if (currentObj) {
					currentObj.parent.SetParent (this.transform);
					movingObj = true;

				}
					
			}

		}
	}

	void ReleaseObject() {
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger))) {
			if (currentObj && movingObj) {
				currentObj.parent.SetParent (null);
				movingObj = false;
				currentObj = null;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (!movingObj) {
			if (col.gameObject.layer == 8) {
				currentObj = col.gameObject.transform;
			
			}
		}
	}

	void OnTriggerExit(Collider col) {
		if (!movingObj) {
			if (col.gameObject.layer == 8) {
				currentObj = null;

			}
		}
	}
}
