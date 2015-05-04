using UnityEngine;
using System.Collections;

public class Neurons : MonoBehaviour {


	private Brain brain;
	private NodeAI nAI;
	private NodeAI_2 nAI_2;

	private bool checkDone = false;

	   // Current states for the AI
	private bool Idle;
	private bool Scanning;
	private bool Wandering;
	private bool Walking;
	private bool pickingUpBomb;
	private bool runningAway;
	private bool Hiding;
	private bool Stay;
	private bool Panic;
	   // Panic States
		private bool Flee;
	    private bool Fleeing;
		private bool PanicHide;
		private bool PanicHiding;
		private bool DesperateWithBomb;
	    private bool SuicideOnPlayer;


	private bool Monster;
		// Monster States
	   private bool ChasingPlayer;
	   private bool Attacking;

	private bool attacked = false;
	private GameObject lastAttacked = null;
	private bool interrupt;   // Signaling that player was spotted
		


	    // Components of AI memory
	private ArrayList hidingObjectMemory; // Seen hiding Objects
	private ArrayList bombMemory;   // Bombs AI has seen 
	private GameObject visiblePlayer;  // Holds the player if visible
	private Vector2 lastKnownPlayerPos; // The last place the player was seen

		
	  // Short term memory components
	private GameObject targetBomb; // A bomb that AI plans to pick up
	private GameObject targetHiding; // Hiding object intended to hide within
	private ArrayList occupiedMem;       // hididng objects that someone else is hiding in
	private int memCount;  // For how many frames they will remember occupied


	private int wait; // Causing AI to wait a certain number of frames
	private int count;



	// Use this for initialization
	void Start () {
	
		GameObject nod = GameObject.Find ("Nodes");
		nAI = nod.GetComponent ("NodeAI") as NodeAI;
		nAI_2 = nod.GetComponent ("NodeAI_2") as NodeAI_2;
		brain = gameObject.GetComponent ("Brain") as Brain;
		hidingObjectMemory = new ArrayList ();
		occupiedMem = new ArrayList ();
		bombMemory = new ArrayList ();



		Idle = true;
		Scanning = false;
		Wandering = false;
		pickingUpBomb = false;
		runningAway = false;
		Hiding = false;
		Panic = false;
		Monster = false;

		wait = 0;
		count = 0;
		memCount = 0;

	}
	
	// Update is called once per frame
	void Update () 
	{
		interrupt = false;
	
		ArrayList tempHiding = brain.hidingObjects ();
		ArrayList tempBombs = brain.getVisisbleBombs ();
		visiblePlayer = brain.visiblePlayer ();

		if(visiblePlayer != null) 
		{ lastKnownPlayerPos = visiblePlayer.transform.position; interrupt=true; brain.sprint (); attacked=false; lastAttacked=null; }

		else 
		{
			if(attacked && lastAttacked!=null)
			{
				visiblePlayer = lastAttacked;
				lastKnownPlayerPos = lastAttacked.transform.position;
				interrupt = true;
				brain.sprint ();
				attacked = false;
				lastAttacked=null;
			}
		}

		remember (hidingObjectMemory, tempHiding);
		remember (bombMemory, tempBombs);


		if(memCount>0) memCount++;     
		else if(memCount==0) occupiedMem.Clear ();
        

		if(nAI!=null)
		{
			if(nAI.nodesDone && brain.IsRunning) { goodOlFashionAI(); }
		}

		else { if(nAI_2.nodesDone && brain.IsRunning) { goodOlFashionAI(); } }

	}


	public void seePlayer(GameObject p)
	{
		attacked = true;
		lastAttacked = p;
	}


	// UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3
	private void check(int dir)
	{
		//print ("Check called");
		GameObject n = brain.closestNode ();
		NodeInfo nI;
		if(n!=null)
		{
			nI = n.GetComponent("NodeInfo") as NodeInfo;
			switch (dir)
			{
			  	case 0: if(nI.up != null) brain.seek (n, nI.up); break;
				case 1: if(nI.down != null) brain.seek (n, nI.down); break;
				case 2: if(nI.left != null) brain.seek (n, nI.left); break;
				case 3: if(nI.right != null) brain.seek (n, nI.right); break;
			}

		}
	}




