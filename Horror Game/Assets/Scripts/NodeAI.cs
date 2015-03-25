using UnityEngine;
using System.Collections;

public class NodeAI : MonoBehaviour {

	public GameObject start;
	private ArrayList nodesAbove;

	// Use this for initialization
	void Start () {
		if(start != null)
		{
			nodesAbove = new ArrayList();
			generateNodes ();
		}

		GameObject temp = (GameObject)Instantiate (start);

		//temp.transform.position = new Vector3(strt.x+1, strt.y, strt.z);
		temp.name = "01";

		NodeInfo t = temp.GetComponent ("NodeInfo") as NodeInfo;
		NodeInfo sT = start.GetComponent ("NodeInfo") as NodeInfo;

		t.left = start;
		sT.right = temp;

		temp.transform.parent = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void generateNodes()
	{
		Vector3 sPos = start.transform.position;
		ArrayList currNodes = new ArrayList ();
		//int xBound = Mathf.Abs (sPos.x);
		//int yBound = Mathf.Abs (sPos.y);
		GameObject last = start;


		//for(int i=yBound; i>(-1*yBound); i--)
		//{

		//}
	}








}
