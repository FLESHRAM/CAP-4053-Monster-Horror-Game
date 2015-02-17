using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public float moveSpeed = 8.0F;
	public float rotSpeed = 200.0F;
	public UnityEngine.UI.Text player;
	private string direction = "";

	void Update() {

		float move = Input.GetAxis("Vertical") * moveSpeed;
		float rot = Input.GetAxis("Horizontal") * rotSpeed;
		move *= Time.deltaTime;
		rot *= Time.deltaTime;

		transform.Translate (move, 0, 0);
		transform.Rotate(0, 0, -rot);

		if (transform.rotation.eulerAngles.z >= 80.0 && transform.rotation.eulerAngles.z <= 115.0) {
			direction = "North";
		}
		if (transform.rotation.eulerAngles.z >= 175.0 && transform.rotation.eulerAngles.z <= 208.0) {
			direction = "West";
		}
		if (transform.rotation.eulerAngles.z >= 266.0 && transform.rotation.eulerAngles.z <= 308.0) {
			direction = "South";
		}
		if (transform.rotation.eulerAngles.z <= 26.0 && transform.rotation.eulerAngles.z <= 358.0) {
			direction = "East";
		}

		//print ("Current Position: " + transform.position + " at roation: " + transform.rotation.eulerAngles);
		//player.text = ("Current Position: " + transform.position + " at roation: " + transform.rotation.eulerAngles + " Facing: " + direction);
	}
}
