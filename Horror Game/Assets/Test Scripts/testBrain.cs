using UnityEngine;
using System.Collections;

public class testBrain : MonoBehaviour {
	
	public GameObject startNode;

	private Vector3 moveDir;

	private Vector3 prevPos;
	private Vector3 targetPos;
	private NodeData data;


	private ArrayList path = new ArrayList();

	private ArrayList openList = new ArrayList ();
	private ArrayList closedList = new ArrayList ();

	private int count;
	public bool stop;

	private bool start = false;
	private bool wasDisabled = false;

	private SpriteRenderer rend;
	private NodeFunctions nod;
	public bool done = false;


	void OnDisable() {
		wasDisabled = true;
	}


	void OnEnable() {
		if (wasDisabled) {
			Initiate();
		}
	}


	// Use this for initialization
	void Start () {

	}




	// Update is called once per frame
	void Update () {
		if(nod.done && !done) { finish (); }
		if (start) 
		{
			if (stop) {
				count++;
				if(count < path.Count)
				{
					GameObject t = (GameObject)path[count];
					targetPos = t.transform.position;
					stop = false;
				}
				
			}
			
			if(Vector3.Distance(transform.position, targetPos) < 0.08f ) stop = true;
			
			if (!stop) {
				Vector3 currPos = transform.position;
				
				
				moveDir = targetPos - currPos;
				moveDir.z = 0;
				moveDir.Normalize ();
				
				
				
				
				Vector3 target = moveDir * 1f + currPos;
				transform.position = Vector3.Lerp (currPos, target, Time.deltaTime);
				
				float angle = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), 2.5f * Time.deltaTime);
				
				if(Vector3.Distance(transform.position, targetPos) < 0.08f ) stop = true;
			}
		}


		//else if (stop) { transform.position = targetPos; }
	}



	public void Initiate()
	{
		start = true;
		stop = false;
		prevPos = startNode.transform.position;
		data = startNode.GetComponent("NodeData") as NodeData;
		
		//targetPos = data.down.transform.position;
		
		data.debugLines (data.name);
		count = 1;
		seek (startNode, "Floater");
		GameObject t = (GameObject)path [count];
		targetPos = t.transform.position;
		nod = startNode.GetComponentInParent<NodeFunctions>();
	}
	
	
	void finish()
	{
		GameObject t = (GameObject)path [0];
		nod = t.GetComponentInParent<NodeFunctions>();
		Sprite sprite = nod.sprite;
		for(int i=0; i<path.Count; i++)
		{
			t = (GameObject)path[i];
			rend = t.GetComponent<SpriteRenderer>();
			rend.sprite = sprite;
			t.renderer.material.color = Color.green;
		}

		done = true;
	}


	void seek(GameObject node, string target)
	{

		NodeData data = node.GetComponent ("NodeData") as NodeData;
		data.length = 0;
		data.last = null;
		closedList.Add (node);

		bool running = true;
		while (running) 
		{ 
			data = node.GetComponent ("NodeData") as NodeData;
			if(data.Inhabited!=null)
			{
				if(data.Inhabited.name == target) { running = false; break; }
			}

			if(data.up!=null && !closedList.Contains(data.up))
			{
				NodeData temp = data.up.GetComponent("NodeData") as NodeData;
				if(openList.Contains(data.up))
				{
					if(temp.length < data.length+1) {}
					
					else if (temp.length > data.length+1)
					{
						openList.Remove(data.up);
						temp.last = node;
						temp.length = data.length++;
						openList.Add(data.up);
						
					}
				}
				else 
				{
					temp.last = node;
					temp.length = data.length++;
					openList.Add (data.up);
				}
			}

			if(data.down!=null && !closedList.Contains(data.down))
			{   
				NodeData temp = data.down.GetComponent("NodeData") as NodeData;
				if(openList.Contains(data.down))
				{
					if(temp.length < data.length+1) {}
					
					else if (temp.length > data.length+1)
					{
						openList.Remove(data.down);
						temp.last = node;
						temp.length = data.length++;
						openList.Add(data.down);
						
					}
				}
				else 
				{
					temp.last = node;
					temp.length = data.length++;
					openList.Add (data.down);
				}
			}

			if(data.left!=null && !closedList.Contains (data.left))
			{
				NodeData temp = data.left.GetComponent("NodeData") as NodeData;
				if(openList.Contains(data.left))
				{
					if(temp.length < data.length+1) {}
					
					else if (temp.length > data.length+1)
					{
						openList.Remove(data.left);
						temp.last = node;
						temp.length = data.length++;
						openList.Add(data.left);
						
					}
				}
				else 
				{
					temp.last = node;
					temp.length = data.length++;
					openList.Add (data.left);
				}
			}

			if(data.right!=null && !closedList.Contains(data.right))
			{
				NodeData temp = data.right.GetComponent("NodeData") as NodeData;
				if(openList.Contains(data.right))
				{
					if(temp.length < data.length+1) {}

					else if (temp.length > data.length+1)
					{
						openList.Remove(data.right);
						temp.last = node;
						temp.length = data.length++;
						openList.Add(data.right);

					}
				}
				else 
				{
					temp.last = node;
					temp.length = data.length++;
					openList.Add (data.right);
				}
			}
			 

			if(openList.Contains(node)) openList.Remove(node);
			if(!closedList.Contains(node)) closedList.Add (node);
			print ((GameObject)openList[0]);
			node = (GameObject)openList[0];
		}

		running = true;
		while(running)
		{
			path.Add (node);
			NodeData temp = node.GetComponent("NodeData") as NodeData;
			GameObject tN = node;
			node = temp.last;

			   temp = tN.GetComponent("NodeData") as NodeData;
			   temp.last = null;
			if (node == null) running = false;
		}

		path.Reverse ();
		string p = "";
		for (int i=0; i<path.Count; i++) {
			p += (GameObject)path[i] + " ";		
		}
		print (p);
	}
}
