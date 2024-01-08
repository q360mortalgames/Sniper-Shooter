using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleEffect : MonoBehaviour {
	public GameObject[] ScaleObjs;
	public bool UseDealy;

	public bool AllChilds;
	void OnEnable () 
	{
		if (AllChilds) 
		{
			ScaleObjs = new GameObject[transform.childCount];
			for (int i = 0; i < ScaleObjs.Length; i++) {
				ScaleObjs [i] = transform.GetChild (i).gameObject;
			}
		}
		for (int i = 0; i < ScaleObjs.Length; i++) {
			iTween.Stop (ScaleObjs [i]);
			ScaleObjs [i].transform.localScale = new Vector3 (0, 0, 0);
		}
		float delay = 0.2f;
		for (int i = 0; i < ScaleObjs.Length; i++) 
		{
			iTween.ScaleTo(ScaleObjs [i],iTween.Hash("scale",new Vector3(1,1,1),"delay",delay,"time",0.3f,"eastype",iTween.EaseType.easeOutBack));
			if(UseDealy)
				delay = delay+0.1f;
		}
	}
	

}
