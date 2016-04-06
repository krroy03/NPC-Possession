using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	public GravityAttractor planet;
	Rigidbody rigidbody;

	public float gravityMultiplier = 1f;
	// when enters another planet radius, gets attracted to that planet 

	public bool beingControlled = false; 

	void Awake () {
		rigidbody = GetComponent<Rigidbody> ();

		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		planet = GetComponentInParent<GravityAttractor> ();
	}


	void FixedUpdate () {
		if (planet && !beingControlled)
			planet.Attract(rigidbody, gravityMultiplier);
	}


	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Planet" && !beingControlled) {
			planet = null;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Planet" && !beingControlled) {
			planet = col.gameObject.GetComponent<GravityAttractor> ();
			if (this.gameObject.layer == 8) {
				this.transform.SetParent (col.transform);
			}
		}
	}

}