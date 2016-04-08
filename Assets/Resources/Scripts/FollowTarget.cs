﻿using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	public Transform target; 

	public bool followTarget = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (followTarget) {
			FollowTargetPos ();
		}
	}


	void FollowTargetPos() {
		Vector3 temp = new Vector3 (target.position.x, 0f, target.position.z);
		this.transform.position = temp;
	}
}
