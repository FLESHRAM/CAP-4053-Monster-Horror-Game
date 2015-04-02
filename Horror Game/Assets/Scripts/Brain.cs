using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {

	public GameObject sight;
	private ArrayList path = new ArrayList ();



	private ArrayList openList = new ArrayList ();
	private ArrayList closedList = new ArrayList ();
	private ArrayList allVisited = new ArrayList();


	private Vector3 targetPos;
	private Vector3 moveDir;

	private bool moving;
	private bool turning;
	private bool pathing;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		targetPos = transform.position;
		moving = true;
		turning = true;
	}



	// Update is called once per frame
	void Update () {

		if(moving)
		{
			anim.SetBool("IsWalking", true);
			Vector3 currPos = transform.position;
			Vector3 target = getTarget (currPos);
			transform.position = Vector3.Lerp (currPos, target, Time.deltaTime);
		}

		if(turning)
		{
			Vector3 currPos = transform.position;
			Vector3 target = getTarget(currPos);
			float angle = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), 2.5f * Time.deltaTime);
		}

		if(Vector3.Distance(transform.position, targetPos) < 0.08f ) { moving = false; turning = false; anim.SetBool ("IsWalking", false); }
		
		if (path.Count>0) takingPath();
	}





	public void testSight()
	{
		Collider2D[] nodes = Physics2D.OverlapCircleAll (sight.transform.position, 1.5f, 1 << LayerMask.NameToLayer ("Node"));
		if(nodes!=null)
		{
			GameObject mostFar = null;
			GameObject mostNear = null;
			for(int i=0; i<nodes.Length; i++)
			{
				print(nodes[i].gameObject);
				if (mostFar == null) mostFar = nodes[i].gameObject; 
				if(mostNear == null) mostNear = nodes[i].gameObject;

				else
				{
					if(Vector3.Distance(transform.position, nodes[i].transform.position) > Vector3.Distance(transform.position, mostFar.transform.position))
						mostFar = nodes[i].gameObject;

					if(Vector3.Distance(transform.position, nodes[i].transform.position) < Vector3.Distance(transform.position, mostNear.transform.position))
						mostNear = nodes[i].gameObject;
				}
			}

			//targetPos = mostFar.transform.position;
			moving = true;
			turning = true;
			print (mostFar);
			print (mostNear);
			print (targetPos);
			seek (mostNear, mostFar);
		}
	}



	private void takingPath()
	{
		if(!moving)
		{
			GameObject t = (GameObject) path[0];
			targetPos = t.transform.position;
			moving = true; turning = true;

			path.Remove(t);
		}
	}





	private Vector3 getTarget(Vector3 curr)
	{
		moveDir = targetPos - curr;
		moveDir.z = 0;
		moveDir.Normalize ();
		
		
		return (moveDir * 1f + curr);
	}




	void advancedSeek(GameObject start, GameObject target)
	{
		//bool linked = false;
		//GameObject temp = target;
		//while (!linked) 
		//{

		//}
	}




	void seek(GameObject start, GameObject target)
	{
		NodeInfo info = start.GetComponent ("NodeInfo") as NodeInfo;
		info.length = 0;
		info.last = null;
		closedList.Add (start);
		
		bool running = true;
		while (running) 
		{ 
			allVisited.Add(start);
			info = start.GetComponent ("NodeInfo") as NodeInfo;
			if(start == target)
			{
				running = false; break;
			}
			
			if(info.up!=null && !closedList.Contains(info.up))
			{ traverseTo(info.up, start); }
			
			if(info.down!=null && !closedList.Contains(info.down))
			{ traverseTo(info.down, start); }
			
			if(info.left!=null && !closedList.Contains (info.left))
			{ traverseTo(info.left, start); }
			
			if(info.right!=null && !closedList.Contains(info.right))
			{ traverseTo(info.right, start); }
			
			
			if(openList.Contains(start)) openList.Remove(start);
			if(!closedList.Contains(start)) closedList.Add (start);
			//print ((GameObject)openList[0]);
			start = (GameObject)openList[0];
		}



		running = true;
		while(running)
		{
			path.Add (start);
			NodeInfo temp = start.GetComponent("NodeInfo") as NodeInfo;
			GameObject tN = start;

		
				start = temp.last;
				//temp = tN.GetComponent("NodeData") as NodeData;
				//temp.last = null;


			if (start == null) running = false;
		}
		
		path.Reverse ();
		string p = "";
		for (int i=0; i<path.Count; i++) {
			p += (GameObject)path[i] + " ";		
		}
		print (p);
		Cleanup ();


	}





	private void traverseTo(GameObject node, GameObject curr)
	{
		NodeInfo info = node.GetComponent("NodeInfo") as NodeInfo;
		NodeInfo prevInfo = curr.GetComponent("NodeInfo") as NodeInfo;
		if(openList.Contains(node))
		{
			if(info.length < prevInfo.length+1) {}
			
			else if (info.length > prevInfo.length+1)
			{
				openList.Remove(node);
				info.last = node;
				info.length = prevInfo.length++;
				openList.Add(node);
				
			}
		}
		else 
		{
			info.last = curr;
			info.length = prevInfo.length++;
			openList.Add (node);
		}
	}
	
	


	private void Cleanup()
	{
		openList.Clear ();
		closedList.Clear ();

		for(int i=0; i<allVisited.Count; i++)
		{
			GameObject curr = (GameObject)allVisited[i];
			NodeInfo info = curr.GetComponent("NodeInfo") as NodeInfo;
			info.last = null;
			info.length = 0;
		}

		allVisited.Clear ();
	}


	
}
