using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public bool visible = true;

	private bool lookingAtPlayer = false;
	public Transform player;
	private Quaternion originalRot; 

	private Animator anim;
	public HeadLookController headlook;

	void Start() {
		originalRot = this.transform.rotation;

		anim = GetComponent <Animator> ();
	}

	void Update() {
		if (!lookingAtPlayer) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, originalRot, Time.deltaTime);
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
		/*
		var rotation = Quaternion.LookRotation (player.position - this.transform.position, Vector3.up);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, rotation, Time.deltaTime*2f);
		*/
	}


	public void LookAwayFromPlayer() {
		lookingAtPlayer = false;
	}


	public void IsPossessed (bool possessed) {
		visible = !possessed;
		UpdateVisibility ();
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
}
