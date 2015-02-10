using UnityEngine;
using System.Collections;

public class BallAnimator : MonoBehaviour {


	public Sprite[] sprites;
	public float FPS;
	private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		rend = renderer as SpriteRenderer;
	}
	
	// Update is called once per frame
	void Update () {
		int index = (int)(Time.timeSinceLevelLoad * FPS);
		index = index % sprites.Length;
		rend.sprite = sprites[ index ];
	}
}
