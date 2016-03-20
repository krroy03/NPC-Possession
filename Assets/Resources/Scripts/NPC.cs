using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public bool visible = true;

	private bool lookingAtPlayer = false;
	private Transform player;
	private Quaternion originalRot; 
	void Start() {
		originalRot = this.transform.rotation;
	}

	void Update() {
		if (!lookingAtPlayer) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, originalRot, Time.deltaTime);
		}
	}

	public void LookAtPlayer(GameObject playerNew) {
		lookingAtPlayer = true;
		player = playerNew.transform;
		var rotation = Quaternion.LookRotation (player.position - this.transform.position, Vector3.up);
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, rotation, Time.deltaTime);
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
}
