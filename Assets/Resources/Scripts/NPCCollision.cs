using UnityEngine;
using System.Collections;

public class NPCCollision : MonoBehaviour {
	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.layer == 8) {
			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();
			PossessionMechanic pm = GameObject.Find("CamContainer").GetComponent<PossessionMechanic>();
			//teleport to the NPC get hit
			if(obj.npc)
			pm.ShiftTeleportToNewPos (obj.npc.transform.position);
		}
	}

}
