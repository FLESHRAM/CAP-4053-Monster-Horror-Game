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


	// Use this for initialization
	void Start () {

		//Create the outside square
		createBorder();

		createFloor();

		spawnWalls();

				/*

				if(x < -11 && y > 3)
					rand = 0;

				if ((x != -15.36f && y != 7.36f) && rand == 4) {
					Instantiate (wall_vertical, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else if ((x != -15.36f && y != 7.36f) && rand == 8) {
					Instantiate (wall_horizontal, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else {
					Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				*/
	
	}

	void createBorder()
	{
		Instantiate (top_left_corner, new Vector3 (leftBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (top_right_corner, new Vector3 (16.0f, 8.0f, 0.0f), Quaternion.identity);
		Instantiate (bottom_left_corner, new Vector3 (-16.0f, -8.0f, 0.0f), Quaternion.identity);
		Instantiate (bottom_right_corner, new Vector3 (16.0f, -8.0f, 0.0f), Quaternion.identity);

		for (float x = leftBound + increment; x < 15.64f; x += increment) {
			Instantiate (wall_horizontal, new Vector3 (x, upperBound, 0.0f), Quaternion.identity);
			Instantiate (wall_horizontal, new Vector3 (x, lowerBound, 0.0f), Quaternion.identity);
		}

		for (float y = upperBound; y > -7.64f; y -= increment) {
			Instantiate (wall_vertical, new Vector3 (leftBound, y, 0.0f), Quaternion.identity);
			Instantiate (wall_vertical, new Vector3 (rightBound, y, 0.0f), Quaternion.identity);
		}

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
				if(Random.Range(0,25) == 1 && wallResources > 0)
				{
					if(x <= leftBound + 4 && y >= upperBound - 4)
						continue;
				
					int distance = Random.Range(10,20);

					if(Random.Range(0, 2) == 1)
						generateWall(x, y, 'v', 1, distance);
					else
						generateWall(x, y, 'h', 1, distance);
				}

	}
	
	void generateWall(float x, float y, char wallDirection, int movementDirection, int distance)
	{
		if(distance == 0)
			return;
		
		for(int i = distance; i > 0; i--)
		{
			if(Physics2D.OverlapCircle(new Vector2(x, y), .01f))
				return;
				
			if(wallDirection == 'v')
			{
				if(Physics2D.OverlapCircle(new Vector2(x, y - increment), .1f))
					return;
			
				Instantiate(wall_vertical, new Vector3(x, y, -1f), Quaternion.identity);
				y -= increment;
			}
			else
			{
				if(Physics2D.OverlapCircle(new Vector2(x + increment, y), .1f))
					return;
			
				Instantiate(wall_horizontal, new Vector3(x, y, -1f), Quaternion.identity);
				x += increment;
			}
			
			distance--;
			
			if(Random.Range (0, 3) == 1)
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
	
	bool checkSpot(float x, float y, char wallDirection)
	{
		if(Physics2D.OverlapCircle(new Vector2(x, y), .1f))
			return false;
	
		if(wallDirection == 'v')
		{
			if(Physics2D.OverlapCircle(new Vector2(x, y - increment), .1f))
				return false;
			
			if(Physics2D.OverlapCircle(new Vector2(x - increment, y), .1f) || Physics2D.OverlapCircle(new Vector2(x + increment, y), .1f))
				return false;			
		}
		
		else
		{
			if(Physics2D.OverlapCircle(new Vector2(x + increment, y), .1f))
				return false;
			
			if(Physics2D.OverlapCircle(new Vector2(x, y - increment), .1f) || Physics2D.OverlapCircle(new Vector2(x, y + increment), .1f))
				return false;
		}
		
		if(Physics2D.OverlapCircle(new Vector2(x - increment, y - increment), .1f) || Physics2D.OverlapCircle(new Vector2(x + increment, y - increment), .1f))
			return false;
		
		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
