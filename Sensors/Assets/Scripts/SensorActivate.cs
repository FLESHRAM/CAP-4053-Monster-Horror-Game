using UnityEngine;
using System.Collections;

public class SensorActivate : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		GetComponent<SpriteRenderer>().color = Color.red;
	}

	void OnCollisionExit2D(Collision2D other)
	{
		GetComponent<SpriteRenderer>().color = Color.black;
	}


}
