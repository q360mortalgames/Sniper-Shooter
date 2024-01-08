using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour,IPointerDownHandler
{

	public int sound=0;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log ("ButtonSound");
		if (AudioManager.instance) {
			AudioManager.instance.PlaySound (sound);
		}
	}
}
