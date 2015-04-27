using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {


	public GameObject loadingImage;
	private GameObject director;



	public void loadScene(int level)
	{
		if(loadingImage!=null) loadingImage.SetActive (true);
		Application.LoadLevel (level);
	}
}
