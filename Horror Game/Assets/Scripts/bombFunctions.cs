using UnityEngine;
using System.Collections;

public class bombFunctions : MonoBehaviour {

      

	public void explode()
	{
		Animator ex = gameObject.GetComponent<Animator> ();
		ex.SetBool ("Exploded", true);
	}



	public void doDamage()
	{
		Collider2D player = Physics2D.OverlapCircle (transform.position, 2f, 1 << LayerMask.NameToLayer("Player"));
		Collider2D[] victims = Physics2D.OverlapCircleAll(transform.position, 2f, 1 << LayerMask.NameToLayer("Victim"));
		
		
		stats pStats = player.GetComponent ("stats") as stats;
		pStats.bombDamage ();
		if(pStats.isMonster) 
		{
			PlayerControl p = player.GetComponent("PlayerControl") as PlayerControl;
			p.loseForm();
		}

		for(int i = 0; i<victims.Length; i++)
		{
			stats vStats = victims[i].GetComponent("stats") as stats;
			vStats.bombDamage();
		}
	}


	public void finishExplosion()
	{ Destroy (gameObject); }
}
