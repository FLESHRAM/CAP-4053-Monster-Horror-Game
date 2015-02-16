using UnityEngine;
using System.Collections;

public class cone_back_trigger : MonoBehaviour {

	public int isOnBack = 0;
	
	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("Object Entered the trigger");
		isOnBack = 1;
	}
	void OnTriggerExit2D(Collider2D other) {
		//Debug.Log ("Object Exited the trigger");
		isOnBack = 0;
	}
}
