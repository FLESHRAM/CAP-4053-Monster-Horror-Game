using UnityEngine;
using System.Collections;

public class tileSensor : MonoBehaviour {


	private int chanceOfVictim = 20;

	// Use this for initialization
	void Start () {
	 
		GameObject d = GameObject.Find("AI Director");
		if(d!=null)
		{
			Director dir = d.GetComponent("Director") as Director;

		}
	   
	}

	
	// Update is called once per frame
	void Update () {
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.64f, 1 << LayerMask.NameToLayer("Player"));

		if(hit!=null)
		{
			stats pStats = hit.gameObject.GetComponent("stats") as stats;
			if(!pStats.isMonster)
			{
				PlayerControl p = hit.gameObject.GetComponent("PlayerControl") as PlayerControl;
				RuntimeAnimatorController con = (RuntimeAnimatorController)Resources.Load ("Demon", typeof(RuntimeAnimatorController));
				p.transformation(con, gameObject);
				
				
				pStats.health += 100;
				pStats.isMonster = true;
			}

		}

		else
		{
			int rand = Random.Range(1, 101);
			hit = Physics2D.OverlapCircle(transform.position, 0.64f, 1 << LayerMask.NameToLayer("Victim"));
			if(hit!=null && rand<chanceOfVictim)
			{
				stats vStats = hit.gameObject.GetComponent("stats") as stats;
				if(!vStats.isMonster)
				{
					Brain b = hit.gameObject.GetComponent("Brain") as Brain;
					RuntimeAnimatorController con = (RuntimeAnimatorController)Resources.Load ("Demon", typeof(RuntimeAnimatorController));
					b.transformation(con, gameObject);
					
					
					vStats.health += 100;
					vStats.isMonster = true;
				}
				
			}
		}
	}
}
