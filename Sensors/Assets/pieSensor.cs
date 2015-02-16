using UnityEngine;
using System.Collections;

public class pieSensor : MonoBehaviour {

	public Transform fStart_line1, fEnd_line1;
	public Transform fStart_line2, fEnd_line2;

	private cone_front_trigger cone_front;
	private cone_left_trigger cone_left;
	private cone_back_trigger cone_back;
	private cone_right_trigger cone_right;

	// Use this for initialization
	void Start () {
		//GameObject frontCone = GameObject.Find ("cone_front");
		//cone_front_trigger cone_front = frontCone.GetComponent<cone_front_trigger> ();
		cone_front = GameObject.Find ("cone_front").GetComponent<cone_front_trigger> ();
		cone_left = GameObject.Find ("cone_left").GetComponent<cone_left_trigger> ();
		cone_back = GameObject.Find ("cone_back").GetComponent<cone_back_trigger> ();
		cone_right = GameObject.Find ("cone_right").GetComponent<cone_right_trigger> ();
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine (fStart_line1.position, fEnd_line1.position, Color.white);
		Debug.DrawLine (fStart_line2.position, fEnd_line2.position, Color.white);
		Debug.Log ("Return: " + cone_front.isInfront.ToString() + "," + cone_left.isOnLeft.ToString() + "," + cone_back.isOnBack.ToString() + "," + cone_right.isOnRight.ToString());


		Collider2D[] colliders = Physics2D.OverlapCircleAll (transform.position, 1.75f);

		foreach (Collider2D col in colliders) {

			//Debug.Log("I see: " + col.name + " At an agle: " + Vector2.Angle(fStart_line1.position,col.transform.position));
		}
	}
}
