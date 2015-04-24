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
	
	int mazeSize = 0; 
	int xsize = 50;
	int ysize = 25;
	char [,,] maze;
	
	// Use this for initialization
	void Start () {
		
		//For random mapsize generation
		//xsize = Random.Range(40, 101);
		//ysize = xsize / 2;
		
		createMaze();
		
		//createRandomMap();	
	}
	
	void createMaze()
	{
		floor_patterns = new Transform[6] {floor_2,floor_3,floor_5,floor_6,floor_7,floor_8};
		
		
		
		while(mazeSize < 30)	
		{
			mazeSize = 0;
			maze = new char[ysize, xsize, 2];
			for(int i = 0; i < ysize; i++)
				for(int j = 0; j < xsize; j++)
			{
				maze[i,j, 0] = '_';
				maze[i,j, 1] = 'u';
			}	
			
			List<Coordinate> coordStack = new List<Coordinate>();
			coordStack.Add(new Coordinate(0,0));
			coordStack.Add(new Coordinate(0, 1));
			coordStack.Add (new Coordinate(1, 0));
			fillInMaze(coordStack);
		}

		for(int i = 0; i < ysize; i++)
		{				
			for(int j = 0; j < xsize; j++)
			{
				float x = j * increment - ( increment * xsize / 2);
				float y = (increment * ysize / 2) - (i * increment);
			
				if(maze[i,j, 0] == 'f')
					Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 1.0f), Quaternion.identity);
				else
				//else if(maze[i,j,0] == 'w') //Leaves black spaces where no walls or floors are
					Instantiate (wall_vertical, new Vector3 (x, y, 0.0f), Quaternion.identity);
			}
		}
				
		createBorder();
	}
	
	void fillInMaze(List<Coordinate> coordStack)
	{
		if(coordStack.Count < 1)
			return;
		
		int x = coordStack[0].x;
		int y = coordStack[0].y;
		coordStack.RemoveAt(0);
		
		maze[y, x, 0] = 'f';
		maze[y, x, 1] = 'v';
		
		int chance = 2, chanceIncrement = 4;
		bool added = false;
		
		if((y - 1) > 0)
			if(maze[y-1, x, 1] != 'v')
				if(Random.Range(0, chance) == 0)
				{
					chance *= chanceIncrement;
					coordStack.Insert(0, new Coordinate(x, y-1));
					added = true;
					mazeSize++;
				}
				else
				{
					maze[y-1, x, 0] = 'w';
					maze[y-1, x, 1] = 'v';
					chance /= chanceIncrement;
				}
				
		if((x + 1) < xsize)
			if(maze[y, x+1, 1] != 'v')
				if(Random.Range(0, chance) == 0)
			{
				chance *= chanceIncrement;
				coordStack.Insert(0, new Coordinate(x+1, y));
				added = true;					
				mazeSize++;
			}
			else
			{
				maze[y, x+1, 0] = 'w';
				maze[y, x+1, 1] = 'v';
				chance /= chanceIncrement;
				
			}			
		
		if((y + 1) < ysize)
		   if(maze[y+1, x, 1] != 'v')
			   if(Random.Range(0, chance) == 0)
			   {
					chance *= chanceIncrement;
					coordStack.Insert(0, new Coordinate(x, y+1));
					added = true;
					mazeSize++;
				}
				else
				{
					maze[y+1, x, 0] = 'w';
					maze[y+1, x, 1] = 'v';
					chance /= chanceIncrement;
				}				
					
		
		if((x -1) > 0)
			if(maze[y, x-1, 1] != 'v')
				if(Random.Range(0, chance) == 0)
				{
					chance *= chanceIncrement;
					coordStack.Insert(0, new Coordinate(x-1, y));
					added = true;					
					mazeSize++;
				}
				else
				{
					maze[y, x-1, 0] = 'w';
					maze[y, x-1, 1] = 'v';
					chance /= chanceIncrement;
					
				}
				
		
		fillInMaze(coordStack);
	}
	
	//Probably a bad implementation, froze unity last time I tried
	void fillInMaze2(List<Coordinate> coordStack)
	{
		if(coordStack.Count < 1)
			return;
	
		int x = coordStack[0].x;
		int y = coordStack[0].y;
		coordStack.RemoveAt(0);
	
		maze[y, x, 0] = 'f';
		maze[y, x, 1] = 'v';
				
		int chance = 2, chanceIncrement = 4;
		//Added and counter provide a way to break out of the while loop
		int added = 0, newX, newY, counter = 0;
		
		
		
		while(added < 2 && counter < 4)
		{
			int direction = Random.Range(0, 4);
			newY = y;
			newX = x;			
			//Up direction
			if(direction == 0)
				newY = y - 1;
			//Down direction
			else if(direction == 1)
				newY = y + 1;
			//Left direction
			else if(direction == 2)
				newX = x - 1;
			//Right direction
			else
				newX = x + 1;
				
			if(newX < 0 || newX >= xsize || newY < 0 || newY > ysize)
				continue;
			
			print (newX + " " + newY);
			
			if(maze[newY, newX, 1] == 'v')
				continue;
				
			if(Random.Range(0, chance) == 0)
			{
				chance *= chanceIncrement;
				coordStack.Insert(0, new Coordinate(newX, newY));
				added++;
			}
			else
			{
				maze[newY, newX, 0] = 'w';
				maze[newY, newX, 1] = 'v';
				chance /= chanceIncrement;
			}
			
			counter++;
		}
		
		fillInMaze2(coordStack);
	}
	
	void createRandomMap()
	{
		createBorder();
		
		createFloor();
		
		createEmptySpaces();
		
		spawnWalls();	
	}

	void createBorder()
	{
		upperBound = (ysize * increment / 2) + increment;
		rightBound = (xsize * increment / 2);
		lowerBound = (upperBound * -1) + increment;
		leftBound = (rightBound * -1) - increment;
	
		Instantiate (top_left_corner, new Vector3 (leftBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (top_right_corner, new Vector3 (rightBound, upperBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_left_corner, new Vector3 (leftBound, lowerBound, 0.0f), Quaternion.identity);
		Instantiate (bottom_right_corner, new Vector3 (rightBound, lowerBound, 0.0f), Quaternion.identity);
		//Instantiate (bottom_right_corner, new Vector3 (leftBound + 4, upperBound - 4, -1f), Quaternion.identity);

		//Creating the border
		for (float x = leftBound + increment; x < rightBound; x += increment) {
			Instantiate (wall_horizontal, new Vector3 (x, upperBound, 0.0f), Quaternion.identity);
			Instantiate (wall_horizontal, new Vector3 (x, lowerBound, 0.0f), Quaternion.identity);
		}

		for (float y = upperBound; y > lowerBound; y -= increment) {
			Instantiate (wall_vertical, new Vector3 (leftBound, y, 0.0f), Quaternion.identity);
			Instantiate (wall_vertical, new Vector3 (rightBound, y, 0.0f), Quaternion.identity);
		}

		//Creating the upper left spawn area
	//	for (float x = leftBound + (increment * 3); x < leftBound + 4; x += increment)
		//	Instantiate (wall_horizontal, new Vector3 (x, upperBound - 4, -1f), Quaternion.identity);

		//for (float y = upperBound - increment; y > upperBound - 4; y -= increment)
			//Instantiate (wall_vertical, new Vector3 (leftBound + 4, y, -1f), Quaternion.identity);



	}

	void createFloor()
	{
		floor_patterns = new Transform[6] {floor_2,floor_3,floor_5,floor_6,floor_7,floor_8};

		for (float y = upperBound - increment; y > -7.64f; y -= increment)
			for (float x = leftBound + increment; x < 15.64f; x += increment)
				Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 1.0f), Quaternion.identity);
	}
	
	void createEmptySpaces()
	{
		for (float y = upperBound - 5; y > -7.64f + 3; y -= increment)
			for (float x = leftBound + 5; x < 15.64f - 5; x += increment)
				if(Random.Range(0, 250) == 1)
				{
					float wallRadius = (Random.Range(2, 4) * increment);
					
					for(float tempY = y + wallRadius; tempY > y - wallRadius; tempY -= increment)
					{
						Instantiate(wall_vertical, new Vector3(x - wallRadius, tempY, -1f), Quaternion.identity);
						Instantiate(wall_vertical, new Vector3(x + wallRadius, tempY, -1f), Quaternion.identity);
					}
					
					for(float tempX = x - wallRadius; tempX < x + wallRadius; tempX += increment)
					{
						Instantiate(wall_horizontal, new Vector3(tempX, y + wallRadius, -1f), Quaternion.identity);
						Instantiate(wall_horizontal, new Vector3(tempX, y - wallRadius, -1f), Quaternion.identity);
					
					}
					
				Instantiate (top_left_corner, new Vector3 (x - wallRadius, y + wallRadius, -1.1f), Quaternion.identity);
				Instantiate (top_right_corner, new Vector3 (x + wallRadius, y + wallRadius, -1.1f), Quaternion.identity);
				Instantiate (bottom_left_corner, new Vector3 (x - wallRadius, y - wallRadius, -1.1f), Quaternion.identity);
				Instantiate (bottom_right_corner, new Vector3 (x + wallRadius, y - wallRadius, -1.1f), Quaternion.identity);
						
					
				}
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

		//print (x + " " + (y - radius));
	
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
	
	public class Coordinate
	{
		public int x, y;
		
		public Coordinate(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
