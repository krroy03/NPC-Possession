using UnityEngine;
using System.Collections;

public class EnemyCollide : MonoBehaviour {

	GameObject item = null;
	Vector3 TargetPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Auto Target to the Enemy's child component(the actual mesh)
	void OnTriggerStay (Collider col)
	{	
		// item is an object
		if (col.gameObject.layer == 8) {
			item = col.gameObject;
			// item!=null, item is not in hand, item is thrown out with a speed
			if (item && !item.GetComponent<Rigidbody> ().isKinematic /*&& item.GetComponent<Rigidbody> ().velocity.magnitude > .01f*/) {
				TargetPosition = this.transform.position;
				item.GetComponent<Rigidbody>().AddForce(.1f * (TargetPosition - item.transform.position));
			}
		}
	}
}	
   
