using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {


	public bool isMonster;
	public float health;
	public bool isPlayer;
	public GameObject bomb = null;



	public void damage()
	{
		health -= 50;
		checkLife ();
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
			Destroy (gameObject);
	}
}
