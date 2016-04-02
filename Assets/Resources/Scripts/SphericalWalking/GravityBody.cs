using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {
	
	public GravityAttractor planet;
	Rigidbody rigidbody;
	
	void Awake () {
		rigidbody = GetComponent<Rigidbody> ();

		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		planet = GetComponentInParent<GravityAttractor> ();
	}
	
	void FixedUpdate () {

		planet.Attract(rigidbody);
	}


}