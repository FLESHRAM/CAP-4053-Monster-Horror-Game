using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {


	private ArrayList Victims;
	private GameObject Player;
	private int level=1;
	private bool gameRunning=false;

	private GameObject dead;
	private GameObject win;


	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.F10)) { level++; }

		if(gameRunning && Victims.Count==0)
		{
			if(win!=null)
			{
				win.SetActive(true);
				gameRunning = false;
				level++;
			}
		}


		if(gameRunning && Player==null)
		{
			if(dead != null)
			{
				print ("Player is dead");
				dead.SetActive(true);
				gameRunning = false;
			}

			else print ("Dead Screen Not Found");
		}
	} 


	public void Refresh()
	{
		if(Victims!=null) Victims.Clear ();

		Victims = null;
		Victims = new ArrayList ();

		GameObject V = GameObject.Find ("Victims");
		if(V!=null)
		{
			Neurons[] Vics = V.GetComponentsInChildren<Neurons>();
			print ("Found "+Vics.Length+" victims.");

			for(int i=0; i<Vics.Length; i++)
			{
				setBravery(Vics[i]);
				Victims.Add(Vics[i].gameObject);
			}
		}


		Player = GameObject.FindGameObjectWithTag("Player");

		GameObject canvas = GameObject.Find ("Canvas");
		loadLevel lev = canvas.GetComponent ("loadLevel") as loadLevel;
		dead = lev.dead;
		win = lev.win;



		gameRunning = true;

		print ("Finished setting up the game");
	}




	private void setBravery (Neurons n)
	{
		//Neurons n = victim.GetComponent ("Neurons") as Neurons;
		int bravery = (level * 10);
		n.setBravery (bravery);
	}


	public void setTransformRate(tileSensor sensor)
	{
		int chance = (level * 10);
		sensor.victimChance (chance);
	}



	public int victimsToSpawn()
	{
		int num = (level * 5);
		if(num > 40) num = 40;

		return num;
	}


	public int victimChanceOfSpawn()
	{
		if(level < 6) return 35;
		else if (level>5 && level<9) return 20;
		else return 10;
	}


	public void killVictim(GameObject vic)
	{
		if (Victims.Contains (vic)) Victims.Remove (vic);
	}


	public int demonTiles()
	{
		if(level < 3) return 7;
		else if(level>=3 && level<=5) return 10;
		else if (level>5 && level<10) return 15;
		else return 20;
	}

}
