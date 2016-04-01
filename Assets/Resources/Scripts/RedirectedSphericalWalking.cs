using UnityEngine;
using System.Collections;

public class RedirectedSphericalWalking : MonoBehaviour {
	public Transform player;

	private Vector3 oldPos;
	private Vector3 newPos; 

	// Use this for initialization
	void Start () {
		oldPos = player.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (onPlanet) {
			TrackingSpaceGain ();
			// TrackingSpaceRotation();
		}
	}

	void TrackingSpaceGain() {
		newPos = player.transform.localPosition;

		// get difference in local position to find out if player moved
		Vector3 temp = new Vector3 (newPos.x - oldPos.x, newPos.y - oldPos.y, newPos.z - oldPos.z);

		// add that difference to trackingspace position
		this.transform.position += temp;
		planet.Rotate (temp * 100f);
		Debug.Log (planet.rotation);
		oldPos = newPos;
	}


	// always keep trackingSpace at an angle such that it is perpendicular to direction vector between
	// tracking space and current planet center 

	void TrackingSpaceRotation() {

		// get difference in local position to find out if player moved
		Vector3 temp = new Vector3 (newPos.x - oldPos.x, newPos.y - oldPos.y, newPos.z - oldPos.z);

		// add that difference to trackingspace position
		this.transform.Rotate(temp);
	
	}


	// always move the the trackingSpace downwards
	// always keep the rotation perpendicular to planet
	private bool onPlanet = false;
	private Vector3 planetCenter = Vector3.zero;
	private Transform planet; 

	void AssertGravity() {
		if (onPlanet) {
			
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "Planet") {
			onPlanet = true;
			planetCenter = other.transform.position;
			planet = other.gameObject.transform;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Planet") {
			onPlanet = false;
			planetCenter = Vector3.zero;
			planet = null;
		}
	}
}
