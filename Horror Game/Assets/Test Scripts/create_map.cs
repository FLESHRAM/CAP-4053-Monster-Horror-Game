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

	// Use this for initialization
	void Start () {

		floor_patterns = new Transform[7] {floor_2,floor_3,floor_5,floor_6,floor_7,floor_8,floor_9};

		//Create the outside square
		Instantiate (top_left_corner, new Vector3 (-9.64f, 5.64f, 0.0f), Quaternion.identity);

		for (float x = -9f; x < 10f; x += 0.64f) {
			Instantiate (wall_horizontal, new Vector3 (x, 5.64f, 0.0f), Quaternion.identity);
		}

		Instantiate (top_right_corner, new Vector3 (-9.64f, 5.64f, 0.0f), Quaternion.identity);

		for (float y = 5.64f; y > -10f; y -= 0.64f) {
			Instantiate (wall_vertical, new Vector3 (-9.64f, y, 0.0f), Quaternion.identity);
		}

		Instantiate (bottom_left_corner, new Vector3 (-9.64f, -10.36f, 0.0f), Quaternion.identity);

		for (float x = -9f; x < 10f; x += 0.64f) {
			Instantiate (wall_horizontal, new Vector3 (x, -10.36f, 0.0f), Quaternion.identity);
		}

		Instantiate (bottom_right_corner, new Vector3 (10.2f, -10.36f, 0.0f), Quaternion.identity);

		for (float y = -10.36f; y < 5.0f; y += 0.64f) {
			Instantiate (wall_vertical, new Vector3 (10.2f, y, 0.0f), Quaternion.identity);
		}

		Instantiate (top_right_corner, new Vector3 (10.2f, 5.64f, 0.0f), Quaternion.identity);

		//Create the floor patter
		for (float y = 5.0f; y > -10f; y -= 0.64f) {
			for (float x = -9f; x < 10f; x += 0.64f) {

				int rand = Random.Range(0, 10);

				if ((x != -9f && y != 5.0f) && rand == 4) {
					Instantiate (wall_vertical, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else if ((x != -9f && y != 5.0f) && rand == 8) {
					Instantiate (wall_horizontal, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else {
					Instantiate (floor_patterns [Random.Range (0, 7)], new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
