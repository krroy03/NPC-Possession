using UnityEngine;
using System.Collections;

public class HandControls : MonoBehaviour {
	private int deviceIndex; 
	private bool movingObj = false;
	private GameObject currentObj;
	Animator handAnimator;
	Vector3 speed;
	Vector3 prePos;

	public bool left;

	// Use this for initialization
	void Start () {
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

		handAnimator = gameObject.GetComponentInChildren<Animator>();

		prePos = transform.position;
		speed = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        MoveObject();
        ReleaseObject();
	}

	void FixedUpdate () {
		Vector3 curPos = transform.position;
		speed = (curPos - prePos) * (1 / Time.fixedDeltaTime);
		prePos = curPos;
	}

	void MoveObject ()
	{
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);
		//Debug.Log("deviceIndex = " + deviceIndex);
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger))) {
			handAnimator.SetBool ("Fist", true);
			handAnimator.SetBool ("Idle", false);
			if (!movingObj) {
				if (currentObj != null) {
					currentObj.transform.parent = this.transform;
					movingObj = true;
					Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
					if (rg != null) {
						currentObj.GetComponent<Rigidbody>().isKinematic = true;
                    	currentObj.GetComponent<Rigidbody>().useGravity = false;
					}
                    
				}
					
			}

		}
	}

	void ReleaseObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger))) {
			handAnimator.SetBool ("Idle", true);
			handAnimator.SetBool ("Fist", false);
			if (currentObj && movingObj) {
				currentObj.transform.SetParent (null);
				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					rg.AddForce(speed);
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					currentObj.GetComponent<Rigidbody> ().useGravity = true;
				}
				movingObj = false;
				currentObj = null;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (!movingObj) {
			if (col.gameObject.layer == 8) {
				currentObj = col.gameObject;
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
