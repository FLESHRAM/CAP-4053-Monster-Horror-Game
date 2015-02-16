using UnityEngine;
using System.Collections;

public class cone_front_trigger : MonoBehaviour {

	public bool isInfront = false;

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Object Entered the trigger");
		isInfront = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Object Exited the trigger");
		isInfront = false;
	}
}
