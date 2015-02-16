using UnityEngine;
using System.Collections;

public class cone_right_trigger : MonoBehaviour {

	public int isOnRight = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.collider2D.gameObject.layer == LayerMask.NameToLayer ("Floater")) {
			isOnRight = 1;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		isOnRight = 0;
	}
}
