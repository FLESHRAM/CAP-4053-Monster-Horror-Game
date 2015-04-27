using UnityEngine;
using System.Collections;

public class bombFunctions : MonoBehaviour {

	private bool marked = false;  

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





	public GameObject closestNode()
	{
		Collider2D[] nodes = Physics2D.OverlapCircleAll (transform.position, 3f, 1 << LayerMask.NameToLayer ("Node"));
		GameObject mostNear = null;
		if(nodes!=null)
		{
			for(int i=0; i<nodes.Length; i++)
			{
				if(mostNear == null) mostNear = nodes[i].gameObject;
				
				else
				{
					if(Vector3.Distance(transform.position, nodes[i].transform.position) < Vector3.Distance(transform.position, mostNear.transform.position))
						mostNear = nodes[i].gameObject;
				}
			}
		}
		
		return mostNear;
	}


	public void mark() { marked = true; }
	public void unMark() { marked = false; }
	public bool isMarked() { return marked; }

}
