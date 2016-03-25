using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public bool visible = true;

	private bool lookingAtPlayer = false;
	public Transform player;
	private Quaternion originalRot; 

	private Animator anim;
	public HeadLookController headlook;

	private Transform headTarget; 

	public Vector3 center; 

	void Start() {
		originalRot = this.transform.rotation;

		anim = GetComponent <Animator> ();
		center = this.transform.position;
	}

	void Update() {
		if (!lookingAtPlayer && visible) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, originalRot, Time.deltaTime);
		}

		if (headTarget && !visible) {
			FollowHead ();
		}
	}

	void FixedUpdate ()
	{
		//Animating();
	}

	public void LookAtPlayer(GameObject playerNew) {
		lookingAtPlayer = true;
		player = playerNew.transform;
		headlook.target = player.position;

		//RotateNPC (player.position - this.transform.position);
	
	}

	private void RotateNPC(Vector3 lookDir) {
		var rotation = Quaternion.LookRotation (lookDir, Vector3.up);
		rotation.x = 0f;
		rotation.z = 0f;
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, rotation, Time.deltaTime*2f);
	}

	public void LookAwayFromPlayer() {
		lookingAtPlayer = false;
	}


	public void IsPossessed (bool possessed, Transform head) {
		visible = !possessed;
		UpdateVisibility ();
		headTarget = head;
	}


	private void UpdateVisibility() {
		MeshRenderer[] renderers = this.gameObject.GetComponentsInChildren<MeshRenderer> ();
		foreach (MeshRenderer renderer in renderers) {
			renderer.enabled = visible;
		}
	}
	/*
	private void Animating ()
	{
		bool walking = 
		anim.SetBool("IsWalking",walking);
	}
	*/

	private void FollowHead() {
		Vector3 newPos = headTarget.position;
		newPos.y = 0f;
		this.transform.position = newPos;

		Quaternion newRot = headTarget.rotation;
		newRot.x = 0f;
		newRot.z = 0f;
		this.transform.rotation = newRot;
	}

}
