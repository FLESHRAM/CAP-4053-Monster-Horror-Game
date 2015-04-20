using UnityEngine;
using System.Collections;

public class HidingObject : MonoBehaviour {


	public GameObject node;
	private GameObject prevSeen;

	// Use this for initialization
	void Start () {
		node = null;
		prevSeen = null;
	}
	
	// Update is called once per frame
	void Update () {
	    if(node == null)
		{
			Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.36f);
			node = hit.gameObject;
		}

		Collider2D seenPlayer = Physics2D.OverlapCircle (transform.position, 0.30f, 1 << LayerMask.NameToLayer ("Player"));
		Collider2D seenVictim = Physics2D.OverlapCircle (transform.position, 0.30f, 1 << LayerMask.NameToLayer("Victim"));
		bool seen = (seenPlayer != null || seenVictim != null);
		if(prevSeen == null && seen)
		{
			if (seenPlayer!=null && seenVictim==null) { prevSeen = seenPlayer.gameObject; }
			else if(seenPlayer==null && seenVictim!=null) { prevSeen = seenVictim.gameObject; }
			prevSeen.renderer.material.color = Color.clear;
		}

		else if(prevSeen!=null && !seen)
		{
			prevSeen.renderer.material.color = Color.white;
			prevSeen = null;
		}


	}





	public void setTrap(GameObject bomb)
	{
		bomb.renderer.material.color = Color.clear;
		bomb.transform.position = new Vector2 (node.transform.position.x, node.transform.position.y);
		prevSeen = bomb;
	}

}
