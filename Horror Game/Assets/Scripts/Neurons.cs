using UnityEngine;
using System.Collections;

public class Neurons : MonoBehaviour {


	private Brain brain;
	private NodeAI nAI;

	private bool checkDone = false;

	private bool Idle;
	private bool Scanning;
	private bool Wandering;
	private bool Walking;
	private bool pickingUpBomb;
	private bool runningAway;
	private bool hiding;
	private bool Panic;


	private ArrayList hidingObjectMemory;
	private ArrayList bombMemory;


	private int wait;
	private int count;



	// Use this for initialization
	void Start () {
	
		GameObject nod = GameObject.Find ("Nodes");
		nAI = nod.GetComponent ("NodeAI") as NodeAI;
		brain = gameObject.GetComponent ("Brain") as Brain;
		hidingObjectMemory = new ArrayList ();
		bombMemory = new ArrayList ();



		Idle = true;
		Scanning = false;
		Wandering = false;
		pickingUpBomb = false;
		runningAway = false;
		hiding = false;
		Panic = false;

		wait = 0;
		count = 0;

	}
	
	// Update is called once per frame
	void Update () 
	{
	
		ArrayList tempHiding = brain.hidingObjects ();
		ArrayList tempBombs = brain.getVisisbleBombs ();

		remember (hidingObjectMemory, tempHiding);
		remember (bombMemory, tempBombs);


		if(nAI.nodesDone && brain.IsRunning)
		{
			goodOlFashionAI();
		}
	}



	// UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3
	private void check(int dir)
	{
		GameObject n = brain.closestNode ();
		NodeInfo nI;
		if(n!=null)
		{
			nI = n.GetComponent("NodeInfo") as NodeInfo;
			switch (dir)
			{
			  case 0: if(nI.up != null) brain.seek (n, nI.up); break;
			  case 1: if(nI.down != null) brain.seek (n, nI.down); break;
			  case 2: if(nI.left != null) brain.seek (n, nI.left); break;
			  case 3: if(nI.right != null) brain.seek (n, nI.right); break;
			}

		}
	}




	private void remember(ArrayList mem, ArrayList input)
	{
		if(input.Count > 0)
		{
			for(int i=0; i<input.Count; i++)
			{
				if (!mem.Contains((GameObject)input[i])) mem.Add((GameObject)input[i]);
			}
		}
	}



	private void goodOlFashionAI()
	{
		if(wait == 0)
		{
			if (Idle) 
			{
				Scanning = true;
				Idle = false;
				count = 0;
			}
			
			if(Scanning)
			{
				if(count<4) { check(count); wait=20; count++;} 
				if(count == 4) { wait=0; Scanning=false; Wandering=true;}
			}

			if(Wandering)
			{
				int chance = Random.Range(1, 101);
				if(chance>20 && chance<56) { Wandering=false; Idle=true; wait=15; }
				else
				{
					ArrayList n = brain.getVisibleNodes();
					if(n.Count == 0) { Wandering=false; Idle=true; }
					else if (n.Count > 0)
					{
						brain.seek (brain.closestNode(), (GameObject)n[Random.Range (0, n.Count)]);
						Wandering=false; Walking=true; wait=5;
					}
				}
			}


			if(Walking)
			{
				if (!brain.pathing) { Walking=false; Wandering=true; wait=5; }
			}




		}


		if(wait > 0) wait--;

	}
	
}
