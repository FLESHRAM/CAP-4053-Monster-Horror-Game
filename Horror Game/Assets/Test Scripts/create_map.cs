using UnityEngine;
using System.Collections.Generic;

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

	public Transform small_vertical_wall;
	public Transform wall_vertical;
	public Transform wall_horizontal;

	public Transform top_left_corner;
	public Transform top_right_corner;
	public Transform bottom_right_corner;
	public Transform bottom_left_corner;

	private Transform[] floor_patterns;

	private int wallResources = 2000;
	private float leftBound = -16;
	private float rightBound = 16;
	private float upperBound = 8;
	private float lowerBound = -8;
	private float increment = .64f;

	private List<GameObject> wallSections = new List<GameObject>();


	// Use this for initialization
	void Start () {

		//Create the outside square
		createBorder();

		createFloor();

		spawnWalls();	
	}

	void createBorder()
	{
		Instantiate (top_left_corner, new Vector3 (leftBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (top_right_corner, new Vector3 (rightBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_left_corner, new Vector3 (leftBound, lowerBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_right_corner, new Vector3 (rightBound, lowerBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_right_corner, new Vector3 (leftBound + 4, upperBound - 4, -1f), Quaternion.identity);

		//Creating the border
		for (float x = leftBound + increment; x < 15.64f; x += increment) {
			Instantiate (wall_horizontal, new Vector3 (x, upperBound, 0.0f), Quaternion.identity);
			Instantiate (wall_horizontal, new Vector3 (x, lowerBound, 0.0f), Quaternion.identity);
		}

		for (float y = upperBound; y > -7.64f; y -= increment) {
			Instantiate (wall_vertical, new Vector3 (leftBound, y, 0.0f), Quaternion.identity);
			Instantiate (wall_vertical, new Vector3 (rightBound, y, 0.0f), Quaternion.identity);
		}

		//Creating the upper left spawn area
		for (float x = leftBound + (increment * 3); x < leftBound + 4; x += increment)
			Instantiate (wall_horizontal, new Vector3 (x, upperBound - 4, -1f), Quaternion.identity);

		for (float y = upperBound - increment; y > upperBound - 4; y -= increment)
			Instantiate (wall_vertical, new Vector3 (leftBound + 4, y, -1f), Quaternion.identity);



	}

	void createFloor()
	{
		floor_patterns = new Transform[6] {floor_2,floor_3,floor_5,floor_6,floor_7,floor_8};

		for (float y = upperBound - increment; y > -7.64f; y -= increment)
			for (float x = leftBound + increment; x < 15.64f; x += increment)
				Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 1.0f), Quaternion.identity);
	}

	void spawnWalls()
	{
		for (float y = upperBound; y > -7.64f; y -= increment)
			for (float x = leftBound; x < 15.64f; x += increment)
				if(Random.Range(0,15) == 1 && wallResources > 0)
				{
					if(x <= leftBound + 5 && y >= upperBound - 5)
						continue;
				
					int distance = Random.Range(5,20);

					if(checkSpot(x, y))
						continue;

					if(Random.Range(0, 2) == 1)
						generateWall(x, y, 'v', 1, distance);

					else
						generateWall(x, y, 'h', 1, distance);
				}
	}
	
	void generateWall(float x, float y, char wallDirection, int movementDirection, int distance)//, GameObject[] wallList)
	{
		if (distance < 1) 
		{
			return;
		}
		
		for(int i = distance; i > 0; i--)
		{
			if(!checkSpot2(x, y, wallDirection))
				return;
				
			if(wallDirection == 'v')
			{
				//wallSection = (GameObject)Instantiate(wall_vertical, new Vector3(x, y, -1f), Quaternion.identity);
				//Instantiate(Resources.Load ("wall_vertical"), new Vector3(x, y, -1f), Quaternion.identity);
				//wallSections.Add((GameObject)Instantiate(wall_vertical, new Vector3(x, y, -1f), Quaternion.identity));
				Instantiate(wall_vertical, new Vector3(x, y, -1f), Quaternion.identity);
				y -= increment;

				//Destroy (clone);
			}
			else
			{
				Instantiate(wall_horizontal, new Vector3(x, y, -1f), Quaternion.identity);
				x += increment;
			}
						
			distance--;

			if(!checkSpot2(x, y, wallDirection))
				return;
			
			if(Random.Range (0, 2) == 1)
			{
				if(wallDirection == 'v')
				{
					Instantiate(bottom_left_corner, new Vector3(x, y, -1f), Quaternion.identity);
					generateWall(x + increment, y, 'h', movementDirection, distance);
				}
				else
				{
					Instantiate(top_right_corner, new Vector3(x, y, -1f), Quaternion.identity);
					generateWall(x, y + increment, 'v', movementDirection, distance);
				}
			}	
		}
	}

	void addEndPiece(float x, float y, char wallDirection)
	{
		if (wallDirection == 'v')
			Instantiate (wall_vertical, new Vector3 (x, y, -1f), Quaternion.identity);
		else
			Instantiate (wall_horizontal, new Vector3 (x, y, -1f), Quaternion.identity);
	}

	
	bool checkSpot(float x, float y)
	{
		if(Physics2D.OverlapCircle(new Vector2(x, y), increment * 2))
			return true;
		
		return false;			
	}

	bool checkSpot2(float x, float y, char wallDirection)
	{
		float radius = increment;

		print (x + " " + (y - radius));
	
		if(Physics2D.OverlapCircle(new Vector2(x, y), .1f))
			return false;
		
		if(wallDirection == 'v')
		{
			if(Physics2D.OverlapCircle(new Vector2(x, y - radius), .1f))
				return false;

			if(Physics.OverlapSphere(new Vector3(x, y - radius, 1f), .1f).Length > 0)
			   return false;
			
			if(Physics2D.OverlapCircle(new Vector2(x - radius, y), .1f) || Physics2D.OverlapCircle(new Vector2(x + radius, y), .1f))
				return false;	
		}
		
		else
		{
			if(Physics2D.OverlapCircle(new Vector2(x + radius, y), .1f))
				return false;

			if(Physics2D.OverlapCircle(new Vector2(x, y - radius), .1f) || Physics2D.OverlapCircle(new Vector2(x, y + radius), .1f))
				return false;
		}
		
		if(Physics2D.OverlapCircle(new Vector2(x - radius, y - radius), .1f) || Physics2D.OverlapCircle(new Vector2(x + radius, y - radius), .1f))
			return false;
		
		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
