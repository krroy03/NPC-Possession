using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	public GravityAttractor planet;
	Rigidbody rigidbody;

	public float gravityMultiplier = 1f;
	// when enters another planet radius, gets attracted to that planet 

	void Awake () {
		rigidbody = GetComponent<Rigidbody> ();

		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		planet = GetComponentInParent<GravityAttractor> ();
	}


	void FixedUpdate () {
		if (planet)
			planet.Attract(rigidbody, gravityMultiplier);
	}


	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Planet") {
			planet = null;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Planet") {
			planet = col.gameObject;
			this.transform.SetParent (col.transform);
		}
	}

}