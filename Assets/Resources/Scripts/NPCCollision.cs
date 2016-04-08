using UnityEngine;
using System.Collections;

public class NPCCollision : MonoBehaviour {

	GameObject NPC;
	PossessionMechanic pm;
	Vector3 start;
	Vector3 end;
	GameObject soul = null;
	bool isCollide = false;

	void Start ()
	{
		//get myself first
		NPC = this.transform.parent.gameObject;
		pm = GameObject.Find ("CamContainer").GetComponent<PossessionMechanic> ();
	}

	// When NPC collides with a cube
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Soul") {
			soul = col.gameObject;
			Debug.Log(soul);
			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();

			// teleport to the NPC get hit if NPC I want to transfer is not myself
			// teleport to the NPC get hit if NPC I am transferring to is not myself 
			// and also if obj is moving when it hits npc
			//Test
			//obj.npc = GameObject.Find ("Roy");

			if (obj.GetComponent<Rigidbody> ().velocity.magnitude > 0.01f && !obj.GetComponent<Rigidbody> ().isKinematic && obj.npc && (obj.npc != NPC))   
			{
				isCollide = true;
				end = obj.npc.transform.position;

				/*
				obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				pm.PossessNewNPC (NPC);

				col.gameObject.GetComponent<SoulMovement> ().followHead = true;
				col.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				*/
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		isCollide = false;
	}

	void Update ()
	{
		if (isCollide) {
			soul.GetComponent<Rigidbody> ().AddForce (.1f * (end - soul.transform.position));
			float distance = Vector3.Distance (end, soul.transform.position);
			Debug.Log("enter");
			Debug.Log("distance" + distance);
			if(distance < 1f) {
				Debug.Log("lalalal");
				ObjectThrow obj = soul.GetComponent<ObjectThrow> ();
				obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				pm.PossessNewNPC (NPC);


				isCollide = false;

				soul.GetComponent<SoulMovement> ().followHead = true;
				soul.GetComponent<MeshRenderer> ().enabled = false;
			}
		}

	}
}