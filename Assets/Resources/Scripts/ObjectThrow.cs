using UnityEngine;
using System.Collections;

public class ObjectThrow : MonoBehaviour {

	private GameObject cam = null;
	private Transform npcTransform = null;
	public GameObject npc = null;

	private GravityBody gBody; 


	void Start() {
		gBody = this.gameObject.GetComponent<GravityBody> ();

		cam = GameObject.Find("CamContainer");
	}

	void FixedUpdate ()
	{	
		// when move the object, set the current player

		if (GetComponent<Rigidbody> ().isKinematic == true) {
			npcTransform = cam.transform.GetChild(cam.transform.childCount - 1);
			npc = npcTransform.gameObject;
		}

	}




}
