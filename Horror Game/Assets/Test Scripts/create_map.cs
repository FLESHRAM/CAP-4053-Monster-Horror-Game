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

	private int wallResources = 100;
	private float leftBound = -16;
	private float rightBound = 16;
	private float upperBound = 8;
	private float lowerBound = -8;
	private float increment = .64f;


	// Use this for initialization
	void Start () {

		//Create the outside square
		createWalls();

		createFloor();

		spawnObstacles();

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

	void createWalls()
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

		for (float y = 7.36f; y > -7.64f; y -= 0.64f)
			for (float x = -15.36f; x < 15.64f; x += 0.64f)
				Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 0.0f), Quaternion.identity);
	}

	void spawnObstacles()
	{
		for (float y = 7.36f; y > -7.64f; y -= 0.64f)
			for (float x = -15.36f; x < 15.64f; x += 0.64f)
				if(Random.Range(0,1) == 1 && wallResources > 0)
				{
					Instantiate (wall_vertical, new Vector3 (x, y, 1.0f), Quaternion.identity);
					wallResources--;
				}

	}

	// Update is called once per frame
	void Update () {
	
	}
}
