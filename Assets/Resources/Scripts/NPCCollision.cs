using UnityEngine;
using System.Collections;

public class NPCCollision : MonoBehaviour {

	GameObject currentNPC;
	PossessionMechanic pm;
	void Start ()
	{
		//get myself first
		currentNPC = this.transform.parent.gameObject;

		pm = GameObject.Find ("CamContainer").GetComponent<PossessionMechanic> ();
	}

	// When NPC collides with a cube
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Soul") {

			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();

			//teleport to the NPC get hit if NPC I want to transfer is not myself
			// teleport to the NPC get hit if NPC I am transferring to is not myself 
			// and also if obj is moving when it hits npc
		
			if (obj.GetComponent<Rigidbody> ().velocity.magnitude > 0.01f && !obj.GetComponent<Rigidbody> ().isKinematic && obj.npc && (obj.npc != currentNPC))   
			{
				obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				pm.PossessNewNPC (currentNPC);
				currentNPC =  this.transform.parent.gameObject;
				col.gameObject.GetComponent<SoulMovement> ().followHead = true;
				col.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}

}
