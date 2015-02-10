using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public float moveSpeed = 8.0F;
	public float rotSpeed = 200.0F;
	void Update() {

		float trans = Input.GetAxis("Vertical") * moveSpeed;
		float rot = Input.GetAxis("Horizontal") * rotSpeed;
		trans *= Time.deltaTime;
		rot *= Time.deltaTime;

		transform.Translate(trans, 0, 0);
		transform.Rotate(0, 0, rot);
	}
}
