using UnityEngine;
using System.Collections;

public class cone_left_trigger : MonoBehaviour {

	public int isOnLeft = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Object Entered the trigger");
		isOnLeft = 1;
	}
	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Object Exited the trigger");
		isOnLeft = 0;
	}
}
