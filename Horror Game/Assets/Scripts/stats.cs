using UnityEngine;
using System.Collections;

public class stats : MonoBehaviour {


	public bool isMonster;
	public float health;
	public bool isPlayer;



	public void damage()
	{
		health -= 50;
		checkLife ();
	}


	private void checkLife()
	{
		if (health == 0)
			Destroy (gameObject);
	}
}
