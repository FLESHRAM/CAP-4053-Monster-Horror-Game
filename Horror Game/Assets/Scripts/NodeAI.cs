using UnityEngine;
using System.Collections;

public class NodeAI : MonoBehaviour {

	public GameObject start;
	private ArrayList nodesAbove;
	private ArrayList currNodes;

	// Use this for initialization
	void Start () {

		MapMaker maker = gameObject.GetComponent ("MapMaker") as MapMaker;
		maker.Initialize ();

			nodesAbove = new ArrayList();
		    currNodes = new ArrayList ();
			generateNodes ();
		    deleteCollisions();

		GameObject Girl = GameObject.Find ("Girl");
		Brain girlBrain = Girl.GetComponent("Brain") as Brain;
		girlBrain.testSight ();
	}

	// Update is called once per frame
	void Update () {

	}





	void deleteCollisions ()
	{
		NodeInfo[] nodes = GetComponentsInChildren<NodeInfo> ();
		ArrayList collisions = new ArrayList ();


		for (int i=1; i<nodes.Length; i++) 
		{
			Collider2D[] obstacles = Physics2D.OverlapCircleAll (nodes[i].transform.position, 0.25f, 1 << LayerMask.NameToLayer ("Obstacle"));
			if (obstacles.Length > 0) collisions.Add (nodes[i]);
		}

		for (int i=0; i<collisions.Count; i++) 
		{
			NodeInfo curr = (NodeInfo)collisions[i];
			NodeInfo temp = null;

			if (curr.up!=null) { temp = curr.up.GetComponent("NodeInfo") as NodeInfo; temp.disconnectDown(); }
			if (curr.down!=null) { temp = curr.down.GetComponent("NodeInfo") as NodeInfo; temp.disconnectUp(); }
			if (curr.left!=null) { temp = curr.left.GetComponent("NodeInfo") as NodeInfo; temp.disconnectRight(); }
			if (curr.right!=null) { temp = curr.right.GetComponent("NodeInfo") as NodeInfo; temp.disconnectLeft(); }

			Destroy(curr.gameObject);

		}
	}




	void generateNodes()
	{
		int count = 0;
		Vector3 sPos = start.transform.position;
		currNodes.Add (start);

		float xBound = sPos.x;
		     float xOffset = Mathf.Abs (xBound);
		float yBound = sPos.y;
		GameObject last = start;
		GameObject temp = null;
		print (-1 * xBound);





		for(float i=yBound; i>=(-1*yBound); i-=0.64f)
		{
			if(nodesAbove.Count > 0)
			{
				setAbove (currNodes.IndexOf(last), last);
			}


			for(float j=sPos.x; j<(-1*sPos.x); j+=0.64f)
			{
				temp = (GameObject)Instantiate (last);
				temp.transform.position = new Vector3((float)(last.transform.position.x+0.64), last.transform.position.y, 0);
				count++;
				temp.name = count.ToString();

				NodeInfo t = temp.GetComponent ("NodeInfo") as NodeInfo;
				NodeInfo sT = last.GetComponent ("NodeInfo") as NodeInfo;

				t.left = last;
				sT.right = temp;

				temp.transform.parent = gameObject.transform;
			    currNodes.Add(temp);
			    last = temp;
			
			if(nodesAbove.Count > 0)
			    {
					setAbove (currNodes.IndexOf (last), last);
			    }

			   
		  }

			nodesAbove.Clear();
			nodesAbove.AddRange(currNodes);
			currNodes.Clear();

			temp = (GameObject)Instantiate (last);
			count++;
			temp.name = count.ToString();
			temp.transform.position = new Vector3(sPos.x, (float)(sPos.y-0.64), 0);
			sPos = temp.transform.position;
			temp.transform.parent = gameObject.transform;
			currNodes.Add (temp);
			last = temp;
			print (nodesAbove);
		}

		if(temp!=null) Destroy (temp);
	}





	void setAbove (int index, GameObject last)
	{
		//int spot = (int)(curr) + (int)offset;
		GameObject temp = (GameObject)nodesAbove[index];
		
		
		NodeInfo t = temp.GetComponent ("NodeInfo") as NodeInfo;
		NodeInfo sT = last.GetComponent ("NodeInfo") as NodeInfo;
		
		t.down = last;
		sT.up = temp;
		
	}
	
	
	
	
	
	
}
