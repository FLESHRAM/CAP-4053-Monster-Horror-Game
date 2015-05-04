using UnityEngine;
using System.Collections;

public class Director : MonoBehaviour {


	private ArrayList Victims;
	private int level=1;


	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	} 


	public void Refresh()
	{
		if(Victims!=null) Victims.Clear ();

		Victims = null;
		Victims = new ArrayList ();

		GameObject V = GameObject.Find ("Victims");
		if(V!=null)
		{
			Neurons[] Vics = V.GetComponentsInChildren<Neurons>();
			print ("Found "+Vics.Length+" victims.");


		}


	}
}
