using UnityEngine;
using System.Collections;

public class testBrain : MonoBehaviour {
	
	public GameObject startNode;

	private Vector3 moveDir;

	private Vector3 prevPos;
	private Vector3 targetPos;
	private NodeData data;

	public bool stop;

	// Use this for initialization
	void Start () {
		stop = false;
		prevPos = transform.position;
		data = startNode.GetComponent("NodeData") as NodeData;

		targetPos = data.down.transform.position;

		data.debugLines (data.name);
	}




	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, targetPos) < 0.003f ) stop = true;

		if (!stop) {
			Vector3 currPos = transform.position;
			

				moveDir = targetPos - currPos;
				moveDir.z = 0;
				moveDir.Normalize ();
				
				


			Vector3 target = moveDir * 1f + currPos;
			transform.position = Vector3.Lerp (currPos, target, Time.deltaTime);

			float angle = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), 1f * Time.deltaTime);

			if(Vector3.Distance(transform.position, targetPos) < 0.003f ) stop = true;
		}

		//else if (stop) { transform.position = targetPos; }
	}
}
