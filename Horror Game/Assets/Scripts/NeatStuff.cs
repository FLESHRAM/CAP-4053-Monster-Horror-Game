using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class NeatStuff : UnitController {



	// Stuff for NEAT
	bool IsRunning;
	IBlackBox box;

	// Stuff for controlling the victims
	AISensors ais;								// This will control the brain

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
		float fitness = 0;
		// Brandon's fitness measure, distance from goal
		// TODO:

		// Subtract the number of consecutive turns from the fitness
		fitness -= ais.turn_count;

		// Lastly, make sure that the fitness doesn't drop below 0
		if (fitness > 0)
			return fitness;
		else
			return 0;
	}
}
