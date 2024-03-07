using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchControlsKit;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum failReason
{
    KilledInnocent,
    TimOut,
    TargetMissed,
    PlayerDead,
}
public class GameManager : MonoBehaviour {


    public string[] MissionTexts;
    public Text MissionTextHUD;
    public int TargetKills=1;
	public bool testLevel;
	public Image RefillCircle;
	public static GameManager instance;
	public Image CellActivator,CellCollector;

    public failReason FailReason=failReason.TimOut;

	public GameObject chargingHUD;
	public Image ChargeStatus;
	public GameObject CollectText;
	public GameObject EnemyIndicator;
	public GameObject CanvasMain;

	public int SelectedLevel;
	public GameObject Player;
	public Transform[] PlayerPos;
	public GameObject[] Levels,LevelMissions,LevelChars;
	public GameObject Level1Help;
	public GameObject HUD;
	public Text PopUpTextRef;
	public TCKJoystick JoyStick;
	int curwep;

	public int Health,maxHealth;
	public Text HealthText;
	public Image HealthBarFill;

	public Image curwepIcon;
	public Sprite[] wepSprites;
	public Image nextwepIcon;
	public Sprite[] WepSrpites2;
	public int curBulletsLeft, curAmmo;
	public Text AmmoText;

	public bool PlayerDead=false;
	public Image FadeImage;
	public GameObject SavePopUp;

	public GameObject PausePanel;

	public GameObject Help1, Help2,Help3,Help4;

	public GameObject MissionCompleteObj;

	public int EnemiesKilled = 0;

    public GameObject ZoomPanel;

    public GameObject[] MissionTargets;
    public GameObject MissionObjects;

    public GameObject GameCanvas;
    public GameObject NewHelp;
    [HideInInspector]
    public bool FistshotDone;
	
	public void Changeweapon(bool next)
	{
		Debug.Log ("Cur1" + curwep);
		if (next)
		{
			curwep = PlayerWeapons.instance.currentWeapon;
			if (curwep < 3)
				curwep++;
			else
				curwep = 1;
		} 
		else
		{
			if (curwep > 1)
				curwep--;
			else
				curwep = 3;
		}
		Debug.Log ("Cur2" + curwep);
		UpdateHUDweapons ();


	}
    public Slider ZoomSlider;
    public GameObject Sliderparent, ZoomBtn;

