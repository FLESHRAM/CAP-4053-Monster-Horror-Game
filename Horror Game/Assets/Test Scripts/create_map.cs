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

		floor_patterns = new Transform[6] {floor_2,floor_3,floor_5,floor_6,floor_7,floor_8};

		//Create the outside square
		Instantiate (top_left_corner, new Vector3 (-16.0f, 8.0f, 0.0f), Quaternion.identity);

		for (float x = -15.36f; x < 15.64f; x += 0.64f) {
			Instantiate (wall_horizontal, new Vector3 (x, 8.0f, 0.0f), Quaternion.identity);
		}

		Instantiate (top_right_corner, new Vector3 (16.0f, 8.0f, 0.0f), Quaternion.identity);

		for (float y = 8.0f; y > -7.64f; y -= 0.64f) {
			Instantiate (wall_vertical, new Vector3 (-16.0f, y, 0.0f), Quaternion.identity);
		}

		Instantiate (bottom_left_corner, new Vector3 (-16.0f, -8.0f, 0.0f), Quaternion.identity);

		for (float x = -15.36f; x < 15.64f; x += 0.64f) {
			Instantiate (wall_horizontal, new Vector3 (x, -8.0f, 0.0f), Quaternion.identity);
		}

		Instantiate (bottom_right_corner, new Vector3 (16.0f, -8.0f, 0.0f), Quaternion.identity);

		for (float y = -7.36f; y < 7.64f; y += 0.64f) {
			Instantiate (wall_vertical, new Vector3 (16.0f, y, 0.0f), Quaternion.identity);
		}


		//Create the floor patter
		for (float y = 7.36f; y > -7.64f; y -= 0.64f) {
			for (float x = -15.36f; x < 15.64f; x += 0.64f) {

				int rand = Random.Range(0, 10);

				if ((x != -15.36f || y != 7.36f) && rand == 4) {
					Instantiate (wall_vertical, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else if ((x != -15.36f || y != 7.36f) && rand == 8) {
					Instantiate (wall_horizontal, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
				else {
					Instantiate (floor_patterns [Random.Range (0, 6)], new Vector3 (x, y, 0.0f), Quaternion.identity);
				}
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
