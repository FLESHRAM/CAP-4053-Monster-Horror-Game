using UnityEngine;
using System.Collections;

public class Brain : MonoBehaviour {

	public GameObject sight;
	public GameObject blood;
	private ArrayList path = new ArrayList ();


	private float sightRadius = 6f;
	private ArrayList openList = new ArrayList ();
	private ArrayList closedList = new ArrayList ();
	private ArrayList allVisited = new ArrayList();


	private Vector3 targetPos;
	private Vector3 moveDir;

	private bool moving;
	private bool turning;
	private bool pathing;
	private stats stat;
	private RuntimeAnimatorController saved_cont;

	private float walkingSpeed = 1f;
	private float runningSpeed = 2.5f;
	private int sprintCount = 0;

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		saved_cont = anim.runtimeAnimatorController;
		stat = gameObject.GetComponent ("stats") as stats;

		GameObject close = closestNode ();
		transform.position = new Vector2 (close.transform.position.x, close.transform.position.y);

		stat.health = 100f;
		targetPos = transform.position;
		moving = true;
		turning = true;
	}



	// Update is called once per frame
	void Update () {

		//print (Vector2.Distance (transform.position, sight.transform.position));
		ArrayList visibleNodes = getVisibleNodes ();
		GameObject player = visiblePlayer ();
		ArrayList objects = hidingObjects ();


		//for(int i = 0; i<visibleNodes.Count; i++)
		//{
			//GameObject t = (GameObject) visibleNodes[i];
			//print (t);
		//}

		for(int i = 0; i<objects.Count; i++)
		{
			GameObject t = (GameObject) objects[i];
			print (t);
		}

		if(player != null) print("I see the Player!!!");

		float speed = walkingSpeed;
		if (sprintCount > 0) { speed = runningSpeed; sprintCount--; }


		if(moving)
		{
			anim.SetBool("IsMoving", true);
			Vector3 currPos = transform.position;
			Vector3 target = getTarget (currPos);
			transform.position = Vector3.Lerp (currPos, target, speed * Time.deltaTime);
		}

		if(turning)
		{
			Vector3 currPos = transform.position;
			Vector3 target = getTarget(currPos);
			float angle = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), 2.5f * Time.deltaTime);
		}

		float closeEnough = 0.08f;
		if(sprintCount > 0) closeEnough = 1f;
		if(Vector3.Distance(transform.position, targetPos) < closeEnough) { moving = false; turning = false;  if(path.Count == 0) anim.SetBool("IsMoving", false); }
		
		if (path.Count>0) takingPath();
	}




	public void sprint()
	{ sprintCount = 50; }




	public ArrayList getVisibleNodes ()
	{
		ArrayList visible = new ArrayList ();
		Collider2D[] nodes = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Node"));

		for(int i=0; i<nodes.Length; i++)
		{
			bool hit = Physics2D.Linecast(transform.position, nodes[i].transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
			if (!hit) visible.Add (nodes[i].gameObject);
		}

		return visible;
	}


	public GameObject visiblePlayer()
	{
		GameObject player = null;
		Collider2D p = Physics2D.OverlapCircle (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Player"));
		if(p!=null)
		{
			bool hit = Physics2D.Linecast(transform.position, p.transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
			if(hit) print ("Wall was hit");
			if (!hit) player = p.gameObject;
		}


		return player;
	}


	public ArrayList getVisisbleBombs()
	{
		ArrayList visibleBombs = new ArrayList ();
		Collider2D[] bombs = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Bomb"));
		
		for(int i=0; i<bombs.Length; i++)
		{
			bool hit = Physics2D.Linecast(transform.position, bombs[i].transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
			if (!hit) visibleBombs.Add (bombs[i].gameObject);
		}
		
		return visibleBombs;
	}


	public ArrayList hidingObjects ()
	{
		ArrayList visible = new ArrayList ();
		Collider2D[] hiding = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Object"));
		
		for(int i=0; i<hiding.Length; i++)
		{
			bool hit = Physics2D.Linecast(transform.position, hiding[i].transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
			if (!hit) 
			{
				if(!visible.Contains(hiding[i].gameObject)) visible.Add (hiding[i].gameObject);
			}
		}
		
		return visible;
	}



	public GameObject closestNode()
	{
		Collider2D[] nodes = Physics2D.OverlapCircleAll (transform.position, 3f, 1 << LayerMask.NameToLayer ("Node"));
		GameObject mostNear = null;
		if(nodes!=null)
		{
			for(int i=0; i<nodes.Length; i++)
			{
				if(mostNear == null) mostNear = nodes[i].gameObject;
				
				else
				{
					if(Vector3.Distance(transform.position, nodes[i].transform.position) < Vector3.Distance(transform.position, mostNear.transform.position))
						mostNear = nodes[i].gameObject;
				}
			}
	     }

		return mostNear;
	}



	public GameObject furthestNode()
	{
		Collider2D[] nodes = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Node"));
		GameObject mostFar = null;
		if(nodes!=null)
		{
			for(int i=0; i<nodes.Length; i++)
			{
				if(mostFar == null) mostFar = nodes[i].gameObject;
				
				else
				{
					if(Vector3.Distance(transform.position, nodes[i].transform.position) > Vector3.Distance(transform.position, mostFar.transform.position))
						mostFar = nodes[i].gameObject;
				}
			}
		}
		
		return mostFar;
	}




	public void testSight()
	{
		Collider2D[] nodes = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Node"));
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




	public void seek(GameObject start, GameObject target)
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
