using UnityEngine;
using System.Collections;

public class WallSensors : MonoBehaviour {

	public int range;
	private float prev_right;
	private float prev_left;
	private float prev_center;

	// Use this for initialization
	void Start () {
		range = 3;
		prev_right = -1;
		prev_left = -1;
		prev_center = -1;
	}
	
	// Update is called once per frame
	void Update () {
		prev_center = Raycasting ("center_feeler", 0, Color.green, prev_center);
		prev_left = Raycasting ("left_feeler", 30, Color.red, prev_left);
		prev_right = Raycasting ("right_feeler", -30, Color.red, prev_right);
	}
	
	
	
	float Raycasting(string name, float z_offset, Color aColor, float prev) {

		Vector2 dir;
		dir.x = range * Mathf.Cos (Mathf.Deg2Rad * (transform.rotation.eulerAngles.z + z_offset));
		dir.y = range * Mathf.Sin (Mathf.Deg2Rad * (transform.rotation.eulerAngles.z + z_offset));
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir, range, 1 << LayerMask.NameToLayer ("Obstacle"));
		if (hit.collider != null) {
			Debug.DrawLine(transform.position, hit.point, aColor);	

			// Print distance?
			if (prev != hit.distance){
				Debug.Log(name + " " + hit.distance);
				prev = hit.distance;
			}
		}

		return prev;
	}
}
