﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SharpNeat.Phenomes;							// For the IBlackBox.InputSignalArray

public class AISensors : MonoBehaviour {
	public int dir = -1;							// -1: unknown; 0: N; 1: W; 2: S; 3: E
	public int prev_dir = -1;
	public int left_count;
	public int right_count;
	public int back_count;

	private GameObject sight;
	private float sightRadius = 6;
	private Brain brain;

	/* For Measuing Fitness */
	private bool did_turn;								// Indicates if the AI changed direction for its last action
	public int turn_count;								// Indicates the number of CONSECUTIVE turns
	public int distance_from_monster;
	public bool see_monster;
	public bool facing_monster;
	public bool isFleeing;								// Indicates that monster was recently seen (but we are not looking at now)
	public bool hasSprint;								// Indicates that sprint is NOT on cooldown
	public bool isSprinting;							// Indicates 
	public bool isHiding;
	public bool isScared;								// Indicates a personallity that likes to hide
	public bool has_bomb;
	public bool placed_bomb;							
	public bool is_powerful;
	
	// Use this for initialization
	void Start () {
		// Set our brain!
		brain = this.GetComponent<Brain> ();

		// Set our initial direction
		dir = this.getDirection ();
		prev_dir = dir;								// "-1" will cause issues...

		// Initialize other stuff...
		distance_from_monster = 7;				// Start at some maximum distance (7?)
		see_monster = false;
		facing_monster = false;
		isHiding = false;
//		isScared = brain.						// TODO:
		has_bomb = false;
		is_powerful = false;
		
		// Testing:
		brain.IsRunning = true;

		turn_count = 0;
		left_count = 0;
		right_count = 0;
		turn_count = 0;
		this.seekAbsoluteDirection (3);
	}
	
	// Update is called once per frame
	void Update () {
//		// Left sensor
//		bool left_glance = glanceDirection (false, 2.0f);
//		Debug.Log ("left: " + left_glance);
//
//		// Right sensor
//		bool right_glance = glanceDirection (true, 2.0f);
//		Debug.Log ("right: " + right_glance);

		// Get path count
//		this.getPathCount ();

//		// Test Direction
//		int direct = this.getDirection ();
//		if (direct == 0)
//						Debug.Log ("North");
//		if (direct == 1)
//						Debug.Log ("East");
//		if (direct == 2)
//						Debug.Log ("South");
//		if (direct == 3)
//						Debug.Log ("West");
	
	}


	public ISignalArray getInput(ISignalArray input){
		// Do we see the monster?
		// TODO

		// Inputs
		input [0] = this.getPathCount ();				// Forward path count
		input [1] = this.glanceDirection (false, 3);
		input [2] = this.glanceDirection (true, 3);
		input [3] = (float) (this.back_count / 24);		// TODO
		input [4] = this.monsterDirection();
		input [5] = this.seeBomb ();
		input [6] = this.seeCabinet ();
//		input [7] = this.seePowerUp ();
		input [7] = this.getHealth ();
		input [8] = this.hasSprintA ();
		if (has_bomb)
			input [9] = 1.0f;
		else
			input [9] = 0.0f;

		return input;
	}


	/* ANN output functions */

	public void moveForward()
	{
		this.turn_count = 0;			// It is OK to reset this if we move in the same direction twice
		this.did_turn = true;
		this.prev_dir = this.dir;
		// Determine which node we need to seek to
		int new_dir = this.relativeToAbsoluteDirection (0);
		this.seekAbsoluteDirection (new_dir);
		Debug.Log ("seeking Forward");
	}

	public void moveRight()
	{
		if(this.did_turn)
		{
			this.turn_count++;
		}
		this.did_turn = true;
		this.prev_dir = this.dir;
		// Determine which node we need to seek to
		int new_dir = this.relativeToAbsoluteDirection (1);
		this.seekAbsoluteDirection (new_dir);
	}

	public void moveBack()
	{
		if(this.did_turn)
		{
			this.turn_count++;
		}
		this.did_turn = true;
		this.prev_dir = this.dir;
		// Determine which node we need to seek to
		int new_dir = this.relativeToAbsoluteDirection (2);
		this.seekAbsoluteDirection (new_dir);
	}

	public void moveLeft()
	{
		if(this.did_turn)
		{
			this.turn_count++;
		}
		this.did_turn = true;
		this.prev_dir = this.dir;
		// Determine which node we need to seek to
		int new_dir = this.relativeToAbsoluteDirection (3);
		this.seekAbsoluteDirection (new_dir);
	}

