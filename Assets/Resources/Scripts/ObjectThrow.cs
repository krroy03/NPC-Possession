using UnityEngine;
using System.Collections;

public class ObjectThrow : MonoBehaviour {

	private GameObject cam = null;
	private Transform npcTransform = null;
	public GameObject npc = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void FixedUpdate ()
	{
		if (GetComponent<Rigidbody> ().isKinematic == true) {
			cam = GameObject.Find("CamContainer");
			npcTransform = cam.transform.GetChild(cam.transform.childCount - 1);
			npc = npcTransform.gameObject;
		}

	}



}
