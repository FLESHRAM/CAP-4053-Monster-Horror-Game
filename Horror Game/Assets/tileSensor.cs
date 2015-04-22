//using UnityEngine;
//using System.Collections;
//
//public class tileSensor : MonoBehaviour {
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.64f, 1 << LayerMask.NameToLayer("Player"));
//
//		if(hit!=null)
//		{
//			PlayerControl p = hit.gameObject.GetComponent("PlayerControl") as PlayerControl;
//			Animator m = gameObject.GetComponentInChildren<Animator>();
//			p.transformation(m);
//		}
//	}
//}
