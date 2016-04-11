using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public float speed = 0.1f;

	private Transform target; 

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Goal").transform;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (target.position);
		MoveTo (target.position);
	}


	public void HitGoal() {

		GameObject.Destroy (this.gameObject);
	}

	private void MoveTo(Vector3 pos) {
		Vector3 moveDir = pos - this.transform.position;
		this.transform.Translate (moveDir * Time.deltaTime * speed, Space.World);

		if (Vector3.Distance (this.transform.position, pos) <= 1f) {
			HitGoal ();
		}
	}
}
