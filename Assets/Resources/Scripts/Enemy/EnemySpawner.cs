using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject spawnedEnemy;

	private bool inWave = false; 

	private GameObject spawned;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// spawn a monster each time the previous one is destroyed and wave still ongoing
		if (inWave) {
			if (!spawned) {
				SpawnObject ();
			}
		}
	}

	private void SpawnObject() {
		spawned = GameObject.Instantiate (spawnedEnemy, this.transform.position, Quaternion.identity) as GameObject;
		spawned.transform.SetParent (this.transform);

	}

	public void StartWave() {
		inWave = true; 
		SpawnObject ();
	}

	public void StopWave() {
		inWave = false;
		GameObject.Destroy (spawned);
		spawned = null;
	}
}
