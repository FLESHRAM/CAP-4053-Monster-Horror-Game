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

		transform.Translate(translation, 0, 0);
		transform.Rotate(0, 0, rotation);
	}
}
