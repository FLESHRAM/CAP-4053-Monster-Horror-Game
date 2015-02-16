using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	public float moveSpeed = 8.0F;
	public float rotSpeed = 200.0F;
	public UnityEngine.UI.Text player;

	void Update() {

		float move = Input.GetAxis("Vertical") * moveSpeed;
		float rot = Input.GetAxis("Horizontal") * rotSpeed;
		move *= Time.deltaTime;
		rot *= Time.deltaTime;

		transform.Translate(move, 0, 0);
		transform.Rotate(0, 0, -rot);

		print ("Current Position: " + transform.position + " at roation: " + transform.rotation.z);
		player.text = ("Current Position: " + transform.position + " at roation: " + transform.rotation.z);
	}
}
