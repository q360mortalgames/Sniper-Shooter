using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager : MonoBehaviour {

	public Transform Target,Player;
	Vector3 LookPos;
	public static DirectionManager instance;
	public GameObject ArrowObj;
	void Start () 
	{
		instance = this;
		Target = GameObject.FindObjectOfType<Cell> ().transform;
	}
	
	void Update () 
	{
		transform.position = Player.transform.position;
		transform.rotation = Player.transform.rotation;
		LookPos = new Vector3 (Target.position.x, transform.position.y, Target.position.z);
		ArrowObj.transform.LookAt (LookPos);
	}
	public void ShowArrow(bool show)
	{
		ArrowObj.SetActive (show);
	}
}
