using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Animator anim;
	public float moveSpeed = 5f;
	public float turnSpeed = 50f;
	
	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;
	private Animator hold;

	private bool attackDone = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent ("Animator") as Animator;
	}
	

		
		void Update() 
	{        


		if(Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
		{
			
			if(Input.GetKey(KeyCode.W)) transform.position = Vector2.MoveTowards(transform.position, up.transform.position, moveSpeed*Time.deltaTime);

			if(Input.GetKey(KeyCode.A)) transform.position = Vector2.MoveTowards(transform.position, left.transform.position, moveSpeed*Time.deltaTime);
			
			if(Input.GetKey(KeyCode.S)) transform.position = Vector2.MoveTowards(transform.position, down.transform.position, moveSpeed*Time.deltaTime);

			if(Input.GetKey(KeyCode.D)) transform.position = Vector2.MoveTowards(transform.position, right.transform.position, moveSpeed*Time.deltaTime);
		

			anim.SetBool("IsMoving", true);
		}


		Vector3 currPos = transform.position;

		if(!Input.GetButton("Fire1") && attackDone) anim.SetBool("IsAttacking", false);
		if(Input.GetButton("Fire1")) 
		{
			attackDone = false;
			anim.SetBool("IsAttacking", true);
		}


		Vector3 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		direction = direction - currPos;
		direction.z = 0;
		direction.Normalize ();

		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, angle), turnSpeed * Time.deltaTime);




		
		if(!Input.GetButton("Vertical") && !Input.GetButton("Horizontal")) anim.SetBool("IsMoving", false);
	}


	public void attackFinished()
	{
		attackDone = true;

	}


	public void transformation(Animator Monster)
	{
		Animator player = gameObject.GetComponent<Animator> ();

		player.runtimeAnimatorController = Monster.runtimeAnimatorController;
	}
}



