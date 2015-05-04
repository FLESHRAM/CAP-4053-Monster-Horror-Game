using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {


	public bool isMonster;
	public float health;
	public bool isPlayer;
	public GameObject bomb = null;
	public bool hasBomb = false;

	private bool girl = true;


	void Awake()
	{
		health = 100f;
		bomb = (GameObject)Resources.Load ("bomb", typeof(GameObject));
	}



	public void setMan() { girl = false; }



	public void damage(GameObject Player)
	{
		health -= 50;

		if (isMonster && health==100) loseForm(); 

		checkLife ();
		Neurons n = gameObject.GetComponent ("Neurons") as Neurons;

		if(n!=null) { n.seePlayer(Player); }
	}

	public void bombDamage()
	{
		health -= 100;
		if(health < 0) health = 0;

		checkLife ();
	}


	private void checkLife()
	{
		if (health == 0) 
		{
			GameObject gore;
			if(isPlayer) { gore = (GameObject)Resources.Load("Gore/Doctor Gore", typeof(GameObject)); }
			else
			{
				if(girl) { gore = (GameObject)Resources.Load("Gore/Girl Gore", typeof(GameObject)); }
				else { gore = (GameObject)Resources.Load("Gore/Man Gore", typeof(GameObject)); }
			}

			GameObject setGore = (GameObject)Instantiate(gore);
			setGore.transform.position = new Vector2(transform.position.x, transform.position.y);
			Destroy (gameObject);
		}
			
	}


	private void loseForm()
	{
		if(isPlayer)
		{
			PlayerControl c = gameObject.GetComponent("PlayerControl") as PlayerControl;
			c.loseForm();
		}

		else
		{
			Brain b = gameObject.GetComponent("Brain") as Brain;
			b.loseForm();
		}
	}


}
