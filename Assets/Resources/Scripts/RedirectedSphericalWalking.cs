using UnityEngine;
using System.Collections;

public class RedirectedSphericalWalking : MonoBehaviour {
	public Transform trackingSpace; 

	private Vector3 oldPos;
	private Vector3 newPos; 

	// Use this for initialization
	void Start () {
		oldPos = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		//TrackingSpaceGain ();
	}

	void TrackingSpaceGain() {
		newPos = this.transform.localPosition;

		// get difference in local position to find out if player moved
		Vector3 temp = GetSpherePosFromPlanePos(newPos.x, newPos.z);

		// add that difference to trackingspace position
		trackingSpace.position = temp;
		oldPos = newPos;
	}

	private Vector3 GetSpherePosFromPlanePos(float xPos,float yPos) {
		float x = (2f * xPos) / (1f + Mathf.Pow (xPos, 2) + Mathf.Pow (yPos, 2));
		float y = (2f * yPos) / (1f + Mathf.Pow (xPos, 2) + Mathf.Pow (yPos, 2));
		float z = (-1 + Mathf.Pow (xPos, 2) + Mathf.Pow (yPos, 2)) / (1f + Mathf.Pow (xPos, 2) + Mathf.Pow (yPos, 2));

		return new Vector3 (x,y,z);
	}
}
