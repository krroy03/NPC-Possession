using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;

	public void Attract(Rigidbody body, float multiplier) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;
		
		// Apply gravity 
		body.AddForce(gravityUp * gravity * multiplier);
		// Allign the body's up axis with the centre of planet
		body.rotation = Quaternion.FromToRotation(localUp,gravityUp) * body.rotation;
	}  

	void OnTriggerEnter(Collider col) {

		GravityBody obj = col.gameObject.GetComponent<GravityBody> ();
		if (obj) {
			obj.planet = this;
		}
	}
}
