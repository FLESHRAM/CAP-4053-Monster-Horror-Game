using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {




	public void fragmentAnalyzer()
	{
		NodeInfo[] nodes = GetComponentsInChildren<NodeInfo> ();
		GameObject Cabinet = (GameObject)Resources.Load ("Cabinet", typeof(GameObject));
		GameObject Bomb = (GameObject)Resources.Load ("bomb", typeof(GameObject));


		for(int i=0; i<nodes.Length; i++)
		{
			bool spawn=false;
			bool bomb=false;

			int s = Random.Range(1, 101);
			int b = Random.Range (1, 101);


			if(s>50) spawn = true;
			if(b<51) bomb=true;

			if(spawn)
			{
				if(hasPatternOne(nodes[i])) { Spawn(nodes, bomb, Bomb, Cabinet, i, 90); }

				else if(hasPatternTwo(nodes[i])) {}
				else if (hasPatternThree(nodes[i])) {}
				else if(hasPatternFour(nodes[i])) {}
			}

		}

	}




	private void Spawn(NodeInfo[] nodes, bool bomb, GameObject Bomb, GameObject Cabinet, int i, int rotation)
	{
		GameObject Objects = GameObject.Find ("Objects");

		GameObject spawner;

		if(bomb) spawner = (GameObject)Instantiate(Bomb);
		else spawner = (GameObject)Instantiate(Cabinet);
		
		spawner.transform.position= new Vector2(nodes[i].transform.position.x, nodes[i].transform.position.y);
		if(Objects!=null) spawner.transform.parent = Objects.transform;
		
		if(!bomb) spawner.transform.rotation = Quaternion.Euler(0,0,rotation);
	}
	
	
	
	public bool hasPatternOne(NodeInfo node)
	{
		if(node!=null)
		{
			if(node.up==null && node.left == null && node.right==null && node.down!=null)
				return true;
		}

		return false;
	}


	public bool hasPatternTwo(NodeInfo node)
	{
		if(node!=null)
		{
			if(node.up!=null && node.left == null && node.right==null && node.down==null)
				return true;
		}

		return false;
	}

	public bool hasPatternThree(NodeInfo node)
	{
		if(node!=null)
		{
			if(node.up==null && node.left != null && node.right==null && node.down==null)
				return true;
		}

		return false;
	}

	public bool hasPatternFour(NodeInfo node)
	{
		if(node!=null)
		{
			if(node.up==null && node.left == null && node.right!=null && node.down==null)
				return true;
		}

		return false;
	}
}
