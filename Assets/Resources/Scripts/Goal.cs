using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// can take hits equal to hp
	public int maxHitPoints = 10;
	private int currentHitPoints = 5;
	// Use this for initialization
	void Start () {
		currentHitPoints = maxHitPoints;
		UIManager.Instance.OrbHP.text = "" + currentHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
	}
		

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == 11) {
			// a monster has hit the treasure
			col.gameObject.GetComponent<EnemyBehavior>().HitGoal();
			currentHitPoints--;
			UIManager.Instance.OrbHP.text = "" + currentHitPoints;
			if (currentHitPoints <= 0) {
				
			}
		}
	}

	void GameOver() {
		WaveManager.gameOver = true;
		UIManager.Instance.ShowScore ();
	}
}
