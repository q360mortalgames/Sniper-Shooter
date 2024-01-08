using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour 
{
	public Image[] LevelButtons;
	public Text[] levelTexts;
	public int LevelsUnlocked;

	public Sprite[] lvlButtonSprites;

    public GameObject[] LevelSets, ActiveBtns;
    int CurSet = 0;
    public static LevelSelection instance;
    public GameObject PlayBtn;
    public GameObject LevelSelPanel, ModeSelection, EnvironmentSel, LoadingPage; 
	void Start () 
	{
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LvlSelection, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        //bannerAd.instance.showBanner();
        instance = this;
        if (AudioManager.instance)
		{
		AudioManager.instance.StopMusic (false);
		}
     //   PlayerPrefs.SetInt("LevelsUnlocked", 10);

        if (!PlayerPrefs.HasKey ("LevelsUnlocked")) 
		{
			PlayerPrefs.SetInt ("LevelsUnlocked", 1);
		}
		LevelsUnlocked = PlayerPrefs.GetInt ("LevelsUnlocked");
		//levelTexts=new Text[LevelButtons.Length];
		Debug.Log ("Unlocked"+PlayerPrefs.GetInt ("LevelsUnlocked"));
        CurSet = Mathf.Clamp((PlayerPrefs.GetInt("LevelsUnlocked") - 1) / 5, 0, 7);
        // CurSet = (PlayerPrefs.GetInt("LevelsUnlocked")-1) / 5;
        //for (int i = 0; i < LevelSets.Length; i++)
        //{
        //    LevelSets[i].SetActive(false);
        //}
        //LevelSets[CurSet].SetActive(true);



        //  StartCoroutine("OpenModeSel");  //venkat
        StartCoroutine("OpenLevelSel");
    }

    //IEnumerator OpenModeSel()
    //{
    //    LoadingPage.SetActive(false);
    //    CloseAllpages();
    //    yield return new WaitForSeconds(1);
    //    LoadingPage.SetActive(false);
    //    ModeSelection.SetActive(false);
    //    LevelSelPanel.SetActive(true);
    //}

    void CloseAllpages()
    {
        ModeSelection.SetActive(false);
        EnvironmentSel.SetActive(false);
        LevelSelPanel.SetActive(false);
        LevelSelPanel.SetActive(true);
    }

    // IEnumerator OpenEnvironmentSel()
    //{
    //    LoadingPage.SetActive(false);
    //    CloseAllpages();
    //    yield return new WaitForSeconds(1);
    //    LoadingPage.SetActive(false);
    //    EnvironmentSel.SetActive(false);
        
    //}
    IEnumerator OpenLevelSel()
    {
        LoadingPage.SetActive(true);
        CloseAllpages();
        yield return new WaitForSeconds(1);
        LoadingPage.SetActive(false);
        LevelSelPanel.SetActive(true);

    }
    public void GotoEnvirment()
    {
        StartCoroutine("OpenEnvironmentSel");
    }
    public void GotoModel()
    {
        StartCoroutine("OpenModeSel");
    }

    public void GotoLevelsel()
    {
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.LvlSelection, PlayerPrefs.GetInt("SelectedLevel"));
        //}
        StartCoroutine("OpenLevelSel");

    }
    public void RequestAd()
    {
       // AdManager.instance.RunActions(AdManager.PageType.RequestAd, PlayerPrefs.GetInt("SelectedLevel"));
    }

    void Awake()
    {
        ActiveBtns = GameObject.FindGameObjectsWithTag("ActiveBtn");
        diableAllActive();
    }
    public void diableAllActive()
    {
        for (int i = 0; i < ActiveBtns.Length; i++)
        {
            ActiveBtns[i].SetActive(false);
            
        }

    }
   public void ChangeSet(int inc)
    {
        CurSet = Mathf.Clamp(CurSet + inc, 0, 7);
        for (int i = 0; i < LevelSets.Length; i++)
        {
            LevelSets[i].SetActive(false);
        }
        LevelSets[CurSet].SetActive(true);

        
    }
    public void UnlockLevels()
	{
	}
	
	void Update () 
	{
		
	}
	public void Back()
	{
		Application.LoadLevel ("WeaponSelection");

	}
	public void LoadLevel(int level)
	{
		if (level <= LevelsUnlocked) 
		{
			PlayerPrefs.SetInt ("SelectedLevel", level);
			if (level < 6) {
				Loading.LevelToLoad = "Theme1";
				Application.LoadLevel ("Loading");

			} else {
				Loading.LevelToLoad = "Theme2";
				Application.LoadLevel ("Loading");

			}
		}
	}
    public bool levelBtnPressed;
    int ThemeToLoad1;
    public void GotoLevel()
    {
        if (!levelBtnPressed)
        {
            PlayerPrefs.SetInt("SelectedLevel", PlayerPrefs.GetInt("LevelsUnlocked"));
            ThemeToLoad1 = ActiveBtns[PlayerPrefs.GetInt("LevelsUnlocked") - 1].gameObject.transform.parent.GetComponent<LevelButton>().ThemeToLoad;
            Loading.LevelToLoad = "Theme" + ThemeToLoad1;
        }

        Application.LoadLevel("Loading");
    }

    }
