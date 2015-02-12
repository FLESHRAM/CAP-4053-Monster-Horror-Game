using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public float moveSpeed = 8.0F;
	public float rotSpeed = 200.0F;
	public Vector3 stuff;

	void Update() {

		float move = Input.GetAxis("Vertical") * moveSpeed;
		float rot = Input.GetAxis("Horizontal") * rotSpeed;
		move *= Time.deltaTime;
		rot *= Time.deltaTime;

		transform.Translate(move, 0, 0);
		transform.Rotate(0, 0, -rot);

		Collider[] colliders = Physics.OverlapSphere (transform.position, 20f);
		foreach (Collider col in colliders) {
			Debug.Log ("I hit: " + col.name);
			stuff = col.transform.position;
			Debug.DrawLine(transform.position, stuff, Color.red);
		}
	}
}
