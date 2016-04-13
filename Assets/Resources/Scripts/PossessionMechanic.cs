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
	public bool finishedShifting = false; 
	public bool reachedPlanet = false; 
	public int shiftCount = 0;


	private float shiftStopDist = 0.01f;


	// variables for looking at an NPC
	public float timeNeeded;
	private float fillAmount = 0.05f;
	private float lookTime = 0f;

	private TransitionEffect transition;


	// Use this for initialization
	void Start () {

		transition = this.gameObject.GetComponent<TransitionEffect> ();
		if (currentNPC) {
			parentNPCToCam (currentNPC);
		}
		npcList = GameObject.Find ("NPCs").transform;
		finishedShifting = true;
	}
	
	// Update is called once per frame
	void Update () {
		LookAtNPC ();

		if (shifting) {
			ShiftTeleportToNewPos (currentNPC.transform.position);
		}

	}


	private GameObject lookingAt; 

	void LookAtNPC() {
		
		if (!shifting) {
			// only cast ray when not shifting 
			RaycastHit[] hits;
			hits = Physics.RaycastAll (head.transform.position, head.transform.forward, 100f);
			bool hitNPC = false; 
			for (int i = 0; i < hits.Length; i++) {
				if (hits [i].collider.tag == "NPC") {
					// we hit an NPC
					hitNPC = true;

					lookingAt = hits[i].collider.gameObject;
					lookTime += fillAmount;
					UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);

					if (lookTime >= timeNeeded) {

						// reset circle
						lookTime = 0f;
						UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);

						// we can possess npc
						PossessNewNPC (hits[i].collider.gameObject);
					}

					if (lookTime >= timeNeeded / 3.0f) {
						// we got npc attention
						lookingAt.GetComponent<NPC> ().LookAtPlayer (this.gameObject);
					}
					// after we find npc, don't need to traverse rest of loop
					break;
				} 
			}

			// reset UI recticle
			if (!hitNPC || hits.Length == 0) {
				lookTime = 0f;
				UIManager.Instance.SetCircleTimerVal (lookTime / timeNeeded);
				if (lookingAt) {
					lookingAt.GetComponent<NPC> ().LookAwayFromPlayer ();
					lookingAt = null;
				}
			}
		}
	}

	private Vector3 oldCenter = Vector3.zero;
	private Vector3 newCenter = Vector3.zero;

	/**
	 * possesses a new NPC.
	 * - Has to move to that npc. Unparent current NPC from cam, make it visible again
	 * - use the shift teleport movement to leave current npc body and move cam to new npc
	 * - have to move entire tracking space
	 * - parent new npc to cam 
	 * - make it not visible anymore
	 * */
	public void PossessNewNPC(GameObject npc) {
		

		if (currentNPC) {
			// unpossess npc
			unparentNPCFromCam (currentNPC);
			// reset old npc position
			currentNPC.transform.position = oldCenter;
			oldCenter = currentNPC.transform.GetComponent<NPC> ().center;
		} else {
			oldCenter = this.transform.position;
		}
		finishedShifting = false;
		shifting = true;
		currentNPC = npc;
		newCenter = GetNewCenterOfTrackingSpace (currentNPC.transform.position);

		transition.StartFX ();
		moveSound.Play ();
		shiftCount++;
		// teleported for the first time 
		// so remove teleport ui 
		if (shiftCount == 1) {
			UIManager.Instance.TeleportHelper.enabled = false;
			UIManager.Instance.DefendOrb.enabled = false;
		}
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

	public void ShiftTeleportToNewPos(Vector3 pos) {
		Vector3 dir = newCenter - this.transform.position;
		//dir.y = 0f;

		if (dir.magnitude > shiftStopDist) {
			// still moving to our destination
			this.transform.Translate (dir * shiftSpeed * Time.deltaTime, Space.World);
		} else {
			// we have reached our destination 
			shifting = false; 
			transition.EndFX ();

			RecenterTrackingSpace (currentNPC.transform.position);
			// Either turn cam to face old npc 
			// OR turn cam to face current NPC pov

			TurnCamToFaceDir (oldCenter - pos);
			//TurnCamToFaceDir (currentNPC.transform.forward);
			parentNPCToCam (currentNPC);
			finishedShifting = true;
			reachedPlanet = true;
		}
	}


	// gets the new center based on npc we are moving to
	// take local position of our head transform
	// subtract that from the dir vector
	private Vector3 GetNewCenterOfTrackingSpace(Vector3 pos) {
		Vector3 newCenter = pos;
		
		Vector3 temp = Vector3.zero;
		temp.x = (oldCenter.x - head.position.x);
		temp.z = (oldCenter.z - head.position.z);

		newCenter = newCenter + temp;
		return newCenter;
	}


	private void RecenterTrackingSpace(Vector3 pos) {
		this.transform.position = pos;
		newCenter = pos;
	}

	// turns cam to face the old npc we came from
	private void TurnCamToFaceDir(Vector3 lookDir) {

		var rotation = Quaternion.LookRotation (lookDir, Vector3.up);
		// change x and z rotation values to 0 because tracking space shouldn't rotate on those axes. 
		rotation.x = 0f;
		rotation.z = 0f;

		// rotate tracking space first
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 1f);

		// also need to take into account the rotation of the headset currently. 
		// add that to the rotation of the tracking space. 
		float yOffset = 360f - head.localRotation.eulerAngles.y;
		rotation.eulerAngles = new Vector3 (0f, yOffset + rotation.eulerAngles.y, 0f);
		
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 1f);
	
	}


}