	public void goHide(double selection){
		// Get a list of all hiding objects
		ArrayList cabs = brain.hidingObjects ();
		int s_index = (int)Math.Floor(cabs.Count * selection);
		if (s_index == cabs.Count) {
			s_index = cabs.Count-1;							// We have to have an index within the array		
		}

		// Go hide at s_index
		brain.seek (brain.closestNode (), (GameObject)cabs [s_index]);
	}

	public void grabBomb(){
		// TODO
	}

	public void placeBomb(double fiddle){
		// TODO
	}

	/* Sensor Functions */

	// Glance either left or right and check if there seems to be a path in that direction
	public float glanceDirection(bool right, float distance)
	{
		int adjust = 90;
		if (right)
			adjust = -90;

		Vector2 heading;
		heading.x = sightRadius * Mathf.Cos (Mathf.Deg2Rad * (transform.rotation.eulerAngles.z+adjust));
		heading.y = sightRadius * Mathf.Sin (Mathf.Deg2Rad * (transform.rotation.eulerAngles.z+adjust));
		RaycastHit2D hit = Physics2D.Raycast (transform.position, heading, sightRadius/2, 1 << LayerMask.NameToLayer ("Obstacle"));
		if(hit.distance == 0)
		{
			return 1.0f;
		}
		return 0.0f;
	}
	
	public int getDirection(){
		// Swapped 1 and 3 to be consistent with other functions
		if (this.transform.rotation.eulerAngles.z > 45 && this.transform.rotation.eulerAngles.z <= 135)
			return 0;	// North
		if (this.transform.rotation.eulerAngles.z > 135 && this.transform.rotation.eulerAngles.z <= 225)
			return 3;	// West
		if (this.transform.rotation.eulerAngles.z > 225 && this.transform.rotation.eulerAngles.z <= 315)
			return 2;	// South
		return 1;		// East
		
	}

	// Converts a relative direction that an AI is about to move into an absolute direction (N,W,S,E)
	// N: 0; E: 1; S:2; W:3;    F: 0; R: 1; B: 2; L: 3;
	public int relativeToAbsoluteDirection(int relative)
	{
		if((relative + dir == 0) || (relative + dir == 4))	// Yes, this works!
			return 0;	// Move North (up)
		else if((relative + dir == 1) || (relative + dir == 5))
			return 1;	// Move East (right)
		else if((relative + dir == 2) || (relative + dir == 6))
			return 2;	// Move South (down)
		else if((relative + dir == 3))
			return 3;	// Move West (left)
		
		return relative;
	}

	/* Get Sensor Input from the Brain */
	// 0 for no, 1 for yes
	public float seeBomb(){
		if (brain.getVisisbleBombs().Count > 0)
			return 1.0f;
		return 0;
	}

	public float seeCabinet(){
		if (brain.hidingObjects ().Count > 0)
			return 1.0f;
		return 0;
	}
	
	public float monsterDirection(){
		// TODO, 0 if unknown or behind, 1 if facing monster, .5 if left or right
		// player returns null
		return 0;
	}
	
	public float getHealth(){
		stats s = this.GetComponent<stats> ();
		return s.health / 100;
	}
	
	public float hasSprintA(){
		if (brain.sprintCount < 0)
			return 0.0f;
		else if (brain.sprintCount == 0)
			return 1.0f;
		
		else return 0.0f;
	}

	// Determines if the action indicated by the given action code is possible to do
	public bool actionPossible(int action){
		// Action 0: move forward; Action 1: move right; Action 2; move backward; Action 3 move left
		if(action < 4)
		{
			int abs_dir = this.relativeToAbsoluteDirection(action);	// 'move' functions use absolute directions

			// Can we move North?
			if(abs_dir == 0)
			{
				if(brain.closestNode().GetComponent<NodeInfo>().up != null)
					return true;
				else
					return false;
			}
			// Can we move East?
			if(abs_dir == 1)
			{
				if(brain.closestNode().GetComponent<NodeInfo>().right != null)
					return true;
				else
					return false;
			}
			// Can we move South?
			if(abs_dir == 2)
			{
				if(brain.closestNode().GetComponent<NodeInfo>().down != null)
					return true;
				else
					return false;
			}
			// Can we move East?
			if(abs_dir == 3)
			{
				if(brain.closestNode().GetComponent<NodeInfo>().left != null)
					return true;
				else
					return false;
			}
		}

		// Action 5: Sprint
		else if (action == 4)
		{
			if(brain.sprintCount > 0)
				return true;
			else
				return false;
		}

		// Action 6: Hide
		else if (action == 5)
		{
			// Do we see any hiding objects?
			if(brain.hidingObjects().Count > 0)
				return true;
			else
				return false;
		}

		// Action 8: Pick up Bomb
		else if (action == 7)
		{
			// Do we see any bombs?
			if(brain.getVisisbleBombs().Count > 0)
				return true;
			else
				return false;
		}

		// TODO
		return false;
	}

