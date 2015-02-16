using UnityEngine;
using System.Collections;

public class cone_front_trigger : MonoBehaviour {

	public int isInfront = 0;

	void OnTriggerEnter2D(Collider2D other){
		if (other.collider2D.gameObject.layer == LayerMask.NameToLayer ("Floater")) {
			isInfront = 1;
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		isInfront = 0;
	}
}
