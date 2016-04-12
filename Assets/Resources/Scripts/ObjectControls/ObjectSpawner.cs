using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {

	public GameObject spawnedObj;

	public float radius = 0f;
	public int maxCount = 20;
	public float interval = 1f;

	private Vector3 center; 
	private int count = 0;


	// Use this for initialization
	void Start () {
		center = this.transform.position;
		// immediately spawn 20 objects for each 
		for (int i = 0; i < maxCount; i ++) {
			SpawnObject();
		}
		InvokeRepeating ("SpawnObject", 0f, interval);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SpawnObject() {
		if (count < maxCount) {
			GameObject spawned = GameObject.Instantiate (spawnedObj, center, Quaternion.identity) as GameObject;
			spawned.transform.SetParent (this.transform);

			Vector3 randomDir = new Vector3 (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			randomDir = (randomDir.normalized) * radius;
			Vector3 planetPos = center + randomDir;

			// now propel object outwards in a random direction
			spawned.transform.LookAt (planetPos);
			spawned.GetComponent<ObjectMovement> ().SpawnObj (planetPos);
			count++;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == 8) {
			count++;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.layer == 8) {
			count--;
		}
	}
}
