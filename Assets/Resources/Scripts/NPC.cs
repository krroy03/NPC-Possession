using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public bool visible = true;

	public Transform player;
	public HeadLookController headlook;
	public Vector3 center; 

	private Quaternion originalRot; 

	private Animator anim;

	private Transform headTarget;
	 
	private bool lookingAtPlayer = false;

	void Start() {
		originalRot = this.transform.rotation;

		anim = GetComponent <Animator> ();
		center = this.transform.position;


		//headlook.headUpVector.y = this.transform.position.y;
		//Debug.Log("headupvector:" + headlook.headUpVector);
	}

	void Update ()
	{
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
		//headlook.target.y = 50;
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

		SkinnedMeshRenderer[] renderers2 = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ();
		foreach (SkinnedMeshRenderer renderer in renderers2) {
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
		newPos.y = center.y;
		this.transform.position = newPos;

		Quaternion newRot = headTarget.rotation;
		newRot.x = 0f;
		newRot.z = 0f;
		this.transform.rotation = newRot;
	}

	//wait for a while before doing anything
	IEnumerator Wait ()
	{
		yield return new WaitForSeconds(1);
	}

	//when throw object to awake NPCs
	private void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.layer == 8) {
			//wait for a while
			Wait ();
			ObjectThrow obj = col.gameObject.GetComponent<ObjectThrow> ();
			Debug.Log (obj.npc.ToString () + "throw at me!");
			if (obj.npc) {
				LookAtPlayer (obj.npc);
			}
		}
			//lookingAt.GetComponent<NPC> ().LookAtPlayer (currentNPC);
					
	}
	

}
