using UnityEngine;
using System.Collections;

public class SoulControls : MonoBehaviour {

	int deviceIndex;
	bool movingObj = false;
	GameObject currentObj;
	Animator handAnimator;
	Vector3 velocity;
	Vector3 prePos;
	Vector3 curPos;

	public bool left;
	// Use this for initialization
	void Start () {
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);

		handAnimator = gameObject.GetComponentInChildren<Animator> ();

		prePos = this.transform.position;
		velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		curPos = this.transform.position;
		velocity = 100f * (curPos - prePos) * (1 / Time.deltaTime);
		prePos = curPos;

		MoveObject ();
		ReleaseObject ();
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
					currentObj.GetComponent<SoulMovement> ().followHead = false;
					currentObj.GetComponent<MeshRenderer> ().enabled = true;
					movingObj = true;
					Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
					if (rg != null) {
						currentObj.GetComponent<Rigidbody> ().isKinematic = true;
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
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					rg.AddForce (velocity);
					// if soul doesn't hit npc, return it to player after 5 seconds
					StartCoroutine (currentObj.GetComponent<SoulMovement> ().ResetSoulPositionIfMiss (5f));
				}
				movingObj = false;
				currentObj = null;
			}
		}
	}


	void OnTriggerEnter (Collider col)
	{
		
		if (col.gameObject.tag == "Soul") {
			currentObj = col.gameObject;
		}

	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "Soul") {
		}
	}
}
