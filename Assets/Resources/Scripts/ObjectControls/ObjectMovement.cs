using UnityEngine;
using System.Collections;

public class ObjectMovement : MonoBehaviour {

	public Vector3 planetPos; 
	private bool movingToSpawnPos = false;
	private float speed = 2.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (movingToSpawnPos) {
			MoveTo (planetPos);
		}
	}

	public void SpawnObj(Vector3 pos) {

		movingToSpawnPos = true;
		this.GetComponent<Rigidbody> ().isKinematic = true;
		planetPos = pos;
	}

	private void MoveTo(Vector3 pos) {
		Vector3 moveDir = pos - this.transform.position;
		this.transform.Translate (moveDir * Time.deltaTime * speed, Space.World);

		if (Vector3.Distance (this.transform.position, pos) <= 0.01f) {
			movingToSpawnPos = false;
			this.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
}
