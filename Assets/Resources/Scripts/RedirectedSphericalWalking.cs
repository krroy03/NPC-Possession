﻿using UnityEngine;
using System.Collections;

public class RedirectedSphericalWalking : MonoBehaviour
{

	public enum WalkingMethod
	{
		RotatePlanet,
		RotatePlayer}

	;


	public Transform player;

	public Transform leftController; 
	public Transform rightController; 


	private Vector3 oldPos;
	private Vector3 newPos;

	public float rotationScale = 100.0f;

	public float minXMovement = 0.01f;
	public float minZMovement = 0.01f;

	public bool onPlanet = true;
	public Transform planet;

	private PossessionMechanic pm;

	// movement variables
	public WalkingMethod walkingType;
	public float walkSpeed = 6;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	Rigidbody thisRigidBody;

	private float minY = 0.7f;
	// Use this for initialization
	void Start ()
	{
		minY = Mathf.Min (minY, player.position.y / 3f);
		Debug.Log (minY);

		if (walkingType == WalkingMethod.RotatePlanet) {
			oldPos = player.transform.position;
		} else if (walkingType == WalkingMethod.RotatePlayer) {
			oldPos = player.transform.localPosition;
		}

		pm = this.gameObject.GetComponent<PossessionMechanic> ();
		thisRigidBody = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (onPlanet && pm.finishedShifting && (leftController.position.y >= minY || rightController.position.y >= minY)) {
			if (walkingType == WalkingMethod.RotatePlanet) {
				PlanetRotation ();
			} else if (walkingType == WalkingMethod.RotatePlayer) {
				TrackingSpaceMovement ();
			}
		} else {
			Debug.Log ("gets here");
		}
	}

	void FixedUpdate ()
	{
		if (walkingType == WalkingMethod.RotatePlayer) {
			// Apply movement to rigidbody
			Vector3 localMove = transform.TransformDirection (moveAmount) * Time.fixedDeltaTime;
			thisRigidBody.MovePosition (thisRigidBody.position + localMove);
		}
	}


	void TrackingSpaceMovement ()
	{

		newPos = player.transform.localPosition;
		if (pm.reachedPlanet) {
			oldPos = newPos;
			pm.reachedPlanet = false;
		}
		// get difference in local position to find out if player moved
		float xDiff = newPos.x - oldPos.x;

		float zDiff = newPos.z - oldPos.z;

		if (Mathf.Abs (xDiff) < minXMovement * 2f) {
			xDiff = 0f;
		} 
		if (Mathf.Abs (zDiff) < minZMovement * 2f) {
			zDiff = 0f;
		}

		// get difference in local position to find out if player moved
		Vector3 temp = new Vector3 (xDiff, 0f, zDiff);


		// Calculate movement:
		//float inputX = Input.GetAxisRaw ("Horizontal");
		//float inputY = Input.GetAxisRaw ("Vertical");
		//Vector3 moveDir = new Vector3 (inputX, 0, inputY).normalized;

		Vector3 moveDir = temp.normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp (moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);


		// add that difference to trackingspace position
		//this.transform.position += temp;
		oldPos = newPos;
	}



	// always keep trackingSpace at an angle such that it is perpendicular to direction vector between
	// tracking space and current planet center

	void PlanetRotation ()
	{
		newPos = player.transform.position;
		if (pm.reachedPlanet) {
			oldPos = newPos;
			pm.reachedPlanet = false;
		}
		// get difference in local position to find out if player moved
		float xDiff = newPos.x - oldPos.x;

		float zDiff = newPos.z - oldPos.z;
	
		if (Mathf.Abs (xDiff) < minXMovement) {
			xDiff = 0f;
		} 

		if (Mathf.Abs (zDiff) < minZMovement) {
			zDiff = 0f;
		}

		Vector3 temp = new Vector3 (-zDiff, 0f, xDiff);
		temp *= rotationScale;

		planet.Rotate (temp, Space.World);
		oldPos = newPos;
	}



	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Planet") {
			onPlanet = true;
			planet = other.gameObject.transform;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Planet") {
			onPlanet = false;
			planet = null;
		}
	}
}
