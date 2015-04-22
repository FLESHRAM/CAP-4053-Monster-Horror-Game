using UnityEngine;
using System.Collections;

public class victimsInfo : MonoBehaviour {

	private int girlCount = 0;
	private int manCount = 0;

	private int victimCount = 0;





	public void addGirl() { girlCount++; victimCount++; }
	public void addMan() { manCount++; victimCount++; }



	public int howManyGirls() { return girlCount; }
	public int howManyMen() { return manCount; }
	public int howManyVictims() { return victimCount; }



}