    public void ChangeweaponCyclic()
	{
        /*
		curwep = PlayerWeapons.instance.currentWeapon;
		if (Time.timeScale > 0) {
			if (curwep < 3)
				curwep++;
			else
				curwep = 1;
			UpdateHUDweapons ();
		}
        */

	}
    public void CivilianKilled()
    {
        FailReason = failReason.KilledInnocent;
        Invoke("LevelFail", 1);
    }
	public void UpdateHUDweapons()
	{
        /*
		if (curwep == 3)
			AmmoText.gameObject.SetActive (false);
		else
			AmmoText.gameObject.SetActive (true);

		PlayerWeapons.instance.StartCoroutine (PlayerWeapons.instance.SelectWeapon (curwep));
		curwepIcon.sprite = wepSprites [curwep - 1];

		nextwepIcon.sprite = WepSrpites2 [curwep-1];
        */

	}
    //	public void FadeScene
    [HideInInspector]
    public GameObject CurLevelObj;
    void Start()
    {

        

    }
    void Awake () 
	{

        //PlayerPrefs.SetInt("SelWeapon1",2);
        Debug.Log("SelGun" + PlayerPrefs.GetInt("SelWeapon1"));

        if (AudioManager.instance) {
			AudioManager.instance.StopMusic (false);
		}
		int EnemiesKilled = 0;
		curwep = 1;
//		Health=FPSPlayer.inst
		instance = this;	
		if (!testLevel)
		{
         // PlayerPrefs.SetInt("SelectedLevel",1);
            SelectedLevel = PlayerPrefs.GetInt ("SelectedLevel")%5;
             //SelectedLevel = 4;
			if (SelectedLevel == 0)
				SelectedLevel = 5;
		}
        MissionObjects.SetActive(false);
		RefillCircle.gameObject.SetActive (false);
		CellActivator.gameObject.SetActive (false);
		CellCollector.gameObject.SetActive (false);
		Debug.Log ("SelLevl"+SelectedLevel);
        Player.transform.position = PlayerPos[SelectedLevel - 1].position;
        Player.transform.rotation = PlayerPos[SelectedLevel - 1].transform.rotation;
        for (int i	=	0;i<Levels.Length;i++)
		{
			Levels[i].SetActive(false);

		}
        CurLevelObj = Levels[SelectedLevel - 1];
        Levels[SelectedLevel - 1].SetActive (true);

        MissionTextHUD.text = MissionTexts[SelectedLevel - 1];
        for (int i	=	0;i<LevelMissions.Length;i++)
		{
			LevelMissions[i].SetActive(false);
		}
        for (int i = 0; i < LevelChars.Length; i++)
        {
            LevelChars[i].SetActive(false);
        }
        TargetKills =CurLevelObj.GetComponent<AlertManager>().TargetKills;

        Player.SetActive(true);
       

        Invoke ("DelayedLevelHelp", 0.3f);
		FadeImage.gameObject.SetActive (false);
        //ShowMission(true);
        Time.timeScale = 0;
        /*if (SelectedLevel == 1&&PlayerPrefs.GetInt ("SelectedLevel")<5) 
		{
			Help1.SetActive (true);
			nextwepIcon.gameObject.SetActive (false);
		}*/
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.InGame, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        GameCanvas.SetActive(false);
    }
   
public void ShowMission(bool show)
    {
        MissionObjects.SetActive(show);
		LevelMissions[SelectedLevel - 1].SetActive (true);
        LevelChars[SelectedLevel - 1].SetActive(true);

        if (show)
        {
            //Time.timeScale = 0;
            GameCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            GameCanvas.SetActive(true);

            if(PlayerPrefs.GetInt("SelectedLevel")==1)
            {
                Time.timeScale = 0;
                NewHelp.SetActive(true);
            }
            //Player.transform.position = PlayerPos[SelectedLevel - 1].position;
            //Player.transform.rotation = PlayerPos[SelectedLevel - 1].transform.rotation;
            GameObject.Find("LevelCamera").SetActive(false);

            
        }
    }
    public bool LastTargetTokill, BulletHittedEnemy;
    void Update () 
	{
        if(EnemiesKilled>=TargetKills)
        {
            LevelCompleted();
        }
        if (EnemiesKilled == TargetKills-1)
        {
            if(!LastTargetTokill)
            LastTargetTokill = true;
        }
        if (chargingHUD.activeInHierarchy) 
		{
			ChargeStatus.fillAmount = 1-(Cell.instance.timeleft / Cell.instance.ChargeTime);
		}

		AmmoText.text = curBulletsLeft + "/" + curAmmo;


        if (SelectedLevel == 1)
        {
            if (Input.GetMouseButtonDown(0)&&NewHelp.activeInHierarchy)
            {
                NewHelp.SetActive(false);
                Time.timeScale = 1;
                /*
				if (Help1.activeInHierarchy&&Input.mousePosition.x > Screen.width/2)
				{
					Help1.SetActive (false);
                    //Invoke ("HelpMove",1);
                   
				}
				if (Help2.activeInHierarchy && Input.mousePosition.x < Screen.width/2)
				{
					Help2.SetActive (false);
					Invoke ("showShootHelp",2);
				}
			
                */
            }
        }
	}
	public void showShootHelp()
	{
		Help3.SetActive (true);
		//Time.timeScale = 0;
	}
	public void hideShootHelp()
	{
		Help3.SetActive (false);
		//Time.timeScale = 1;
		nextwepIcon.gameObject.SetActive (true);
		Invoke ("showSwitchHelp", 2);
	}
	public void showSwitchHelp()
	{
		Help4.SetActive (true);
		//Time.timeScale = 0;
	}
	public void hideSwitchHelp()
	{
		Help4.SetActive (false);
		//Time.timeScale = 1;
	}
	void HelpMove()
	{
		//Help2.SetActive (true);
        hideSwitchHelp();//temp
	}
	public void UpdateHealth()
	{
		Health = (int)(FPSPlayer.instance.hitPoints);
		maxHealth = (int)(FPSPlayer.instance.maximumHitPoints);
		HealthText.text = Health.ToString ()+"/"+maxHealth.ToString();
		HealthBarFill.fillAmount = (float)Health / (float)maxHealth;

	}
	public void ammoFRefillStatus(float value)
	{
		RefillCircle.fillAmount = value;

	}
	bool LifeGiven;
	public void ShowSavePopup()
	{
        
		if (!LifeGiven) 
		{
			FadeImage.gameObject.SetActive (true);
			SavePopUp.SetActive (true);
			Time.timeScale = 0;
			LifeGiven = true;
		} else {
			LetHimDie ();
		}
        
    }
	public void saveAndResume()
	{
//		#if UNITY_EDITOR
//		FPSPlayer.instance.hitPoints = FPSPlayer.instance.maximumHitPoints*0.5f;
//		SavePopUp.SetActive (false);
//		Time.timeScale = 1;
//		FadeImage.gameObject.SetActive (false);
//		UpdateHealth ();
//#endif

		//     AdManager.instance.showRewardVideo();
		//	AdManager.instance.ShowUnityAd();

		AdsManagerRwd.Instance.ShowRewardedAd((bool status) => {
			FPSPlayer.instance.hitPoints = FPSPlayer.instance.maximumHitPoints * 0.5f;
			SavePopUp.SetActive(false);
			Time.timeScale = 1;
			FadeImage.gameObject.SetActive(false);
			UpdateHealth();
		});

         // venkat
        /*
		if (Advertisement.IsReady ("rewardedVideo")) {


		Advertisement.Show ("rewardedVideo", new ShowOptions {
		resultCallback = result => {
		//Failed, Skipped, Finished
		Debug.Log ("UnityAdsss=" + result.ToString ());
		if (result.ToString () == "Finished") {
		FPSPlayer.instance.hitPoints = FPSPlayer.instance.maximumHitPoints*0.5f;
		SavePopUp.SetActive (false);
		Time.timeScale = 1;
		FadeImage.gameObject.SetActive (false);
		UpdateHealth ();

		}
		}
		});
		} else 
		{
		if(gameConfigs.mee)
		gameConfigs.mee.jarToast ("Video is getting ready, please wait");

		}
        */


    }
	public void LetHimDie()
	{
        GameManager.instance.FailReason = failReason.PlayerDead;

        SavePopUp.SetActive (false);
		Time.timeScale = 1;
		PlayerDead = true;
		FadeImage.gameObject.SetActive (false);
		FPSPlayer.instance.Die ();
		Invoke ("LevelFail",1.5f);
		//AdManager.instance.showInterstitial();
		//AdManager.instance.ShowUnityAd();
	}
	public void LevelFail()
	{
		Application.LoadLevel ("LevelFailed");
	}
	public void LevelCompleted()
	{
//		Player.SetActive (false);
		MissionCompleteObj.SetActive (true);
		NPCRegistry.instance.HideAllEnemeis ();
		Invoke ("LoadLevelComplete", 6f);
		//AdManager.instance.showInterstitial();
	//	AdManager.instance.ShowUnityAd();
	}
	void LoadLevelComplete()
	{
		SceneManager.LoadScene ("LevelComplete");
		
	}
	public void cellActions()
	{
//		Debug.Log ("CellAction");
		switch (Cell.instance.State) 
		{
		case CellState.Static:
			Cell.instance.StartCharging ();
			break;
		case CellState.Charged:
			Cell.instance.Collect ();	
			break;
		}
		CellActivator.gameObject.SetActive (false);
		CellCollector.gameObject.SetActive (false);

	}
	void DelayedLevelHelp()
	{
        ShowMission(true);
  //      Level1Help.SetActive (true);
		//PauseForAction (true);
	}
	public void ShowLevelHelp(bool show)
	{
        /*
		if (show) 
		{
			Level1Help.SetActive (true);
			PauseForAction (true);
		} else 
		{
			Level1Help.SetActive (false);
			PauseForAction (false);
		}
        */
	}
    public void PopText(string txt)
    {
        StartCoroutine(TextPop(txt, 2.0f));
    }
	public IEnumerator TextPop(string text,float time)
	{
		Text textObj = GameObject.Instantiate (PopUpTextRef, CanvasMain.transform) as Text;
        textObj.text = text;

        textObj.gameObject.SetActive (true);
		float pos = textObj.transform.position.y;
		iTween.MoveTo (textObj.gameObject, iTween.Hash ("y",pos+30,"time",1.5f,"easetype",iTween.EaseType.easeInOutSine));
		yield return new WaitForSeconds (time);
		Destroy (textObj);

	}
	public void Replay()
	{
        Time.timeScale = 1;
      //  AdController.instance.ShowAdmobInterstitial ();
        Loadingpage.SetActive(true);
        Invoke("GotoReply", 1.5f);
        //Application.LoadLevel (Application.loadedLevel);
    }
    void GotoReply()
    {
        Application.LoadLevel(Application.loadedLevel);

    }
	public void Home()
	{
        //Application.LoadLevel ("Menu");
        Time.timeScale = 1;
       // AdController.instance.ShowAdmobInterstitial();
        Loadingpage.SetActive(true);
        
        Invoke("gotoHome", 1.5f);
    }
    void gotoHome()
    {
        Application.LoadLevel("Menu");
    }
    public GameObject Loadingpage;
	public void PauseForAction(bool pause)
	{
        /*
		if (pause) 
		{
			JoyStick.HardRest ();
			Time.timeScale = 0;
			HUD.SetActive (false);

		} else 
		{
			JoyStick.HardRest ();
			Time.timeScale = 1;
			HUD.SetActive (true);
			JoyStick.HardRest ();

		}
        */
	}
	public void PauseGame(bool pause)
	{
		if (pause) 
		{
			JoyStick.HardRest ();
			Time.timeScale = 0;
			HUD.SetActive (false);
         //   bannerAd.instance.showMediumBanner();
			PausePanel.SetActive (true);

		} else 
		{
			JoyStick.HardRest ();
			Time.timeScale = 1;
			HUD.SetActive (true);
			JoyStick.HardRest ();
           // bannerAd.instance.showBanner();
            PausePanel.SetActive (false);
            

        }
			
	}

	public static void ShowToast(string message)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		try
		{
			// Create an Android Java object
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

			// Show a toast message using Android's Toast class
			AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
			AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", currentActivity, message, 0);
			toastObject.Call("show");
		}
		catch
		{ }
#else
            Debug.Log(message);
#endif
	}
}
