using UnityEngine;
using System.Collections;

public class HandControls : MonoBehaviour
{
	int deviceIndex;
	public bool movingObj = false;
	public GameObject currentObj;
	Animator handAnimator;
	Vector3 speed;
	Vector3 prePos;
	Vector3 curPos;

	public bool left;

	public SoulMovement soul;

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
		ReturnSoul ();
	}

	void FixedUpdate ()
	{

	}

	void MoveObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger))) {
			//handAnimator.SetBool ("Fist", true);
			//handAnimator.SetBool ("Idle", false);
			if (!movingObj && currentObj) {
				currentObj.transform.parent = this.transform;
				movingObj = true;
				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = true;
					if (currentObj.tag == "Soul") {
						currentObj.GetComponent<SoulMovement> ().followHead = false;
						currentObj.GetComponent<MeshRenderer> ().enabled = true;
					} else if (currentObj.layer == 8) {
						currentObj.GetComponent<GravityBody> ().beingControlled = true;
					}
				}
			}
		}
	}

	void ReleaseObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger))) {
			//handAnimator.SetBool ("Idle", true);
			//handAnimator.SetBool ("Fist", false);
			if (currentObj && movingObj) {
				currentObj.transform.SetParent (null);
				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					rg.AddForce (speed);
					if (currentObj.tag == "Soul") {

					} else if (currentObj.layer == 8) {
						currentObj.GetComponent<GravityBody> ().beingControlled = false;
						currentObj.GetComponent<GravityBody> ().planet = null;
					}
				}
				movingObj = false;
				currentObj = null;
			}
		}
	}

	// presss touchpad to return
	void ReturnSoul ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Touchpad))) {
			//handAnimator.SetBool ("Idle", true);
			//handAnimator.SetBool ("Fist", false);

			soul.ReturnSoul ();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (!movingObj && !currentObj) {
			if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {
				if (col.gameObject.GetComponent<ObjectThrow> ().touchingHand == 0) {
					MeshRenderer currentObjMeshRenderer = col.gameObject.GetComponent<MeshRenderer> ();
					if (left) {
						currentObjMeshRenderer.material.color = Color.blue;
					} else {
						currentObjMeshRenderer.material.color = Color.green;
					}

					currentObj = col.gameObject;
				}
				col.gameObject.GetComponent<ObjectThrow> ().touchingHand++;
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {

			if (col.gameObject.GetComponent<ObjectThrow> ().touchingHand > 0) {
				col.gameObject.GetComponent<ObjectThrow> ().touchingHand--; 
			}
			if (col.gameObject.GetComponent<ObjectThrow> ().touchingHand == 0)
				col.gameObject.GetComponent<MeshRenderer> ().material.color = Color.white;

			if (!movingObj && currentObj) {
				currentObj = null;
			}
		}
	}
}
