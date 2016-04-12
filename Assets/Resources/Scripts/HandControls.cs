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
	Vector3 dragonPosition = Vector3.zero;
	public bool left;

	public GameObject controllerModel;
	// Use this for initialization
	void Start ()
	{	
		// check the left and right 
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);
		// hand animator for grip and release 
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
		// throw object velocity 
		curPos = this.transform.position;
		detectDragon();
		if (dragonPosition != Vector3.zero) {
			speed = 100f * (dragonPosition - prePos).normalized * (1 / Time.deltaTime); 
			Debug.Log("enter");
		} 
		else {
			speed = 100f * (curPos - prePos).normalized * (1 / Time.deltaTime);
		}
		prePos = curPos;
		MoveObject ();
		ReleaseObject ();
	}

	void OnTriggerEnter (Collider col)
	{	
		//not moving object and no object in hands
		if (!movingObj && !currentObj) {
			// isObject
			if (col.gameObject.layer == 8) {
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

	void detectDragon ()
	{
		bool foundHit = false;
		RaycastHit hit;
		//cast a sphereray
		foundHit = Physics.SphereCast(transform.position, 1, transform.forward,out hit, 10);
		if (foundHit) {
			// if the ray hits a dragon
			if (hit.collider.GetComponent<GameObject> ().layer == 11) {
				Vector3 dragonPosition = hit.collider.GetComponent<GameObject>().transform.position;
			}
		}
	}

	void MoveObject ()
	{	
		// hasDevice && 
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) || Input.GetMouseButtonDown (0)) {
			
			//handAnimator.SetBool ("Fist", true);
			//handAnimator.SetBool ("Idle", false);
			if (!movingObj && currentObj) {
				// change values for soul and objects
				if (currentObj.layer == 8) {
					currentObj.GetComponent<GravityBody> ().beingControlled = true;
					// also change colors of object and make hand model invisible. 
					currentObj.GetComponent<ObjectStats> ().PickedUp ();
					controllerModel.SetActive (false);
				}
				// indicate that we are picking up an object
				currentObj.transform.parent = this.transform;
				movingObj = true;
				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = true;
				}
			}
		}
	}

	void ReleaseObject ()
	{
		if ((deviceIndex != -1 && SteamVR_Controller.Input (deviceIndex).GetPressUp (SteamVR_Controller.ButtonMask.Trigger)) || Input.GetMouseButtonUp (0)) {
			
			//handAnimator.SetBool ("Idle", true);
			//handAnimator.SetBool ("Fist", false);
			if (currentObj && movingObj) {
				// change values for soul and objects
				if (currentObj.layer == 8) {
					currentObj.GetComponent<GravityBody> ().beingControlled = false;
					currentObj.GetComponent<GravityBody> ().planet = null;
					controllerModel.SetActive (true);
				}
				// indicate that we have released an object
				currentObj.transform.SetParent (null);
				Rigidbody rg = currentObj.GetComponent<Rigidbody> (); 
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					rg.AddForce (speed);
					//reset 
					dragonPosition = Vector3.zero;
				}
				movingObj = false;
				currentObj = null;
			}
		}
	}



	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.layer == 8 || col.gameObject.tag == "Soul") {
			ObjectThrow objThrow = col.gameObject.GetComponent<ObjectThrow> ();
			if (objThrow.touchingHand) {
				
				// change values to indicate we are not touching it
				if (left && objThrow.hand == 1) {
					if (col.gameObject.GetComponent<Rigidbody> ().isKinematic) {
						return;
					}
					// change color of object we are not touching anymore
					ObjectStats objColor = col.gameObject.GetComponent<ObjectStats> ();
					objColor.NotTouchingHands ();
					objThrow.touchingHand = false;
					objThrow.hand = 0;
					currentObj = null;
					movingObj = false;
				}
				if (!left && objThrow.hand == 2) {
					if (col.gameObject.GetComponent<Rigidbody> ().isKinematic) {
						return;
					}
					// change color of object we are not touching anymore
					ObjectStats objColor = col.gameObject.GetComponent<ObjectStats> ();
					objColor.NotTouchingHands ();
					objThrow.touchingHand = false;
					objThrow.hand = 0;
					currentObj = null;
					movingObj = false;
				}
			}
		}
	}
}
