using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public GameObject MenuPanel, SettingsPannel,RatePannel,StoryPannel,StorePanel;
	public GameObject StoryBoard,SettingsBoard,StoreBoard;
	public string StoryTextMessge="";
	public GameObject[] StoryTexts;

	public GameObject[] ButtonsRight,ButttonsTop,ButtonsBottom;
	static bool checkSignin=false;

    public static bool ShowStoreFirst;
    public static Menu instace;
	void Start () 
	{
	    
		Time.timeScale = 1;
        
        instace = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	//	PlayerPrefs.DeleteAll ();
		if (!PlayerPrefs.HasKey ("StoryShown")) 
		{
			Invoke ("ShowStory", 1);
			PlayerPrefs.SetString ("StoryShown", "true");

		}
		SettingsPannel.SetActive (false);
        //MenuPanel.SetActive(true);

        if (!ShowStoreFirst)
        {
            Invoke("TweenInMenu", 0f);
        }
        else
        {
            Invoke("ShowStore", 0f);
			
			
        }
         }
	void TweenInMenu()
	{
        //for (int i = 0; i < ButtonsRight.Length; i++) 
        //{
        ////	iTween.MoveFrom (ButtonsRight[i], iTween.Hash ("y",ButtonsRight[i].transform.position.y-800, "time", 0.5f, "delay", 0.1f , "easetype", iTween.EaseType.easeInCubic));
        //}
        MenuPanel.SetActive(true);
        //		for (int i = 0; i < ButttonsTop.Length; i++) 
        //		{
        //			iTween.MoveFrom (ButttonsTop[i], iTween.Hash ("y",ButttonsTop[i].transform.position.y+500, "time", 0.2f, "delay", 0.5f * i, "easetype", iTween.EaseType.easeInCubic));
        //		}
        //		for (int i = 0; i < ButtonsBottom.Length; i++) 
        //		{
        //			iTween.MoveFrom (ButtonsBottom[i], iTween.Hash ("y",ButtonsBottom[i].transform.position.y-500, "time", 0.2f, "delay", 0.5f * i, "easetype", iTween.EaseType.easeInCubic));
        //		}
    }
	void Update () 
	{
		
	}
    public GameObject LoadingPage, ExitPage;
    public void ShowExitpage()
    {
        ExitPage.SetActive(true);
     
    }
    public void Exit(bool Exit)
    {
        if(Exit)
        {
            Application.Quit();

        }else
        {
            ExitPage.SetActive(false);
         

        }
    }
	public void GotoWeaponSel()
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.Upgrade, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        LoadingPage.SetActive(true);
        Invoke("WeaponSel", 1.5f);

        //Application.LoadLevel ("WeaponSelection");
	}
    void WeaponSel()
    {
        Application.LoadLevel("WeaponSelection");

    }
	public void ShowSettings()
	{
		
		Debug.Log ("Settings");
		SettingsPannel.SetActive (true);
        MenuPanel.SetActive(false);
		//SettingsBoard.transform.localScale = new Vector3 (1, 1, 1);
		//iTween.ScaleFrom(SettingsBoard,iTween.Hash("scale",new Vector3(0,0,0),"time",1.5f,"easytype",iTween.EaseType.easeInCubic));
		AudioManager.instance.PlaySound (2);


	}

	public void ShowStore()
	{
		
		Debug.Log ("Settings");
		StorePanel.SetActive (true);
        MenuPanel.SetActive(false);
        ShowStoreFirst = false;
        //StorePanel.transform.localScale = new Vector3 (1, 1, 1);
        //	iTween.ScaleFrom(StoreBoard,iTween.Hash("scale",new Vector3(0,0,0),"time",1.5f,"easytype",iTween.EaseType.easeInCubic));
        AudioManager.instance.PlaySound (2);
	}
	public void ShowRateUs(bool show)
	{
//		RatePannel.SetActive (show);
	}

	public void ShowStory()
	{
		AudioManager.instance.PlaySound (2);
		StoryPannel.SetActive (true);
		iTween.ScaleFrom(StoryBoard,iTween.Hash("scale",new Vector3(0,0,0),"time",1.5f,"easytype",iTween.EaseType.easeInCubic));

	}
	public void HideStory()
	{
		StoryPannel.SetActive (false);
		AudioManager.instance.PlaySound (3);
	}
	public void ShowAchievements()
	{
      //  AdManager.instance.ShowAchievements();
	}
	public void ShowLeadeBoard()
	{
      //  AdManager.instance.ShowLeaderBoards();
    }
	public void mGames()
	{
        //AdManager.instance.ShowMoreGames();
      //  Application.OpenURL("https://play.google.com/store/apps/developer?id=Tenstarfreegames&hl=en");
    }
	public void RemoveAds()
	{
		//GoogleIAB.purchaseProduct (InAppPurchaseManager.allSkus [0]);
	}
	public void StoryNext()
	{
		if (StoryTexts [0].activeInHierarchy) 
		{
			//	StoryTexts [0].SetActive (false);
			//	StoryTexts [1].SetActive (true);
			HideStory();
			AudioManager.instance.PlaySound (2);
		} else 
		{
			HideStory ();
		}
	}
	IEnumerator	TypeText()
	{
		foreach (char letter in StoryTextMessge.ToCharArray()) {
//			StoryText.text += letter;
			//			if (sound)
			//				audio.PlayOneShot (sound);
			yield return 0;
			yield return new WaitForSeconds (0.1f);
		}
	}
    public void ShowMenu()
    {
        MenuPanel.SetActive(true);

    }


}
