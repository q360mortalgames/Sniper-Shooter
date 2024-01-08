using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LevelButton : MonoBehaviour,IPointerClickHandler {
	public Text LevelText;
    [HideInInspector]
	public int LevelNum;
	//public GameObject Lock;

    public int ThemeToLoad;
    public GameObject ActiveBtn, unlockBtn;
	void Start () 
	{
        //PlayerPrefs.SetInt("LevelsUnlocked", 5);


        if (!PlayerPrefs.HasKey ("LevelsUnlocked"))
		{
			PlayerPrefs.SetInt ("LevelsUnlocked", 1);

		}
		string Levelstr=transform.name.Substring (7, 2);
		if (Levelstr.Contains (")")) {
			Levelstr= Levelstr.Remove (1);
		}
		LevelNum = int.Parse(Levelstr);
        //LevelText.text ="Mission "+LevelNum.ToString ();
        LevelText.text = "" + LevelNum.ToString();
        if (LevelNum < PlayerPrefs.GetInt("LevelsUnlocked"))
        {
            //Lock.SetActive (false);
            unlockBtn.SetActive(true);

        } else if (LevelNum == PlayerPrefs.GetInt("LevelsUnlocked"))
        {
            unlockBtn.SetActive(true);
            ActiveBtn.SetActive(true);
        }
        print("unlockedlevels" + PlayerPrefs.GetInt("LevelsUnlocked"));

	}
	
	
	public void OnPointerClick(PointerEventData eventData)
	{
        LevelSelection.instance.diableAllActive();
        //ActiveBtn.SetActive(true);
        //LevelSelection.instance.levelBtnPressed = true;
        if (LevelNum <= PlayerPrefs.GetInt("LevelsUnlocked"))
        {
            ActiveBtn.SetActive(true);
            PlayerPrefs.SetInt("SelectedLevel", LevelNum);
           
            Loading.LevelToLoad = "Theme"+ThemeToLoad;
            //LevelSelection.instance.PlayBtn.gameObject.SetActive(true);
            //unlockBtn.SetActive(true);
            Application.LoadLevel("Loading");
            PlayerPrefs.SetInt("SelectedTheme", ThemeToLoad);

            //else
            //{
            //    Loading.LevelToLoad = "Theme2";
            //    Application.LoadLevel("Loading");

            //}
        }
        else
        {
            //LevelSelection.instance.PlayBtn.gameObject.SetActive(false);
            //AdManager.instance.BuyItem(2, true);
        }
	}
}
