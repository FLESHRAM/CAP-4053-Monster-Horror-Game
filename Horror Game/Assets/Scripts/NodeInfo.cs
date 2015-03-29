using UnityEngine;
using System.Collections;

public class NodeInfo : MonoBehaviour {
	
	public GameObject Inhabited;
	
	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;
	
	public int length = 0;
	public GameObject last;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}




	public void disconnectUp() { up = null; }
	public void disconnectDown() { down = null; }
	public void disconnectLeft() { left = null; }
	public void disconnectRight() { right = null; }


}

