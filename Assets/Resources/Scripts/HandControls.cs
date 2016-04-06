using UnityEngine;
using System.Collections;

public class HandControls : MonoBehaviour
{
	int deviceIndex;
	bool movingObj = false;
	GameObject currentObj;
	Animator handAnimator;
	Vector3 speed;
	Vector3 prePos;
	Vector3 curPos;

	public bool left;

	// Use this for initialization
	void Start ()
	{
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);

		handAnimator = gameObject.GetComponentInChildren<Animator> ();

		prePos = this.transform.position;
		speed = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (left) 
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);
		
		curPos = this.transform.position;
		speed = 100f * (curPos - prePos) * (1 / Time.deltaTime);
		prePos = curPos;

		MoveObject ();
		ReleaseObject ();
	}

	void FixedUpdate ()
	{

	}

	void MoveObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger))) {
			handAnimator.SetBool ("Fist", true);
			handAnimator.SetBool ("Idle", false);
			if (!movingObj) {
				if (currentObj != null) {
					currentObj.transform.parent = this.transform;
					movingObj = true;
					Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
					if (rg != null) {
						currentObj.GetComponent<Rigidbody> ().isKinematic = true;
						if (currentObj.tag == "Soul") {
							currentObj.GetComponent<SoulMovement> ().followHead = false;
							currentObj.GetComponent<MeshRenderer> ().enabled = true;
						}
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
					rg.AddForce (speed);
					if (currentObj.tag == "Soul") {
						// if soul doesn't hit npc, return it to player after 5 seconds
						StartCoroutine (currentObj.GetComponent<SoulMovement> ().ResetSoulPositionIfMiss (5f));
					} else if (currentObj.layer == 8 ) {
						currentObj.GetComponent<GravityBody> ().planet = null;
					}
				}
				movingObj = false;
				currentObj = null;
			}
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (!movingObj) {
			if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {
				currentObj = col.gameObject;
				MeshRenderer currentObjMeshRenderer = currentObj.GetComponent<MeshRenderer> ();
				currentObjMeshRenderer.material.color = Color.blue;
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.layer == 8  || col.gameObject.tag == "Soul") {
			col.gameObject.GetComponent<MeshRenderer> ().material.color = Color.white;
		}
	}
}
