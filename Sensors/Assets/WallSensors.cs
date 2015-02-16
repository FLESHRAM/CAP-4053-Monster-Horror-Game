using UnityEngine;
using System.Collections;

public class WallSensors : MonoBehaviour {

	public int range;

	// Use this for initialization
	void Start () {
		range = 5;
	}
	
	// Update is called once per frame
	void Update () {
		Raycasting ();
	}
	
	
	
	void Raycasting() {

		Vector2 dir;
		dir.x = range * Mathf.Cos (Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
		dir.y = range * Mathf.Sin (Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, range, 1 << LayerMask.NameToLayer ("Obstacle"));
		if (hit.collider != null) {
			Debug.DrawLine(transform.position, hit.point, Color.green);		
			Debug.Log(hit.distance);
		}
	}
}
