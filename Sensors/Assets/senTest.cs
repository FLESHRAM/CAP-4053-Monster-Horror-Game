using UnityEngine;
using System.Collections;

public class senTest : MonoBehaviour {

	public Transform fStart, fEnd;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Raycasting ();
	}



	void Raycasting() {

		RaycastHit2D hit = Physics2D.Raycast (fStart.position, fEnd.position, Vector2.Distance (fStart.position, fEnd.position));

		if (hit.collider != null) {
			Debug.DrawLine(fStart.position, hit.point, Color.green);		
		
		}
	}
}
