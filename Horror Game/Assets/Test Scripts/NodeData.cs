using UnityEngine;
using System.Collections;

public class NodeData : MonoBehaviour {

	public GameObject Inhabited;

	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;

	public int length = 0;
	public GameObject last;
	
	private GameObject[] dir = new GameObject[4];
	public int[] connected = new int[4] {-1, -1, -1, -1};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

	void OnMouseOver() {
		NodeFunctions n = GetComponentInParent<NodeFunctions> ();
		testBrain b = n.player.GetComponent ("testBrain") as testBrain;
		SpriteRenderer rend = gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
		if (n.firstClick && !b.done) { rend.sprite = n.sprite; renderer.material.color = Color.yellow; } 
		else if (!n.firstClick && !n.secondClick) { rend.sprite = n.sprite; renderer.material.color = Color.red;}

	}


	void OnMouseExit() {
		NodeFunctions n = GetComponentInParent<NodeFunctions> ();
		testBrain b = n.player.GetComponent ("testBrain") as testBrain;
		SpriteRenderer rend = gameObject.GetComponent("SpriteRenderer") as SpriteRenderer;
		if(!b.done){rend.sprite = null; renderer.material.color = Color.white; }
	}

	void OnMouseUpAsButton() {
		NodeFunctions n = GetComponentInParent<NodeFunctions> ();
		n.activeNode = this;
		n.recieveClick ();
	}

	public void debugLines(string n)
	{
		NodeData data;
		dir [0] = up;
		dir [1] = down;
		dir [2] = left;
		dir [3] = right;

		for (int i=0; i<4; i++) {
			        if(dir[i] != null && connected[i]!=i)
			        {   
				        data = dir[i].GetComponent("NodeData") as NodeData;
				        Debug.DrawLine (transform.position, dir[i].transform.position, Color.green, 50f);
				        connected[i] = i;
				            if(i==0) data.connected[1]=1; 
				            else if(i==1) data.connected[0]=0;
				            else if(i==2) data.connected[3]=3;
				            else if(i==3) data.connected[2]=2;
				        data.debugLines(this.name);
			        }
				}



	}



}
