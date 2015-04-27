using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {


	public GameObject loadingImage;


	public void loadScene(int level)
	{
		loadingImage.SetActive (true);
		Application.LoadLevel (level);
	}
}
