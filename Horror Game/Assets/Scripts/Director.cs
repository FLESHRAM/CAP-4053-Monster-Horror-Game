using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {

	GameObject

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
		if(Application.loadedLevel == 1)
		{

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
