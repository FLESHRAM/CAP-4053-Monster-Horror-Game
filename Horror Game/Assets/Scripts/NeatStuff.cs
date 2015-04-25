using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;

public class NeatStuff : UnitController {

	/* Output actions (the [possible] action with the highest output value is selected):
	 * ** For each impossible action with a higher output than all possible actions,
	 * 	  the fitness will be punished with by -1.
	 * 
	 * * Group 1: move direction
	 * 		out[0]: move up
	 * 		out[1]: move right
	 * 		out[2]: move down
	 * 		out[3]: move left
	 * 
	 */

	// Stuff for NEAT
	bool IsRunning;
	IBlackBox box;
	float fitness;

	// For Fitness
	bool action_completed;						// Indicates if the last dispatched action is finished
	int top_impossible_actions;

	// Stuff for controlling the victims
	AISensors ais;								// This will control the brain

	// Use this for initialization
	void Start () {
		action_completed = true;				// There is no previous action, so just start this at true
		top_impossible_actions = 0;
		fitness = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (IsRunning) {
			// Check if their is an action dispatched and dispatch a new one if there isn't
			if(action_completed){
				// Update the fitness score
				this.SetIntermediateFitness();

				// Read the sensors
				//box.InputSignalArray = ais.getInput(box.InputSignalArray);

				// Activate the box (evalute the input with the Neural Network)
				box.Activate();

				// Evaluate the output and determine the next action
				// TODO
			}
			// If an action hasn't completed, we should just wait
		}
	
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

	// An intermediate fitness is measured after each action is completed
	public void SetIntermediateFitness()
	{
		fitness = this.fitness * .75f;					// Previous fitness is PARTIALLY factored into the next fitness

		// Check the distance from the monster
		float monsterD = 7 - ais.distance_from_monster;
		fitness += monsterD;

		// Check if we know where the monster is
		if (ais.see_monster) {
			fitness -= 1;

			if(ais.isHiding){
				fitness += 3;							// If we see the monster, "brave" AIs shouldn't be punished
			}
		}

		// Check if we are hiding (and if we should be)
		if (ais.isHiding && ais.isScared) {
			fitness += 3;
		}else if (ais.isHiding && !ais.isScared) {
			fitness -= 3;
		}else if (!ais.isHiding && ais.isScared) {
			fitness -= 3;
		}else if (!ais.isHiding && !ais.isScared) {
			fitness += 3;
		}


		// Subtract the number of consecutive turns from the fitness
		fitness -= ais.turn_count;

		// For each top scoring output that is not possible, reduce the fitness by 1
		fitness -= (top_impossible_actions * -1);

		// Set this.fitness
		this.fitness = fitness;
	}

	public override float GetFitness()
	{
		// Make sure that the fitness doesn't drop below 0
		if (this.fitness > 0)
			return this.fitness;
		else
			return 0;
	}
}
