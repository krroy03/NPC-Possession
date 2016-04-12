using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	public static bool gameOver = false; 
	public static float score = 0f;

	public int waveNumber = 0;

	public float waveInterval = 10f; 
	public float waveTime = 60f; 


	public EnemySpawner[] spawners; 

	// Use this for initialization
	void Start () {

		// start after zeroth wave interval and repeat every 70 seconds after
		InvokeRepeating ("StartWaves", waveInterval, waveTime + waveInterval);

		// start at the start and repeat every 70 seconds after
		InvokeRepeating ("StopWaves", 0f, waveTime + waveInterval);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartWaves() {
		for (int i = 0; i < spawners.Length; i++) {
			spawners [i].StartWave ();
		}
		waveNumber++;
	}

	void StopWaves() {
		for (int i = 0; i < spawners.Length; i++) {
			spawners [i].StopWave ();
		}
	}
}
