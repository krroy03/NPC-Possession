using UnityEngine;
using System.Collections;

public class NPCCollision : MonoBehaviour {

	GameObject currentNPC;

	void Start ()
	{
		//get myself first
		currentNPC = this.transform.parent.gameObject;
	}

	// When NPC collides with a cube
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.layer == 8) {

			
			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();
			PossessionMechanic pm = GameObject.Find ("CamContainer").GetComponent<PossessionMechanic> ();

			//teleport to the NPC get hit if NPC I want to transfer is not myself
			if (obj.npc && (obj.npc != currentNPC)) {
				
				pm.PossessNewNPC (currentNPC);
				currentNPC =  this.transform.parent.gameObject;
			}
		}
	}

}
