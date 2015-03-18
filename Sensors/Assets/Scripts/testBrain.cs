using UnityEngine;
using System.Collections;

public class testBrain : MonoBehaviour {
	
	public GameObject startNode;

	private Vector3 moveDir;

	private Vector3 prevPos;
	private Vector3 targetPos;
	private NodeData data;

	public bool stop;
	private bool newMovement;

	// Use this for initialization
	void Start () {
		stop = false;
		prevPos = transform.position;
		data = startNode.GetComponent("NodeData") as NodeData;

		targetPos = data.down.transform.position;
		newMovement = true;

		data.debugLines (data.name);
	}




	// Update is called once per frame
	void Update () {
		if(transform.position == targetPos) stop = true;

		if (!stop) {
			Vector3 currPos = transform.position;
			
			if (newMovement) {
				moveDir = targetPos - currPos;
				moveDir.z = 0;
				moveDir.Normalize ();
				
				
				newMovement = false;
			}

			if(transform.position == targetPos) stop = true;
			Vector3 target = moveDir * 1f + currPos;
			transform.position = Vector3.Lerp (currPos, target, Time.deltaTime);
			if(transform.position == targetPos) stop = true;
			float angle = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), 1f * Time.deltaTime);
			if(transform.position == targetPos) stop = true;
			newMovement = true;
		}

		//else if (stop) { transform.position = targetPos; }
	}
}
