using UnityEngine;
using System.Collections;

public class Neurons : MonoBehaviour {


	private Brain brain;
	private NodeAI nAI;

	private bool checkDone = false;
	private int wait = 0;
	private int count;


	// Use this for initialization
	void Start () {
	
		GameObject nod = GameObject.Find ("Nodes");
		nAI = nod.GetComponent ("NodeAI") as NodeAI;
		brain = gameObject.GetComponent ("Brain") as Brain;

		count = -1;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if(nAI.nodesDone && !checkDone && brain.IsRunning)
		{
			print ("Nodes were finished, brain is done");

			checkDone = true;
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





	private void goodOlFashionAI()
	{
		
	}
}
