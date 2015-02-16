using UnityEngine;
using System.Collections;

public class cone_back_trigger : MonoBehaviour {

	public int isOnBack = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.collider2D.gameObject.layer == LayerMask.NameToLayer ("Floater")) {
			isOnBack = 1;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		isOnBack = 0;
	}
}
