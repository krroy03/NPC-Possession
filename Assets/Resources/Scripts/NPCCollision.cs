using UnityEngine;
using System.Collections;

public class NPCCollision : MonoBehaviour {

	GameObject currentNPC;

	void Start ()
	{
		//get myself first
		currentNPC = this.gameObject;
	}

	// When NPC collides with a cube
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.layer == 8) {

			
			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();
			PossessionMechanic pm = GameObject.Find ("CamContainer").GetComponent<PossessionMechanic> ();
			Debug.Log ("enter" + obj.npc);
			//teleport to the NPC get hit if NPC I want to transfer is not myself
			if (obj.npc && (obj.npc != currentNPC)) {
				Debug.Log("xixixixixi");
				pm.PossessNewNPC (this.gameObject);
				currentNPC = this.gameObject;
			}
		}
	}

}
