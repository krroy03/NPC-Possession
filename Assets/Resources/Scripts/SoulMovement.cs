using UnityEngine;
using System.Collections;

public class SoulMovement : MonoBehaviour {
	public Transform head; 
	public bool followHead = true;

	private bool returnToPlayer = false;
	private float returnSpeed = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (followHead) {
			FollowHeadPos ();
		}

		if (returnToPlayer) {
			ReturnTo (new Vector3 (head.position.x, head.position.y / 2f, head.position.z));
		}

	}

	void FollowHeadPos() {
		Vector3 temp = new Vector3 (head.position.x, head.position.y / 2f, head.position.z);
		this.transform.position = temp;
	}

	public IEnumerator ResetSoulPositionIfMiss(float waitTime) {
		yield return new WaitForSeconds (waitTime);

		if (!followHead) {
			returnToPlayer = true;
			this.GetComponent<SphereCollider> ().enabled = false;
		}
	}

	private void ReturnTo(Vector3 pos) {
		Vector3 moveDir = pos - this.transform.position;
		this.transform.Translate (moveDir * Time.deltaTime * returnSpeed, Space.World);

		if (Vector3.Distance (this.transform.position, pos) <= 0.001f) {
			returnToPlayer = false;
			followHead = true;
			this.GetComponent<SphereCollider> ().enabled = true;
		}
	}
}
