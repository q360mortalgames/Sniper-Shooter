
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour {
	int selLevel;
	public int[] LevelReward,LevelBonus;
	public Text LevelText, AlienCountText, RewardText, BonusText, TotalRewardText;
	public GameObject ScoreBoard;
    public GameObject LoadingPage;
	
	void Start () 
	{
      //  Interstitial.instance.interstitialplay();
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LC, PlayerPrefs.GetInt("SelectedLevel"));
        //}

        //bannerAd.instance.showMediumBanner();
        // FirebaseManager.instance.Levelcomplete(PlayerPrefs.GetInt("SelectedLevel"));

        if (AudioManager.instance) {
			AudioManager.instance.StopMusic (true);
			AudioManager.instance.PlaySound (4);
		}
		selLevel = PlayerPrefs.GetInt ("SelectedLevel");
		if(PlayerPrefs.GetInt ("LevelsUnlocked")==selLevel)
			{
			PlayerPrefs.SetInt ("LevelsUnlocked", selLevel + 1);
			Debug.Log ("UnlockLevel"+PlayerPrefs.GetInt ("LevelsUnlocked"));

			}

		LevelText.text = ""+PlayerPrefs.GetInt ("SelectedLevel");
		AlienCountText.text = "  "+GameManager.instance.EnemiesKilled;
		RewardText.text = "  "+LevelReward[selLevel-1];
		BonusText.text = "  "+LevelBonus[selLevel-1];
		TotalRewardText.text = "  " + (LevelReward [selLevel - 1] + LevelBonus [selLevel - 1]);
		PlayerPrefs.SetInt ("TotalMoney", PlayerPrefs.GetInt ("TotalMoney") + LevelReward [selLevel - 1] + LevelBonus [selLevel - 1]);
//		iTween.MoveFrom (ScoreBoard, iTween.Hash ("x", ScoreBoard.transform.position.x + 1500, "time", 0.5f, "eastype", iTween.EaseType.easeInCubic));
		iTween.ScaleFrom (ScoreBoard, iTween.Hash ("scale", new Vector3(0,0,0), "time", 1f, "eastype", iTween.EaseType.easeInCubic));
		AdsManager.Instance.ShowAd();

	}
	public void mGames()
	{
		//Application.OpenURL ("market://search?q=pub:Zippy Games");
	}
	public void fbShare()
	{
		//FBMainMenu.Instance.ShareLevelCompleteNormal ();
	}
	
	void Update ()
	{
		
	}
  
    public void Next()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LC, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);

        //Application.LoadLevel ("WeaponSelection");
        Invoke("gotoNext", 2);
	}

    void gotoNext()
    {
        Application.LoadLevel("WeaponSelection");
    }
	public void Home()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LC, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);

        //Application.LoadLevel ("Menu");
        Invoke("GotoMenu", 2);
    }
    void GotoMenu()
    {
        Application.LoadLevel("Menu");
    }
	public void Replay()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LC, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);
        Invoke("GotoReplay", 2);
  //      Application.LoadLevel ("Menu");
		//LoadLevel (PlayerPrefs.GetInt ("SelectedLevel"));
	}
    void GotoReplay()
    {
        Application.LoadLevel("Menu");
        LoadLevel(PlayerPrefs.GetInt("SelectedLevel"));
    }
	public void LoadLevel(int level)
	{
			PlayerPrefs.SetInt ("SelectedLevel", level);
			//if (level < 6) {
			//	Loading.LevelToLoad = "Theme1";
			//	Application.LoadLevel ("Loading");

			//} else {
			//	Loading.LevelToLoad = "Theme2";
			//	Application.LoadLevel ("Loading");

			//}

        Loading.LevelToLoad =  "Theme"+PlayerPrefs.GetInt("SelectedTheme");
        Application.LoadLevel("Loading");
    }

}
