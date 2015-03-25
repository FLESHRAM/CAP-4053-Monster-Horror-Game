using UnityEngine;
using System.Collections;

public class NodeFunctions : MonoBehaviour {

	public GameObject player;
	public GameObject floater;

	public bool SecondMode = false;


	public Sprite sprite;

	private NodeData[] nodes;
	public NodeData activeNode;

	public bool firstClick = false;
	public bool secondClick = false;
	public bool done = false;

	private SpriteRenderer rend;


	// Use this for initialization
	void Start () {
		if(!SecondMode) player.SetActive (false);
		floater.SetActive (false);
		nodes = GetComponentsInChildren<NodeData> ();

		//for (int i =0; i<nodes.Length; i++) 
		//{
		//	rend = nodes [i].gameObject.AddComponent<SpriteRenderer> ();
		//	rend.sprite = sprite;
		//}
		print (nodes [2].transform.position);

		if (SecondMode) 
		{
			firstClick = true;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (firstClick && secondClick && !done) 
		{  
			for (int i =0; i<nodes.Length; i++) 
			{
				rend = nodes [i].gameObject.GetComponent<SpriteRenderer>();
				rend.sprite = null;
			}
			done = true;
		}

		if(Vector3.Distance(player.transform.position, floater.transform.position) < 0.05f ) { floater.SetActive(false); }
	}

	public void recieveClick() {
		if (!firstClick) {
			testBrain brain = player.GetComponent("testBrain") as testBrain;
			firstClick = true;
			player.transform.position = activeNode.transform.position;
			brain.startNode = activeNode.gameObject;
			activeNode.Inhabited = player;
			rend = activeNode.GetComponent<SpriteRenderer>();
			rend.sprite = null;
		}

		else if (firstClick && !secondClick && activeNode.Inhabited == null && !SecondMode)
		{
			secondClick = true;
			floater.transform.position = activeNode.transform.position;
			activeNode.Inhabited = floater;
			floater.SetActive(true);
			player.SetActive(true);
		}

		else if (firstClick && !secondClick && activeNode.Inhabited == null && SecondMode)
		{
			senTest radar = player.GetComponent("senTest") as senTest;
			if (radar.prevNodes.Length > 0)
			{
				GameObject minDist = null;
				for(int i=0; i<radar.prevNodes.Length; i++)
				{
					if (minDist==null) minDist = radar.prevNodes[i].gameObject;
					else
					{   float nodeDist = Vector3.Distance(player.transform.position, radar.prevNodes[i].transform.position);
						float currDist = Vector3.Distance(player.transform.position, minDist.transform.position);
						if(nodeDist < currDist) minDist = radar.prevNodes[i].gameObject;
					}
				}

				secondClick = true;
				testBrain brain = player.GetComponent("testBrain") as testBrain;
				brain.startNode = minDist;
				NodeData min = minDist.GetComponent("NodeData") as NodeData;
				player.transform.position = minDist.transform.position;
				min.Inhabited = player;



				floater.transform.position = activeNode.transform.position;
				activeNode.Inhabited = floater;
				floater.SetActive(true);

				brain.Initiate ();
			}
		}
	}
}
