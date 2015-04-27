using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {


	private int bombSpawnThresh = 51;
	private int spawnThresh = 50;
	private int demonTileSpawn = 7;
	private int victimsSpawn = 10;



	public void fragmentAnalyzer()
	{
		NodeInfo[] nodes = GetComponentsInChildren<NodeInfo> ();
		GameObject Cabinet = (GameObject)Resources.Load ("Cabinet", typeof(GameObject));
		GameObject Bomb = (GameObject)Resources.Load ("bomb", typeof(GameObject));
		GameObject Tile = (GameObject)Resources.Load ("Demon Tile", typeof(GameObject));

		int victims = victimsSpawn;
		int player = 1;


		for(int i=0; i<nodes.Length; i++)
		{
			bool spawn=false;
			bool bomb=false;

			int s = Random.Range(1, 101);
			int b = Random.Range (1, 101);


			if(s>spawnThresh) spawn = true;
			if(b<bombSpawnThresh) bomb=true;

			if(spawn)
			{
				if(hasPatternOne(nodes[i]) && !bomb) { Spawn(nodes[i], bomb, Bomb, Cabinet, 90); }
				else if(hasPatternTwo(nodes[i]) && bomb) { Spawn(nodes[i], bomb, Bomb, Cabinet, 0); }
				else if (hasPatternThree(nodes[i]) && victims>0) { if((Random.Range (1, 101))>30) { spawnVictim(nodes[i]); victims--; } }
				else if(hasPatternFour(nodes[i]) && player>0) { spawnPlayer(nodes[i]); player--; }
				else if (hasPatternFive(nodes[i]) && (Random.Range (1, 101))<demonTileSpawn) { Spawn(nodes[i], true, Tile, Cabinet, 0); }
			}

		}

	}




	private void Spawn(NodeInfo node, bool bomb, GameObject Bomb, GameObject Cabinet, int rotation)
	{
		GameObject Objects = GameObject.Find ("Objects");

		GameObject spawner;

		if(bomb) spawner = (GameObject)Instantiate(Bomb);
		else spawner = (GameObject)Instantiate(Cabinet);
		
		spawner.transform.position= new Vector2(node.transform.position.x, node.transform.position.y);
		if(Objects!=null) spawner.transform.parent = Objects.transform;
		
		if(!bomb) spawner.transform.rotation = Quaternion.Euler(0,0,rotation);
	}
	


	private void spawnVictim(NodeInfo node)
	{
		GameObject victim = (GameObject)Resources.Load ("Template Victim", typeof(GameObject));
		GameObject Victims = GameObject.Find ("Victims");

		if(node != null)
		{
			GameObject v = (GameObject)Instantiate(victim);
			v.transform.position = new Vector2(node.transform.position.x, node.transform.position.y);
			if(Victims != null) v.transform.parent = Victims.transform;
		}
	}


	private void spawnPlayer(NodeInfo node)
	{
		GameObject player = (GameObject)Resources.Load ("Player", typeof(GameObject));
		GameObject camera = GameObject.Find ("Main Camera");
		TrackPlayer track = camera.GetComponent ("TrackPlayer") as TrackPlayer;

		if(node != null)
		{
			GameObject p = (GameObject)Instantiate(player);
			p.transform.position = new Vector2(node.transform.position.x, node.transform.position.y);
			PlayerControl pC = p.GetComponent("PlayerControl") as PlayerControl;
			pC.up = GameObject.Find("Up Direction");  pC.down=GameObject.Find ("Down Direction");  
			pC.left=GameObject.Find ("Left Direction"); pC.right=GameObject.Find ("Right Direction"); 
			track.player = p;
		}
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



	public bool hasPatternFive(NodeInfo node)
	{
		if(node!=null)
		{
			if(node.up!=null && node.left != null && node.right!=null && node.down!=null)
				return true;
		}
		return false;
	}


}
