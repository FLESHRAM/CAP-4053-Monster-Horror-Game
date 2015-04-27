using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Animator anim;
	public float moveSpeed = 5f;
	public float turnSpeed = 50f;
	public GameObject hitSpace;

	
	public GameObject up;
	public GameObject down;
	public GameObject left;
	public GameObject right;
	private Animator hold;

	private stats stat;
	private RuntimeAnimatorController saved_cont;
	private bool attackDone = false;
	private RuntimeAnimatorController transforming;
	private GameObject tile;

	// Use this for initialization
	void Start () {
		anim = GetComponent ("Animator") as Animator;
		saved_cont = anim.runtimeAnimatorController;
		stat = gameObject.GetComponent ("stats") as stats;
		stat.isPlayer = true;
	}
	

		
		void FixedUpdate() 
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


		Collider2D hitBomb = Physics2D.OverlapCircle (hitSpace.transform.position, 0.20f, 1 << LayerMask.NameToLayer("Bomb"));
		if(hitBomb != null)
		{
			bombFunctions bomb = hitBomb.gameObject.GetComponent("bombFunctions") as bombFunctions;
			bomb.explode();
		}


		Collider2D hit = Physics2D.OverlapCircle(hitSpace.transform.position, 0.20f, 1 << LayerMask.NameToLayer("Victim"));
		if(hit!=null)
		   {
			 Brain temp = hit.gameObject.GetComponent("Brain") as Brain;
			 GameObject blood = (GameObject)Instantiate(temp.blood);
			 blood.transform.position = new Vector2(hit.gameObject.transform.position.x, hit.gameObject.transform.position.y);


			 Vector3 dir = transform.position - blood.transform.position; 
			 dir.z = 0; dir.Normalize();
			 float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			 blood.transform.rotation = Quaternion.Slerp (blood.transform.rotation, Quaternion.Euler (0, 0, angle), 1f);

			stats tStats = hit.gameObject.GetComponent("stats") as stats;
			tStats.damage();
		   }
	}



	public void loseForm()
	{
		Animator player = gameObject.GetComponent<Animator> ();
		player.runtimeAnimatorController = saved_cont;

		SpriteRenderer r = gameObject.GetComponent ("SpriteRenderer") as SpriteRenderer;
		r.material.color = Color.white;
	}

    

	public void transformation(RuntimeAnimatorController Monster, GameObject demonTile)
	{
		anim.SetBool ("Transform", true);
		transforming = Monster;
		tile = demonTile;
	}


	public void finishTransform() 
	{ 
		Animator player = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Transform", false);
		player.runtimeAnimatorController = transforming;
		Destroy (tile);
	}






}



