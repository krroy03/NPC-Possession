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

	public GameObject controllerModel;
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



	private bool triedToPickUp = false;
	void MoveObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) || Input.GetMouseButtonDown (0)) {
			
			//handAnimator.SetBool ("Fist", true);
			//handAnimator.SetBool ("Idle", false);
			if (!movingObj && currentObj) {
				// change values for soul and objects
				if (currentObj.tag == "Soul") {
					currentObj.GetComponent<SoulMovement> ().followHead = false;
					currentObj.GetComponent<MeshRenderer> ().enabled = true;
				} else if (currentObj.layer == 8) {
					currentObj.GetComponent<GravityBody> ().beingControlled = true;
					// also change colors of object and make hand model invisible. 
					currentObj.GetComponent<ObjectStats>().PickedUp();
					controllerModel.SetActive (false);
				}
				// indicate that we are picking up an object
				currentObj.transform.parent = this.transform;
				movingObj = true;
				triedToPickUp = true;

				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = true;

				}


			}
		}
	}


	private bool triedToRelease = false;
	void ReleaseObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) || Input.GetMouseButtonUp (0)) {
			
			//handAnimator.SetBool ("Idle", true);
			//handAnimator.SetBool ("Fist", false);
			if (currentObj && movingObj) {
				// change values for soul and objects
				if (currentObj.tag == "Soul") {

				} else if (currentObj.layer == 8) {
					currentObj.GetComponent<GravityBody> ().beingControlled = false;
					currentObj.GetComponent<GravityBody> ().planet = null;
					controllerModel.SetActive (true);
				}
				// indicate that we have released an object
				currentObj.transform.SetParent (null);
				triedToRelease = true;
				Rigidbody rg = currentObj.GetComponent<Rigidbody> (); 
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					rg.AddForce (speed);
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
		if (triedToPickUp) {
			triedToPickUp = false;
			//return;
		}

		if (triedToRelease) {
			triedToRelease = false;
			//return;
		}

		if (!movingObj && !currentObj) {
			if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {
				ObjectThrow objThrow = col.gameObject.GetComponent<ObjectThrow> ();
				if (!objThrow.touchingHand) {
					// change color of object we are touching
					ObjectStats objColor = col.gameObject.GetComponent<ObjectStats> ();
					objColor.TouchingHands ();
					// now change values to indicate we are touching it 
					if (left) {
						objThrow.hand = 1;
					} else {
						objThrow.hand = 2;
					}
					currentObj = col.gameObject;
					col.gameObject.GetComponent<ObjectThrow> ().touchingHand = true;
				}
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (triedToPickUp) {
			//return;
		}

		if (triedToRelease) {
			//return;
		}
		if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {
			ObjectThrow objThrow = col.gameObject.GetComponent<ObjectThrow> ();
			if (objThrow.touchingHand) {
				// change color of object we are not touching anymore
				ObjectStats objColor = col.gameObject.GetComponent<ObjectStats> ();
				objColor.NotTouchingHands ();
				// change values to indicate we are not touching it
				if (left && objThrow.hand == 1) {
					objThrow.touchingHand = false;
					objThrow.hand = 0;
					currentObj = null;
					movingObj = false;
				}
				if (!left && objThrow.hand == 2) {
					objThrow.touchingHand = false;
					objThrow.hand = 0;
					currentObj = null;
					movingObj = false;
				}
			}
		}
	}
}
