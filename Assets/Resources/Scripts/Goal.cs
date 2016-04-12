using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// can take hits equal to hp
	public int maxHitPoints = 5;
	private int currentHitPoints = 5;
	// Use this for initialization
	void Start () {
		currentHitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		ShowHitPointsUI ();
	}

	private void ShowHitPointsUI () {

	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == 11) {
			// a monster has hit the treasure
			col.gameObject.GetComponent<EnemyBehavior>().HitGoal();
			currentHitPoints--;
		}
	}
}
