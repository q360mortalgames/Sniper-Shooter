using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelFailed : MonoBehaviour {

	public GameObject Title;
    public Text Description, LevelText;
    public GameObject LoadingPage;
	void Start () 
	{
        //bannerAd.instance.showMediumBanner();
      //  FirebaseManager.instance.Levelfail(PlayerPrefs.GetInt("SelectedLevel"));
        if (AudioManager.instance)
		{
			AudioManager.instance.StopMusic (true);
			AudioManager.instance.PlaySound (5);
		}
		iTween.ScaleFrom (Title, iTween.Hash ("scale", new Vector3(0,0,0), "time", 0.8f, "eastype", iTween.EaseType.easeInCubic));
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LF, PlayerPrefs.GetInt("SelectedLevel"));
        //}
    //    Interstitial.instance.interstitialplay();
        if (GameManager.instance.FailReason==failReason.KilledInnocent)
        {
            Description.text = "You Killed an innocent!";
        }
        else if (GameManager.instance.FailReason == failReason.TargetMissed)
        {
            Description.text = "Target Escaped!";
        }
        else if (GameManager.instance.FailReason == failReason.PlayerDead)
        {
            Description.text = "You have been killed!";
        }

        LevelText.text = "" + PlayerPrefs.GetInt("SelectedLevel");
    }
	void ShowAd()
	{
		
	}
	
	public void mGames()
	{
        //Application.OpenURL ("market://search?q=pub:Zippy Games");
        //AdManager.instance.ShowMoreGames();
      //  Application.OpenURL("market://search?q=pub:Mortal Games");
    }
	void Update () 
	{
		
	}
    void Gohomepage()
    {
        Application.LoadLevel("Menu");
    }
    void GoWeaponspage()
    {
        Application.LoadLevel("WeaponSelection");
    }
    public void Home()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LF, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);
        //Application.LoadLevel ("Menu");
        Invoke("Gohomepage", 1.5f);
	}
	public void Retry()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LF, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);
        Invoke("GoWeaponspage", 1.5f);
    }
}
