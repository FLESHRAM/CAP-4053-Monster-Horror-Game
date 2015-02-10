using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	void Update() {
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0, translation, 0);
		transform.Rotate(rotation, rotation, 0);
	}
}