	private void randomCheck()
	{
		int rand = Random.Range (1, 201);
		if(rand>0 && rand<51) check (2);
		else if(rand>50 && rand<101) check (0);
		else if(rand>100 && rand<151) check (3);
		else check (1);
	}





	public void setMonsterAI()
	{
		brain.interruptPath ();
		Monster = true;
		Idle = true;
		Scanning = false;
		Wandering = false;
		pickingUpBomb = false;
		runningAway = false;
		Hiding = false;
		Panic = false;

		wait = 0;
		count = 0;
		memCount = 0;
	}



	public void ressetAI()
	{
		brain.interruptPath ();
		Idle = true;
		Scanning = false;
		Wandering = false;
		pickingUpBomb = false;
		runningAway = false;
		Hiding = false;
		Panic = false;
		Monster = false;
		
		wait = 0;
		count = 0;
		memCount = 0;
	}






	private void remember(ArrayList mem, ArrayList input)
	{
		if(input.Count > 0)
		{
			for(int i=0; i<input.Count; i++)
			{
				GameObject t = (GameObject)input[i];
				bombFunctions func = t.GetComponent("bombFunctions") as bombFunctions;
				bool markedBomb = false;

				if(func != null) markedBomb = func.isMarked();

				if (!mem.Contains(t) && !markedBomb && !occupiedMem.Contains(t)) mem.Add(t);
				else if(mem.Contains (t) && markedBomb) mem.Remove(t);
			}
		}
	}







