using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionComletePage : MonoBehaviour {

	public GameObject Title1,Title2,Arrow1,Arrow2;
	void Start ()
	{
		iTween.MoveFrom (Title1.gameObject, iTween.Hash ("y", Title1.transform.position.x + 400, "time", 1,"delay",0.0f,"easetype",iTween.EaseType.easeInCubic));
		iTween.MoveFrom (Title2.gameObject, iTween.Hash ("y", Title2.transform.position.x - 400, "time", 1,"delay",0.0f,"easetype",iTween.EaseType.easeInCubic));
		iTween.MoveFrom (Arrow1.gameObject, iTween.Hash ("x", Arrow1.transform.position.x - 600, "time", 1,"delay",0.2f,"easetype",iTween.EaseType.easeInCubic));
		iTween.MoveFrom (Arrow2.gameObject, iTween.Hash ("x", Arrow2.transform.position.x + 600, "time", 1,"delay",0.2f,"easetype",iTween.EaseType.easeInCubic));

	}
	
	void Update () 
	{
		
	}
}
