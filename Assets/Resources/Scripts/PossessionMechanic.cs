using UnityEngine;
using System.Collections;

public class PossessionMechanic : MonoBehaviour {

	public GameObject currentNPC; 

	public Transform head;

	private Transform npcList; 

	// variables for shift teleporting 
	public AudioSource moveSound;
	public float shiftSpeed = 1f; 
	private bool shifting = false; 
	private float shiftStopDist = 0.01f;
	private Vector3 oldPos = Vector3.zero;

	// variables for looking at an NPC
	public float timeNeeded;
	private float fillAmount = 0.02f;
	private float lookTime = 0f;

	private TransitionEffect transition;


	// Use this for initialization
	void Start () {

		transition = this.gameObject.GetComponent<TransitionEffect> ();
		parentNPCToCam (currentNPC);
		npcList = GameObject.Find ("NPCs").transform;
	}
	
	// Update is called once per frame
	void Update () {
		LookAtNPC ();

		if (shifting) {
			ShiftTeleportToNewPos (currentNPC.transform.position);
		}
		if (Input.GetMouseButton (0)) {
			PossessNewNPC (GetRandomNPC());
		}
	}


	private GameObject lookingAt; 

	void LookAtNPC() {
		
		if (!shifting) {
			// only cast ray when not shifting 
			RaycastHit hit;
			Debug.DrawRay (head.transform.position, head.transform.forward, Color.green);
			if (Physics.Raycast (head.transform.position, head.transform.forward, out hit, 100f)) {
				if (hit.collider.tag == "NPC") {
					lookingAt = hit.collider.gameObject;
					lookTime += fillAmount;
					//UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);

					if (lookTime >= timeNeeded) {

						// reset circle
						lookTime = 0f;
						//UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);

						// we can possess npc
						PossessNewNPC (hit.collider.gameObject);
					}

					if (lookTime >= timeNeeded / 3.0f) {
						// we got npc attention
						lookingAt.GetComponent<NPC> ().LookAtPlayer (currentNPC);
					}
				} else {
					// if we hit another collider, reset too
					lookTime = 0f;

					//UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);
					if (lookingAt) {
						Debug.Log("gets here");
						lookingAt.GetComponent<NPC> ().LookAwayFromPlayer ();
						lookingAt = null;
					}
				}
			} else {
				// if we look away from trigger, then reset time 

				lookTime = 0f;
				//UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);
				if (lookingAt) {
					lookingAt.GetComponent<NPC> ().LookAwayFromPlayer ();
					lookingAt = null;
				}
			}
		}
	}


	/**
	 * possesses a new NPC.
	 * - Has to move to that npc. Unparent current NPC from cam, make it visible again
	 * - use the shift teleport movement to leave current npc body and move cam to new npc
	 * - have to move entire tracking space
	 * - parent new npc to cam 
	 * - make it not visible anymore
	 * */
	public void PossessNewNPC(GameObject npc) {

		oldPos = head.transform.position;

		if (currentNPC) {
			unparentNPCFromCam (currentNPC);
		}

		shifting = true;
		currentNPC = npc;

		transition.StartFX ();
		moveSound.Play ();

	}


	private void parentNPCToCam(GameObject npc) {
		npc.transform.SetParent (this.transform);
		npc.GetComponent<NPC> ().IsPossessed (true, head);
	}

	private void unparentNPCFromCam(GameObject npc) {
		npc.transform.SetParent (npcList);
		npc.GetComponent<NPC> ().IsPossessed (false, null);
	}





	/*
	 * first, we need to get the new center of tracking space, relative to current location of player and the npc he is travelling to
	 * Then, move the tracking space such that the player is still in the same position relative to tracking space. 
	 * */

	private void ShiftTeleportToNewPos(Vector3 pos) {
		Vector3 newCenter = GetNewCenterOfTrackingSpace (pos);
		Vector3 dir = newCenter - this.transform.position;
		dir.y = 0f;

		if (dir.magnitude > shiftStopDist) {
			// still moving to our destination
			this.transform.Translate (dir * shiftSpeed * Time.deltaTime, Space.World);
		} else {
			// we have reached our destination 
			shifting = false; 
			transition.EndFX ();

			// Either turn cam to face old npc 
			// OR turn cam to face current NPC pov

			TurnCamToFaceDir (oldPos - pos);
			//TurnCamToFaceDir (currentNPC.transform.forward);
			parentNPCToCam (currentNPC);
		}
	}


	// gets the new center based on npc we are moving to
	// take local position of our head transform
	// subtract that from the dir vector
	private Vector3 GetNewCenterOfTrackingSpace(Vector3 pos) {
		Vector3 newCenter = pos;
		newCenter = newCenter - head.localPosition;
		return newCenter;
	}



	// turns cam to face the old npc we came from
	private void TurnCamToFaceDir(Vector3 lookDir) {

		var rotation = Quaternion.LookRotation (lookDir, Vector3.up);
		// change x and z rotation values to 0 because tracking space shouldn't rotate on those axes. 
		rotation.x = 0f;
		rotation.z = 0f;

		Vector3 temp = transform.position;
		head.rotation = Quaternion.identity;
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 1f);
		// now move the tracking space again to make up for the rotation shift in position of the head
		temp.x += head.localPosition.x; 
		temp.z += head.localPosition.z; 
		transform.position = temp;

		/*
		Vector3 temp = head.position;
		head.parent.position = temp;
		head.localPosition = Vector3.zero;

		// now rotate headContainer
		head.parent.rotation = Quaternion.Slerp(head.parent.rotation, rotation, 1f);
		head.localRotation = Quaternion.identity;

*/
	}



	// used for debugging without Vive
	private GameObject GetRandomNPC(){
		int maxRange = npcList.childCount;
		bool foundChild = false; 
		int count = 0;
		GameObject child = null; 

		while (!foundChild && count < 5) {
			child = npcList.GetChild (Random.Range (0, maxRange)).gameObject;
			if (child.GetComponent<NPC> ().visible) {
				foundChild = true;
			}
			count++;
		}

		return child;

	}

	// some helper methods for future
	private Vector3 RotatePointAroundPivot(Vector3 point,Vector3 pivot,Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot 
		dir = Quaternion.Euler(angles) * dir; // rotate it 
		point = dir + pivot; // calculated rotated point;
		return point; 
	}



}
