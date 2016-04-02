using UnityEngine;
using System.Collections;

public class RedirectedSphericalWalking : MonoBehaviour {
	public Transform player;

	private Vector3 oldPos;
	private Vector3 newPos; 

	public float rotationScale = 100.0f;

	public float minXMovement = 0.01f;
	public float minZMovement = 0.01f;

	public bool onPlanet = true;
	private Vector3 planetCenter = Vector3.zero;
	public Transform planet; 


	// Use this for initialization
	void Start () {
		oldPos = player.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (onPlanet) {
			PlanetRotation ();
		}
	}

	void TrackingSpaceGain() {
		newPos = player.transform.localPosition;

		// get difference in local position to find out if player moved
		Vector3 temp = new Vector3 (newPos.x - oldPos.x, newPos.y - oldPos.y, newPos.z - oldPos.z);

		// add that difference to trackingspace position
		this.transform.position += temp;
		oldPos = newPos;
	}


	// always keep trackingSpace at an angle such that it is perpendicular to direction vector between
	// tracking space and current planet center 

	void PlanetRotation() {

		newPos = player.transform.localPosition;

		// get difference in local position to find out if player moved
		float xDiff = newPos.x - oldPos.x;

		float zDiff = newPos.z - oldPos.z;
	
		if (Mathf.Abs (xDiff) < minXMovement) {
			xDiff = 0f;
		} 

		if (Mathf.Abs (zDiff) < minZMovement) {
			zDiff = 0f;
		}


		Vector3 temp = new Vector3 (-zDiff, 0f , xDiff);
		temp *= rotationScale;

		planet.Rotate (temp,Space.World);
		oldPos = newPos;
	

	
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
