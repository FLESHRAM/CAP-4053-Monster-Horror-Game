﻿using UnityEngine;
using System.Collections;

public class senTest : MonoBehaviour {

	public Transform fStart, fEnd;
	public float radarRadius = 2f; 
	private RaycastHit2D prevHit;
	public Collider2D[] prevFloaters;
	public UnityEngine.UI.Text floaters;
	private string info;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Raycasting ();
		Radar();
	}



	void Raycasting() {

		if(prevHit.collider != null) prevHit.rigidbody.gameObject.renderer.material.color = Color.white;
		RaycastHit2D hit = Physics2D.Raycast (fStart.position, fEnd.position, Vector2.Distance (fStart.position, fEnd.position), 1 << LayerMask.NameToLayer("Obstacle"));

		if (hit.collider != null) {
			Debug.DrawLine(fStart.position, hit.point, Color.green);		
			hit.rigidbody.gameObject.renderer.material.color = Color.green;
			prevHit = hit;
		}
	}



	void Radar()
	{
		if (prevFloaters != null) 
		{   
			info = null;
			floaters.text = " ";

			for(int i = 0; i<prevFloaters.Length; i++)
			{
				prevFloaters[i].attachedRigidbody.gameObject.renderer.material.color = Color.white;
				print("Object " + prevFloaters[i].gameObject.name + " spotted at a distance of " + Vector2.Distance (transform.position, prevFloaters[i].transform.position));
				info = (info + " " + "Object " + prevFloaters[i].gameObject.name + " " + prevFloaters[i].transform.position +" spotted, Distance: " + Vector2.Distance (transform.position, prevFloaters[i].transform.position) + "\n");
			}

			floaters.text = info;
			System.Array.Clear(prevFloaters, 0, prevFloaters.Length);
		}

		Collider2D[] currFloaters = Physics2D.OverlapCircleAll (transform.position, radarRadius, 1 << LayerMask.NameToLayer ("Floater"));

		if (currFloaters != null) 
		{
			for(int i = 0; i<currFloaters.Length; i++)
			{
				currFloaters[i].attachedRigidbody.gameObject.renderer.material.color = Color.red;
			}

			prevFloaters = (UnityEngine.Collider2D[])currFloaters.Clone();
			System.Array.Clear(currFloaters, 0, currFloaters.Length);
		}
	}
}
