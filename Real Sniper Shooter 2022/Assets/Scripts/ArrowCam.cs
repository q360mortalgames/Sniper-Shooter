using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCam : MonoBehaviour {
	public Transform Player;
	void Start ()
	{
		
	}
	
	void Update () 
	{
		transform.position = Player.position;
		transform.rotation = Player.rotation;
		
	}
}