	private void goodOlFashionAI()
	{
		if(wait == 0)
		{
			  // Base Idle STATE
			if (Idle) 
			{
				if(interrupt) 
				{ Idle=false; if(!Monster) runningAway=true; else ChasingPlayer=true;  wait=(int)(3*Time.deltaTime); }
				else { Scanning = true; Idle = false; count = 0; }
			}


			 // AI scans the area
			if(Scanning)
			{
				if(Monster)
				{
					Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
					if(p!=null) { brain.attack (); }
				}

				if(interrupt) { Scanning=false; if(!Monster) runningAway=true; else ChasingPlayer=true; wait=(int)(3*Time.deltaTime); }
              else
			  {
					if(count<4) { check(count); wait=(int)(3*Time.deltaTime); count++;} 
					if(count == 4 && (bombMemory.Count==0 || brain.hasBomb())) { wait=(int)(5*Time.deltaTime); Scanning=false; Wandering=true;}
					else if (count == 4 && bombMemory.Count>0 && !brain.hasBomb()) 
					{ 
						
						GameObject closestBomb = null;
						for(int i=0; i<bombMemory.Count; i++)
						{
							GameObject t = (GameObject)bombMemory[i];
							
							if(closestBomb == null) closestBomb = t;
							
							else
							{
								
								if(t!=null && Vector2.Distance (transform.position, t.transform.position) < Vector2.Distance(transform.position, closestBomb.transform.position))
									closestBomb = t;
							}
						}
						
						if(closestBomb != null)
						{
							targetBomb = closestBomb;
							bombMemory.Remove (closestBomb);
							
							bombFunctions func = targetBomb.GetComponent("bombFunctions") as bombFunctions;
							if(func.isMarked()) { targetBomb = null; wait=(int)(3*Time.deltaTime); Scanning=false; Wandering=true; }
							else
							{
								func.mark();

								GameObject close1 = brain.closestNode();
								GameObject close2 = func.closestNode();

								//print ("Bomb Path from "+close1+" to "+close2);

								brain.interruptPath();
								brain.seek (close1, close2);
								brain.printPath();
								wait=(int)(3*Time.deltaTime); Scanning=false; pickingUpBomb=true;
							}
							
							
						}
						
						else { wait=(int)(3*Time.deltaTime); Scanning=false; Wandering=true; }
						
					} 
			  }

			}



			  // AI wanders aimlessly
			if(Wandering)
			{

				if(Monster)
				{
					Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
					if(p!=null) { brain.attack (); }
				}


				if(interrupt) { Wandering=false; if(!Monster) runningAway=true; else ChasingPlayer=true; wait=(int)(3*Time.deltaTime); }
				else
				{
					int chance = Random.Range(1, 101);
					if(chance>20 && chance<56) { Wandering=false; Idle=true; wait=(int)(5*Time.deltaTime); }
					else
					{
						ArrayList n = brain.getNodesinSight();
						if(n.Count == 0) { Wandering=false; Idle=true; }
						else if (n.Count > 0)
						{
							int randIndex = Random.Range (0, n.Count-1);
							if(randIndex>0 && randIndex<n.Count-1)
							{
								brain.interruptPath();
								brain.seek (brain.closestNode(), (GameObject)n[randIndex]);
								Wandering=false; Walking=true; wait=(int)(3*Time.deltaTime);
							}

						}
					}
				}

			}


			 // Moving to a location
			if(Walking)
			{
				if(Monster)
				{
					Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
					if(p!=null) { brain.attack (); }
				}

				Collider2D bomb = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Bomb"));
				if(bomb!=null && !brain.hasBomb()) { brain.pickUpBomb(bomb.gameObject); }

				if (!brain.pathing) 
				{ 
					Walking=false;

					if(interrupt) { if(!Monster) runningAway=true; else ChasingPlayer=true; }
					else { Wandering=true; }  

					wait=(int)(3*Time.deltaTime);
				}

				else
				{
					if(interrupt)
					{
						brain.interruptPath();
						Walking=false; 
						if(!Monster)
						{
							runningAway=true;
							wait=(int)(5*Time.deltaTime);
						}
							 
						else
						{
							ChasingPlayer=true;
							wait=(int)(1*Time.deltaTime);
						}

					}
				}

			
			}


			 // Moving to retrieve a bomb
			if(pickingUpBomb)
			{
				if(Monster)
				{
					Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
					if(p!=null) { brain.attack (); }
				}

				if (!brain.pathing) 
				{
					if(targetBomb==null) { brain.interruptPath(); }
					else { brain.pickUpBomb(targetBomb); }

					pickingUpBomb=false;
					if(interrupt) { if(!Monster) runningAway=true; else ChasingPlayer=true; }
					else { Wandering=true; }  

					wait=(int)(5*Time.deltaTime);
				}
			}

			 

			 // Feeling in fear
			if(runningAway)
			{
				brain.sprint();
				int chanceToForget = Random.Range (1, 101);
				if(chanceToForget<41) hidingObjectMemory.Clear();


				if(hidingObjectMemory.Count>0)
				{
					GameObject furthest = null;

					for(int i=0; i<hidingObjectMemory.Count; i++)
					{
						GameObject t = (GameObject)hidingObjectMemory[i];

						if(furthest==null) furthest=t;

						else
						{
							if(Vector2.Distance (t.transform.position, lastKnownPlayerPos) > Vector2.Distance(furthest.transform.position, lastKnownPlayerPos))
								furthest=t;
						}
					}


					targetHiding = furthest;
					HidingObject hide = targetHiding.GetComponent("HidingObject") as HidingObject;
					brain.seek (brain.closestNode(), hide.node);
					runningAway=false; Hiding=true; wait=(int)(5*Time.deltaTime);
				}


				else
				{
					int bravery = Random.Range(1, 101);

					runningAway = false;
					Panic=true;
						Flee=true;
						Fleeing=false;
					    PanicHide=false;
					    PanicHiding=false;
				        DesperateWithBomb=false;
						SuicideOnPlayer=false;

					randomCheck();	
					count=(int)(15*Time.deltaTime);  // frames to panic in
					if(brain.hasBomb() && bravery>65) { Flee=false; DesperateWithBomb=true; }
				}


			}
			
			// Hiding in a place
			if(Hiding)
			{
				Collider2D bomb = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Bomb"));
				if(bomb!=null && !brain.hasBomb()) { brain.pickUpBomb(bomb.gameObject); }

				if(!brain.pathing)
				{
					HidingObject h = targetHiding.GetComponent("HidingObject") as HidingObject;
					Vector2 hPos = h.node.transform.position;
					gameObject.transform.position = new Vector2(hPos.x, hPos.y);

					if(gameObject.renderer.material.color==Color.clear)
					{
						Hiding = false; Stay = true;
						wait = (int)(10*Time.deltaTime);
					}

					else
					{
						Hiding=false; 
						occupiedMem.Add(targetHiding);
						memCount = 200;
						targetHiding = null;

						int rand = Random.Range (1, 101);
						if(rand<84 && rand>56)
						{
							Hiding = false; Idle = true;
						}

						else
						{
							Hiding = false; runningAway = true;
						}
					}

				}
			}


			  // Staying in a hiding spot
			if(Stay)
			{
				if(visiblePlayer==null)
				{
					if(brain.hasBomb())
					{
						int chance = Random.Range (1, 101);
						if(chance<80)
						{
							brain.hideBomb(targetHiding);
						}

						Stay=false; Wandering=true; wait=(int)(3*Time.deltaTime);
						targetHiding=null;
					}

					else 
					{
						int chance = Random.Range (1, 101);
						if(chance>80)
						{
							Stay=false; Wandering=true; wait=(int)(3*Time.deltaTime);
							targetHiding=null;
						}
					}

				}

				else wait=(int)(15*Time.deltaTime);
			}


			  // Special Panicked state, prompts for more erratic behavior
			if(Panic)
			{
				bool hold=false;

				if(gameObject.renderer.material.color==Color.clear) hold=true;

				if(count%2 == 0) brain.sprint ();
				if(count>0) count--;

				if(visiblePlayer!=null) count=(int)(20*Time.deltaTime); // Keep freaking out



				if(Flee && !brain.pathing && !hold)
				{
					GameObject furthest = null;
					ArrayList visible = brain.getNodesinSight();
					
					for(int i=0; i<visible.Count; i++)
					{
						GameObject t = (GameObject)visible[i];
						if(furthest==null) furthest=t;
						else
						{
							if(Vector2.Distance(t.transform.position, lastKnownPlayerPos) > Vector2.Distance(furthest.transform.position, lastKnownPlayerPos))
								furthest=t;
						}
					}
					
					if(furthest!=null) {  brain.interruptPath(); brain.seek (brain.closestNode(), furthest); Flee=false; Fleeing=true; }
					else { randomCheck(); }

					wait=(int)(3*Time.deltaTime);
				}


				if(Fleeing)
				{
					Collider2D bomb = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Bomb"));
					if(bomb!=null && !brain.hasBomb()) { brain.pickUpBomb(bomb.gameObject); }

                    if(!brain.pathing)
					{
						int chance = Random.Range(1, 101);
						if(chance<45 && visiblePlayer == null && hidingObjectMemory.Count>0 && targetHiding==null && Vector2.Distance (transform.position, lastKnownPlayerPos) > 3f)
						{
							chance = Random.Range(1, 101);
							if(chance>5 && chance<45) { Fleeing=false; PanicHide=true; }
							else { randomCheck(); Fleeing=false; Flee=true; }
						}

						else
						{
							if(brain.hasBomb()) 
							{
								chance = Random.Range(1, 101);
								if(chance>10 && chance<93) { Fleeing=false; DesperateWithBomb=true; }
								else { randomCheck(); Fleeing=false; Flee=true; }
							}
						}

						wait=(int)(3*Time.deltaTime);
					}
				}


				if(PanicHide)
				{
					targetHiding = (GameObject)hidingObjectMemory[0];
					HidingObject h = targetHiding.GetComponent("HidingObject") as HidingObject;
					
					brain.seek (brain.closestNode(), h.node);
					PanicHide=false; PanicHiding=true; wait=(int)(3*Time.deltaTime);
				}


				if(PanicHiding)
				{

					Collider2D bomb = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Bomb"));
					if(bomb!=null && !brain.hasBomb()) { brain.pickUpBomb(bomb.gameObject); }


					if(!brain.pathing)
					{
						HidingObject h = targetHiding.GetComponent("HidingObject") as HidingObject;
						Vector2 hPos = h.node.transform.position;
						gameObject.transform.position = new Vector2(hPos.x, hPos.y);

						if(gameObject.renderer.material.color!=Color.clear)
						{
							//HidingObject h = targetHiding.GetComponent("HidingObject") as HidingObject;
							Collider2D look = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Victim"));
							if(look != null && look.gameObject.renderer.material.color==Color.clear)
							{ h.forceOut(gameObject); }
							
							else
							{
								look = Physics2D.OverlapCircle(transform.position, 1f, 1 << LayerMask.NameToLayer("Bomb"));
								if(look != null && look.gameObject.renderer.material.color==Color.clear)
								{
									h.forceOut(gameObject);
									brain.pickUpBomb(look.gameObject);
								}
								
								else
								{
									targetHiding = null;
									lastKnownPlayerPos = transform.position;
									PanicHiding=false; Flee=true;
								}
							}
						}


						else { PanicHiding=false; Flee=true; }

						wait=(int)(3*Time.deltaTime);

					}

				}


				if(DesperateWithBomb)
				{
					int chance = Random.Range(1, 101);
					if( (chance>10 && chance<20) || (chance>70 && chance<74)) { brain.fiddle (); }
					else if(chance<70 || chance>30) 
					{
						Collider2D n = Physics2D.OverlapCircle(lastKnownPlayerPos, 0.3f, 1 << LayerMask.NameToLayer("Node"));
						if(n!=null)
						{
							brain.interruptPath();
							brain.seek(brain.closestNode(), n.gameObject);
							DesperateWithBomb=false; SuicideOnPlayer=true;
						}

						else { randomCheck(); DesperateWithBomb=false; Flee=true;}
					}

					else { randomCheck(); DesperateWithBomb=false; Flee=true; }

					wait=(int)(3*Time.deltaTime);
				}



				if(SuicideOnPlayer)
				{
					if(!brain.pathing)
					{
						if(visiblePlayer!=null)
						{
							if(Vector2.Distance (transform.position, lastKnownPlayerPos) < 3f) { brain.fiddle (); SuicideOnPlayer=false; Flee=true; }
							else
							{
								Collider2D n = Physics2D.OverlapCircle(lastKnownPlayerPos, 0.3f, 1 << LayerMask.NameToLayer("Node"));
								if(n!=null) { brain.seek(brain.closestNode(), n.gameObject); }
								else { randomCheck(); SuicideOnPlayer=false; Flee=true;}
							}
						}

						else { randomCheck(); SuicideOnPlayer=false; Flee=true; }
						wait=(int)(3*Time.deltaTime);
					}

				}


				if(count==0)
				{
					brain.interruptPath();
					Panic=false;
						Flee=false;
						Fleeing=false;
						PanicHide=false;
						PanicHiding=false;
						DesperateWithBomb=false;
						SuicideOnPlayer=false;
					Idle=true;
				}
			}



			if(ChasingPlayer)
			{
				Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
				if(p!=null) { brain.attack (); }

				Collider2D n = Physics2D.OverlapCircle(lastKnownPlayerPos, 0.3f, 1 << LayerMask.NameToLayer("Node"));
				if(n!=null)
				{
					brain.interruptPath();
					brain.seek(brain.closestNode(), n.gameObject);
					ChasingPlayer=false; Attacking=true;
				}
				
				else { randomCheck(); ChasingPlayer=false; Idle=true;}

				wait=(int)(3*Time.deltaTime);
			}


			if(Attacking)
			{
				Collider2D p = Physics2D.OverlapCircle(brain.hitSpace.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Player"));
				if(p!=null) { brain.attack (); }

				if(!brain.pathing)
				{
					if(visiblePlayer!=null)
					{

						if(Vector2.Distance (transform.position, lastKnownPlayerPos) < 0.2f) 
						{ 

							brain.attack (); Attacking=false; ChasingPlayer=true; 
						}
						else
						{
							Collider2D n = Physics2D.OverlapCircle(lastKnownPlayerPos, 0.3f, 1 << LayerMask.NameToLayer("Node"));
							if(n!=null) { brain.seek(brain.closestNode(), n.gameObject); }
							else { randomCheck(); Attacking=false; Idle=true;}
						}
					}
					
					else { randomCheck(); Attacking=false; Idle=true; }
					wait=(int)(3*Time.deltaTime);
				}
			}



		}


		if(wait > 0) wait--;

	}

}
