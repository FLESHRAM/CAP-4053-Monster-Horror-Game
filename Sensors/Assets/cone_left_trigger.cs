using UnityEngine;
using System.Collections;

public class cone_left_trigger : MonoBehaviour {

	public int isOnLeft = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.collider2D.gameObject.layer == LayerMask.NameToLayer ("Floater")) {
			isOnLeft = 1;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		isOnLeft = 0;
	}
}
