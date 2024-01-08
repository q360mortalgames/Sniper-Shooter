using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
	public static string LevelToLoad = "Menu";
	AsyncOperation loader;
	public Image LoadingBar;

	void Start () 
	{
        //bannerAd.instance.showMediumBanner();
		loader= Application.LoadLevelAsync (LevelToLoad);
	}
	
	void Update ()
	{
		Debug.Log (loader.progress*100);
		LoadingBar.fillAmount = loader.progress;

	}

}
