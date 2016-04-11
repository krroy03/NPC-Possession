using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	public GameObject spawnedEnemy;

	public float interval = 20f;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnObject", 5f, interval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnObject() {
		GameObject spawned = GameObject.Instantiate (spawnedEnemy, this.transform.position, Quaternion.identity) as GameObject;
		spawned.transform.SetParent (this.transform);

	}
}
