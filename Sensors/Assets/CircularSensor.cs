using UnityEngine;
using System.Collections.Generic;

public class CircularSensor : MonoBehaviour {
	
	private GameObject[] agentList;
	private GameObject anchor;
	public float sensorCooldown;
	private float nextSensor;
	
	// Use this for initialization
	void Start () {
		
		agentList = GameObject.FindGameObjectsWithTag("Agent");	
		anchor = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {
		
		Bounds bounds1 = GetComponent<SpriteRenderer>().bounds;
		Vector3 anchorPos =  anchor.transform.position;
		anchorPos.z = 2;
		transform.position = anchorPos;
		
		//Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//mousePos.z = 0;
		//transform.position = mousePos;
		
		if(Input.GetKey(KeyCode.Alpha1) && Time.time > nextSensor)
		{
			string message = "";

			Debug.Log(transform.renderer.bounds);

			List<GameObject> inRangeList = new List<GameObject>();
			for(int i = 0; i < agentList.Length; i++)
				if(bounds1.Intersects(agentList[i].renderer.bounds))
					inRangeList.Add(agentList[i]);

			if(inRangeList.Count == 0)
				message = "No agent in range";
			else
				for(int i = 0; i < inRangeList.Count; i++)
				{
					message += agentList[i].renderer.transform.position.ToString();
					message += (i == inRangeList.Count - 1 ? ", " : "");
				}

			Debug.Log(message);
			
			nextSensor = Time.time + sensorCooldown;
			
		}
	}
}
