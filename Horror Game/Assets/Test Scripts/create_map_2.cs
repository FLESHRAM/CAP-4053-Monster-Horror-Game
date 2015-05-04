using UnityEngine;
using System.Collections;

public class create_map_2 : MonoBehaviour {

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

	private float[] point = new float[2];
	
	private int wallResources = 2000;
	private float leftBound = -16;
	private float rightBound = 16;
	private float upperBound = 8;
	private float lowerBound = -8;
	private float increment = .64f;

	void Start () {
	
		//Create the outside square
		createBorder();
		createFloor();
		//drawWall (-9.59f, 4.16f, 1, 5);
		generateMaze (2, 8, 100);
	}

	void createBorder()
	{
		Instantiate (top_left_corner, new Vector3 (leftBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (top_right_corner, new Vector3 (rightBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_left_corner, new Vector3 (leftBound, lowerBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_right_corner, new Vector3 (rightBound, lowerBound, 0.0f), Quaternion.identity);
		
		//Creating the border
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

	void generateMaze(int minLength, int maxLength, int numWall)
	{
		for (int i = 0; i < numWall; i++) 
		{		
			attemptRandomWall (minLength, maxLength);
		}
	}

	void attemptRandomWall(int minLength, int maxLength)
	{
		selectWallStart (1);
		int dir = Random.Range (0, 4);
		int len = Random.Range (minLength, maxLength);
		drawWall (point [0], point [1], dir, len);
	}

	void drawWall(float sx, float sy, int dir, int len)
	{
		float stepx = 0.0f;
		float stepy = 0.0f;

		if (dir == 0) {stepx = 0.0f; stepy = 0.64f;} //Up
		if (dir == 1) {stepx = -0.64f; stepy = 0.0f;} //Left
		if (dir == 2) {stepx = 0.64f; stepy = 0.0f;} //Right
		if (dir == 3) {stepx = 0.0f; stepy = -0.64f;} //Down

		float currx = sx;
		float curry = sy;

		for (float i = 0.64f; i < len; i += 0.64f) 
		{
			Debug.Log ("currx: " + currx + "   curry: " + curry + " " + checkSpot(currx, curry));

			if (checkSpot(currx, curry)){ break; }

			if (dir == 0 || dir == 3)
			{
				Instantiate (wall_vertical, new Vector3 (currx, curry, 0.0f), Quaternion.identity);
			}
			else if (dir == 1 || dir == 2)
			{
				Instantiate (wall_horizontal, new Vector3 (currx, curry, 0.0f), Quaternion.identity);
			}

			currx += stepx;
			curry += stepy;
		}
	}

	bool checkSpot(float x, float y)
	{
		if(Physics2D.OverlapCircle(new Vector2(x, y), 0.1f, 1 << LayerMask.NameToLayer("Obstacle")))
			return true;
		
		return false;			
	}

	void selectWallStart(int granularity)
	{
		float tempSX;
		float tempSY;
		float xTarget = 32 / granularity;
		float yTarget = 16 / granularity;

		Point[] points = new Point[] 
		{
			new Point(-13.44f, 4.16f),
			new Point(-13.44f, -0.3199f),
			new Point(-13.44f, -4.7999f),
			new Point(-11.52f, -3.5199f),
			new Point(-9.5999f, 5.44f),
			new Point(-9.5999f, 2.24f),
			new Point(-9.5999f, -6.6f),
			new Point(-8.3199f, -2.24f),
			new Point(-6.399f, 0.96f),
			new Point(-4.4799f, -2.24f),
			new Point(-3.1999f, 3.52f),
			new Point(-1.9199f, 2.24f),
			new Point(0.64f, -4.7999f),
			new Point(2.56f, -3.5199f),
			new Point(4.48f, 3.52f),
			new Point(4.48f, -0.3199f),
			new Point(8.32f, -2.8799f),
			new Point(9.60f, 3.52f)
		};

		int loc = Random.Range (0, points.Length);

		Point p = points[loc];

		point [0] = p.x;
		point [1] = p.y;
	}

	struct Point
	{
		public float x, y;
		
		public Point(float px, float py)
		{
			x = px;
			y = py;
		}
	}
	

	void Update () {
	
	}
}
