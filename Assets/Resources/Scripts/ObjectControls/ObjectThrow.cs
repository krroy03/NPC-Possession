using UnityEngine;
using System.Collections;

public class ObjectThrow : MonoBehaviour {

	private GameObject cam = null;
	private Transform npcTransform = null;
	public GameObject npc = null;
	public Transform target = null;
	public float speed = 0;
	public bool touchingHand = false; 
	public int hand = 0;

	void Start() {

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

	void Update ()
	{
		if (speed != 0) {
			Debug.Log (target.gameObject);
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
		}
	}

}
