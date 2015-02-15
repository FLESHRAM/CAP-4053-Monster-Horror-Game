using UnityEngine;
using System.Collections;

public class pieSensor : MonoBehaviour {

	public Transform fStart_line1, fEnd_line1;
	public Transform fStart_line2, fEnd_line2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine (fStart_line1.position, fEnd_line1.position, Color.white);
		Debug.DrawLine (fStart_line2.position, fEnd_line2.position, Color.white);
	}
}
