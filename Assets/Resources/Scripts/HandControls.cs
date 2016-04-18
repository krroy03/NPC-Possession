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
	Transform dragonTransform = null;
	public bool left;

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
		// throw object velocity 
		curPos = this.transform.position;
		speed = (curPos - prePos) / Time.deltaTime;
		MoveObject ();
		ReleaseObject ();
		prePos = curPos;
	}

	void FixedUpdate() {
		if (left)
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		else
			deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);
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
		int mask = 1 << 11;
		foundHit = Physics.SphereCast(transform.position,5, (curPos - prePos).normalized ,out hit, 100, mask);
		if (foundHit) {
			// if the ray hits a dragon
				dragonTransform = hit.collider.gameObject.transform;
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
				}
				// indicate that we are picking up an object
				currentObj.transform.parent = this.transform;
				movingObj = true;
				Rigidbody rg = currentObj.GetComponent<Rigidbody> ();
				if (rg != null) {
					currentObj.GetComponent<Rigidbody> ().isKinematic = true;
				}
				if (UIManager.firstTimePickUp) {
					UIManager.Instance.PickedUpForFirstTime ();
					UIManager.firstTimePickUp = false;
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
				}
				// indicate that we have released an object
				currentObj.transform.SetParent (null);
				Rigidbody rg = currentObj.GetComponent<Rigidbody> (); 
				if (rg != null) {
					// update the dragon Position
					detectDragon();
					currentObj.GetComponent<Rigidbody> ().isKinematic = false;
					//auto target to dragon
					if (dragonTransform != null) {
						currentObj.GetComponent<ObjectThrow> ().target = dragonTransform;
						currentObj.GetComponent<ObjectThrow> ().speed = speed.magnitude*2f;
						//reset 
						dragonTransform = null;
					} 
					//did not detect the dragon 
					else 
					{
						rg.AddForce(speed*100f);
					}

				}
				movingObj = false;
				currentObj = null;

				if (UIManager.firstTimeRelease) {
					UIManager.Instance.ThrewForFirstTime ();
					UIManager.firstTimeRelease = false;
				}
			}
		}
	}



	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.layer == 8) {
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