	public List<GameObject> getVisibleNodes ()
	{
		List<GameObject> visible = new List<GameObject> ();
		Collider2D[] nodes = Physics2D.OverlapCircleAll (sight.transform.position, sightRadius, 1 << LayerMask.NameToLayer ("Node"));
		
		for(int i=0; i<nodes.Length; i++)
		{
			bool hit = Physics2D.Linecast(transform.position, nodes[i].transform.position, 1 << LayerMask.NameToLayer("Obstacle"));
			if (!hit) visible.Add (nodes[i].gameObject);
		}
		
		return visible;
	}

	// Count the number 		of nodes and subtract those that are adjacent to 		nodes that have already been counted
	// This doesn't work perfectly, but it seems to get counts that are good enough...
	public float getPathCount(){
		List<GameObject> nodes = this.getPathNodes();
		List<GameObject> counted = new List<GameObject>();	// These are nodes that have been counted already
		int count = 0;
		left_count = 0;
		right_count = 0;
		for(int i = 0; i < nodes.Count; ++i){
			// Check if any of nodes[i] adjacent nodes are already in counted
			bool isCounted = false;
			if (nodes[i].GetComponent<NodeInfo>().up != null)
			{
				// Is this node in counted yet?
				GameObject temp = nodes[i].GetComponent<NodeInfo>().up;
				
				if(counted.Contains(temp))
					isCounted = true;
				else 
				{
					if(temp.GetComponent<NodeInfo>().left != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().left;
						if(counted.Contains(temp2))
						   isCounted = true;
					}
					if(temp.GetComponent<NodeInfo>().right != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().right;
						if(counted.Contains(temp2))
							isCounted = true;
					}

				}
			}
			if (nodes[i].GetComponent<NodeInfo>().right != null)
			{
				// Is this node in counted yet?
				GameObject temp = nodes[i].GetComponent<NodeInfo>().right;
				
				if(counted.Contains(temp))
					isCounted = true;
				else 
				{
					if(temp.GetComponent<NodeInfo>().up != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().up;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					if(temp.GetComponent<NodeInfo>().down != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().down;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					
				}
			}			
			if (nodes[i].GetComponent<NodeInfo>().down != null)
			{
				// Is this node in counted yet?
				GameObject temp = nodes[i].GetComponent<NodeInfo>().down;
				
				if(counted.Contains(temp))
					isCounted = true;
				else 
				{
					if(temp.GetComponent<NodeInfo>().left != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().left;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					if(temp.GetComponent<NodeInfo>().right != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().right;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					
				}
			}
			if (nodes[i].GetComponent<NodeInfo>().left != null)
			{
				// Is this node in counted yet?
				GameObject temp = nodes[i].GetComponent<NodeInfo>().left;
				
				if(counted.Contains(temp))
					isCounted = true;
				else 
				{
					if(temp.GetComponent<NodeInfo>().up != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().up;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					if(temp.GetComponent<NodeInfo>().down != null)
					{
						// We should check the diaganol too
						GameObject temp2 = temp.GetComponent<NodeInfo>().down;
						if(counted.Contains(temp2))
							isCounted = true;
					}
					
				}
			}	
			
			// If isCounted is still false, then we need to count this node
			if(!isCounted)
			{
				Debug.DrawLine(transform.position, nodes[i].transform.position, Color.white, 10.0f);
				count++;

				// Should this node be counted as "left" or "right"?
				if(this.dir == 0)
				{
					// Facing North!
					if(nodes[i].transform.position.x < transform.position.x)
						left_count++;
					else
						right_count++;
				}
				else if(this.dir == 1)
				{
					// Facing East!
					if(nodes[i].transform.position.y > transform.position.y)
						left_count++;
					else
						right_count++;
				}
				else if(this.dir == 2)
				{
					// Facing South!
					if(nodes[i].transform.position.x > transform.position.x)
						left_count++;
					else
						right_count++;
				}
				else if(this.dir == 3)
				{
					// Facing West!
					if(nodes[i].transform.position.y < transform.position.y)
						left_count++;
					else
						right_count++;
				}
			}
			// We still need to place this node in counted so nodes adjacent to it aren't counted either
			counted.Add(nodes[i]);				
		}

		float count_norm = count / 24.0f;
		if(count_norm < 1)
			return count_norm;
		else
			return 1.0f;
	}



	/* Private Functions */

	// Get all the nodes that could be the start of a new path (the player can't see a wall behind the node)
	private List<GameObject> getPathNodes()
	{
		// First, get all visible nodes
		List<GameObject> visible = this.getVisibleNodes ();
		List<GameObject> possible = new List<GameObject>();	// This will hold nodes that MIGHT be new paths
		
		// For each visible node, check if it will start a path
//		Debug.Log ("&&&" + visible.Count);
		for (int i = 0; i < visible.Count; ++i) 
		{
			// We are looking for nodes adjacent to visible[i] that are NOT in visible
			GameObject[] adjacent = new GameObject[4];
			if (((GameObject) visible[i]).GetComponent<NodeInfo>().up != null)
				adjacent[0] = ((GameObject) visible[i]).GetComponent<NodeInfo>().up;
			if (((GameObject) visible[i]).GetComponent<NodeInfo>().right != null)
				adjacent[1] = ((GameObject) visible[i]).GetComponent<NodeInfo>().right;
			if (((GameObject) visible[i]).GetComponent<NodeInfo>().down != null)
				adjacent[2] = ((GameObject) visible[i]).GetComponent<NodeInfo>().down;
			if (((GameObject) visible[i]).GetComponent<NodeInfo>().left != null)
				adjacent[3] = ((GameObject) visible[i]).GetComponent<NodeInfo>().left;
			
			// Get the AI Unit's heading
			this.dir = this.getDirection();
			
			// Are any of the adjacent nodes visible? If any are not, we might have a path
			bool[] isPath = new bool[4];						// Indicates that visible[i] is a new path node
			if (adjacent[0] != null && !visible.Contains(adjacent[0]))
			{
				isPath[0] = this.isPathNode((GameObject)visible[i], 0.0f, .38f, dir);
			}
			else if (adjacent[1] != null && !visible.Contains(adjacent[1]))
			{
				isPath[1] = this.isPathNode((GameObject)visible[i], .38f, 0.0f, dir);
			}	
			else if (adjacent[2] != null && !visible.Contains(adjacent[2]))
			{
				isPath[2] = this.isPathNode((GameObject)visible[i], 0.0f, -.38f, dir);
			}
			else if (adjacent[3] != null && !visible.Contains(adjacent[3]))
			{
				isPath[3] = this.isPathNode((GameObject)visible[i], -.38f, 0.0f, dir);
			}	
			
			// If any of the four adjacent nodes might be a path, we have a path!
			if(isPath[0] || isPath[1] || isPath[2] || isPath[3])
				possible.Add(visible[i]);
		}
		return possible;
	}
	
	private bool isPathNode(GameObject node, float adjustx, float adjusty, int direction)
	{
		// Is there a wall above this node, and can we see it?
		Vector3 los = node.transform.position;
		los.x += adjustx;
		los.y += adjusty;
		
		// Determine the distance between los and this.transform.position
		Vector3 dist = los - this.transform.position;

		// Make sure that the Node is in the desired direction
		if (direction == 1 && dist.x <= 0)
			return false;
		if (direction == 0 && dist.y <= 0)
			return false;
		if (direction == 3 && dist.x >= 0)
			return false;
		if (direction == 2 && dist.y >= 0)
			return false;
		
		// See if the LOS is obscured
		RaycastHit2D hit = Physics2D.Linecast(transform.position, los, 1 << LayerMask.NameToLayer("Obstacle"));
		
		// Draw the line red if it hits a wall before it makes it to that point
		if (hit.distance != 0 && hit.distance < .95*dist.magnitude)
		{
//			Debug.DrawLine(this.transform.position, los, Color.magenta, 10.0f);
			return true;
		}
		else if (hit.distance == 0)
		{
//			Debug.DrawLine(this.transform.position, los, Color.green, 10.0f);
			return true;
		}
		else
		{
			Debug.DrawLine(this.transform.position, los, Color.red, 10.0f);
			return false;
		}
	}

	
	// Each of the move functions will use this...
	private void seekAbsoluteDirection(int absolute_dir)
	{
		// Get the nodeInfo for the closest node
		NodeInfo closest = brain.closestNode().GetComponent<NodeInfo>();
		if (absolute_dir == 0) 
			brain.seek (brain.closestNode(), closest.up);
		else if (absolute_dir == 1) 
			brain.seek(brain.closestNode(), closest.right);
		else if (absolute_dir == 2) 
			brain.seek (brain.closestNode(), closest.down);
		else if (absolute_dir == 3) 
			brain.seek (brain.closestNode(), closest.left);
	}
}
