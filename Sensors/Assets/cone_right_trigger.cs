using UnityEngine;
using System.Collections;

public class cone_right_trigger : MonoBehaviour {

	public int isOnRight = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Object Entered the trigger");
		isOnRight = 1;
	}
	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Object Exited the trigger");
		isOnRight = 0;
	}
}
