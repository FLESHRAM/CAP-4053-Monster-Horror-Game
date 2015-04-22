using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class NeatStuff : UnitController {



	// Stuff for NEAT
	bool IsRunning;
	IBlackBox box;

	// Stuff for controlling the victims
	Brain brain;
	AISensors ais;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	public override void Stop()
	{
		this.IsRunning = false;
	}
	
	public override void Activate(IBlackBox box)
	{
		this.box = box;
		this.IsRunning = true;
	}
	
	public override float GetFitness()
	{
		// Brandon's fitness measure, distance from goal
		// TODO:
		
		return 0;
	}
}
