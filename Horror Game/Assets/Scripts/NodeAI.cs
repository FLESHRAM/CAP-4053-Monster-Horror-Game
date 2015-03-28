using UnityEngine;
using System.Collections;

public class NodeAI : MonoBehaviour {

	public GameObject start;
	private ArrayList nodesAbove;

	// Use this for initialization
	void Start () {


			nodesAbove = new ArrayList();
			generateNodes ();
		    
	}
	
	// Update is called once per frame
	void Update () {
	
	}





	void nameNodes ()
	{
		int count=0;
		NodeData[] nodes = GetComponentsInChildren<NodeData> ();

		for (int i=1; i<nodes.Length; i++) 
		{
			count++;
			string n = "" + count;
			nodes[i].gameObject.name = n;
		}
	}




	void generateNodes()
	{
		int count = 0;
		Vector3 sPos = start.transform.position;
		ArrayList currNodes = new ArrayList ();
		currNodes.Add (start);

		float xBound = sPos.x;
		     float xOffset = Mathf.Abs (xBound);
		float yBound = sPos.y;
		GameObject last = start;
		GameObject temp;
		print (-1 * xBound);





		for(float i=yBound; i>=(-1*yBound); i--)
		{
			if(nodesAbove.Count > 0)
			{
				setAbove ((int)xOffset, (int)(sPos.x), last);
			}


			for(float j=sPos.x; j<(-1*sPos.x); j++)
			{
				temp = (GameObject)Instantiate (last);
				temp.transform.position = new Vector3(j+1, last.transform.position.y, 0);
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
					setAbove ((int)xOffset, (int)(j+1), last);
			    }

			   
		  }

			nodesAbove.Clear();
			nodesAbove.AddRange(currNodes);
			currNodes.Clear();

			temp = (GameObject)Instantiate (last);
			count++;
			temp.name = count.ToString();
			temp.transform.position = new Vector3(sPos.x, i-1, 0);
			temp.transform.parent = gameObject.transform;
			currNodes.Add (temp);
			last = temp;
			print (nodesAbove);
		}
	}





	void setAbove (int offset, int curr, GameObject last)
	{
		int spot = (int)(curr) + (int)offset;
		GameObject temp = (GameObject)nodesAbove[spot];
		
		
		NodeInfo t = temp.GetComponent ("NodeInfo") as NodeInfo;
		NodeInfo sT = last.GetComponent ("NodeInfo") as NodeInfo;
		
		t.down = last;
		sT.up = temp;
		
	}
	
	
	
	
	
	
}
