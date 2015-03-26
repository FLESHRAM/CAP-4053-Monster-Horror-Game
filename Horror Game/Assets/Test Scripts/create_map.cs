using UnityEngine;
using System.Collections;

public class create_map : MonoBehaviour {

	public Transform floor_9;
	public Transform floor_8;
	public Transform floor_7;
	public Transform floor_6;
	public Transform floor_5;
	public Transform floor_4;
	public Transform floor_3;
	public Transform floor_2;
	public Transform floor_1;

	// Use this for initialization
	void Start () {
		for (float x = -9f; x < 10f; x += 0.6f) {
			Instantiate (floor_1, new Vector3 (x, 5.0f, 0.0f), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
