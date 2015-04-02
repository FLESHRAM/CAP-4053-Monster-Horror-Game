using UnityEngine;
using System.Collections;

public class HidingObject : MonoBehaviour {


	public GameObject node;

	// Use this for initialization
	void Start () {
		node = null;
	}
	
	// Update is called once per frame
	void Update () {
	    if(node == null)
		{
			Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.36f);
			node = hit.gameObject;
		}
	}
}
