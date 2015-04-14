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
	

	void Update () {
	
	}
}
