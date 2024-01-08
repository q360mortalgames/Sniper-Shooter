using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchControlsKit;
public class WeaponSelection : MonoBehaviour
{

    public Transform WeaponParent;
    public float rotateSpeed = 10;

    public int pIndex = 1, sIndex = 1, mIndex = 1;
    public GameObject[] pWeapons, sWeapons, mWeapons, pSpecs, sSpecs, mSpecs;

    public int ActiveUpdate = 1;
    public GameObject Lock;
    public Text Price, ButtonText;


    public int[] PrimaryPrices, SecPrices, MeleePrices;
    public int pUnlocked, sUnlocked, mUnlocked;
    public Text MoneyText;

    public GameObject[] TypeButtons;
    public float typPosX;
    public static WeaponSelection instance;
    public GameObject vipGuntext;
    public GameObject Backbtn;
    void Start()
    {
        instance = this;
        Time.timeScale= 1;
        //bannerAd.instance.showBanner();
        //vipGuntext.SetActive(false);
        //iTween.MoveFrom(Backbtn, iTween.Hash("y", Backbtn.transform.position.y + 300, "time", 0.5f, "delay", 0.3, "easetype", iTween.EaseType.spring));
        //if (AdManager.instance)
        //{
        //    AdManager.instance.RunActions(AdManager.PageType.RequestAd);
        //}
        //      if (AudioManager.instance) {
        //	AudioManager.instance.StopMusic (false);
        //}
        //		PlayerPrefs.DeleteAll ();
        //PlayerPrefs.SetInt("TotalMoney", 100000);

        if (!PlayerPrefs.HasKey("SelWeapon1"))
        {
            PlayerPrefs.SetInt("SelWeapon1", 1);
        }
        if (!PlayerPrefs.HasKey("SelWeapon2"))
        {
            PlayerPrefs.SetInt("SelWeapon2", 1);
        }
        if (!PlayerPrefs.HasKey("SelWeapon3"))
        {
            PlayerPrefs.SetInt("SelWeapon3", 1);
        }

        PlayerPrefs.SetString("PWepStatus1", "Unlocked");
        PlayerPrefs.SetString("SWepStatus1", "Unlocked");
        PlayerPrefs.SetString("MWepStatus1", "Unlocked");


        Debug.Log(PlayerPrefs.GetString("PWepStatus2"));

        HideAllWeaponsAndSpecs();
        //updatePWeapon(false, false);
        typPosX = TypeButtons[0].transform.position.x;
        SelUpdateType(1);

        pIndex = PlayerPrefs.GetInt("SelWeapon1");
        updatePWeapon(true, false);

    }
    public GameObject Selectbtn, NextPlaybtn;
    public Text SelectText;
    public void Select()
    {
        Color temp = Selectbtn.GetComponent<Image>().color;
        temp.a = 0.5f;
        Selectbtn.GetComponent<Image>().color= temp;
        SelectText.text = "SELECTED";
        NextPlaybtn.SetActive(true);

    }
    

    public void Continue()
    {
        Debug.Log("pIndex" + pIndex);
        if (PlayerPrefs.GetString("PWepStatus" + pIndex) == "Unlocked")
        {
            PlayerPrefs.SetInt("SelWeapon1", pIndex);
            Application.LoadLevelAsync("LevelSelection");

        }
        else
        {
           // AdManager.instance.BuyItem(1, true);

        }
    }
    public void UnLockAllGuns(bool unlock)
    {
        if (unlock)
        {
            for (int i = 0; i < pWeapons.Length; i++)
            {
                PlayerPrefs.SetString("PWepStatus" + (i + 1), "Unlocked");
            }
            for (int i = 0; i < sWeapons.Length; i++)
            {
                PlayerPrefs.SetString("SWepStatus" + (i + 1), "Unlocked");
            }
            for (int i = 0; i < mWeapons.Length; i++)
            {
                PlayerPrefs.SetString("MWepStatus" + (i + 1), "Unlocked");

            }
        }
        else
        {
            for (int i = 1; i < pWeapons.Length; i++)
            {
                PlayerPrefs.SetString("PWepStatus" + (i + 1), "Locked");
            }
            for (int i = 1; i < sWeapons.Length; i++)
            {
                PlayerPrefs.SetString("SWepStatus" + (i + 1), "Locked");
            }
            for (int i = 1; i < mWeapons.Length; i++)
            {
                PlayerPrefs.SetString("MWepStatus" + (i + 1), "Locked"
                );
            }
        }
        updatePWeapon(true, false);
        updateSWeapon(true, false);
        updateMWeapon(true, false);


    }
    public GameObject StoreObj;
    public void ShowStore()
    {
        //StoreObj.SetActive (true);
        Menu.ShowStoreFirst = true;
        Application.LoadLevel("Menu");
    }
    void Update()
    {

        WeaponParent.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
        //if (TCKInput.GetAxis ("GunSwipe", "Horizontal") !=0) 
        //{
        //	rotateSpeed =  -TCKInput.GetAxis ("GunSwipe", "Horizontal") * 50;
        //	rotateSpeed = Mathf.Clamp (rotateSpeed, -500,500);
        //}
        MoneyText.text = PlayerPrefs.GetInt("TotalMoney").ToString();


    }
    public void BuyGuns()
    {
    }
    public void SelUpdateType(int type)
    {
        ActiveUpdate = type;
        switch (type)
        {
            case 1:
                updatePWeapon(true, false);
                break;
            case 2:
                updateSWeapon(true, false);
                break;
            case 3:
                updateMWeapon(true, false);
                break;
        }
        for (int i = 0; i < 3; i++)
        {
            Vector3 temp = TypeButtons[i].transform.position;
            temp.x = typPosX;
            TypeButtons[i].transform.position = temp;
            TypeButtons[i].GetComponent<Shadow>().enabled = false;

        }
        iTween.MoveTo(TypeButtons[type - 1], iTween.Hash("x", typPosX + 5, "time", 0.2f, "easetype", iTween.EaseType.easeInCubic));
        TypeButtons[type - 1].GetComponent<Shadow>().enabled = true;
    }
    public void next()
    {
        switch (ActiveUpdate)
        {
            case 1:
                updatePWeapon(false, true);
                break;
            case 2:
                updateSWeapon(false, true);
                break;
            case 3:
                updateMWeapon(false, true);
                break;
        }
    }
    public void prev()
    {
        switch (ActiveUpdate)
        {
            case 1:
                updatePWeapon(false, false);
                break;
            case 2:
                updateSWeapon(false, false);
                break;
            case 3:
                updateMWeapon(false, false);
                break;

        }
    }
    public void GotoMenu()
    {
        Application.LoadLevel("Menu");

    }
    public GameObject nextBtn, prevBtn;
    public void updatePWeapon(bool updateonly, bool inc)
    {
        //updateonly just updates weapon without changing index
        if (!updateonly)
        {
            //NextPlaybtn.SetActive(false);
            if (inc)
            {
                pIndex = Mathf.Clamp(pIndex + 1, 1, pWeapons.Length);
            }
            else
            {
                pIndex = Mathf.Clamp(pIndex - 1, 1, pWeapons.Length);

            }

        }

        

        if (pIndex == pWeapons.Length)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }
        if (pIndex == 1)
        {
            prevBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
        }
        HideAllWeaponsAndSpecs();
        pWeapons[pIndex - 1].SetActive(true);
        pSpecs[pIndex - 1].SetActive(true);

        if (PlayerPrefs.GetString("PWepStatus" + pIndex) == "Unlocked")
        {
            Lock.SetActive(false);
            if (pIndex != PlayerPrefs.GetInt("SelWeapon1"))
                ButtonText.text = "Select";
            BuyBtn.SetActive(false);
            //Selectbtn.SetActive(true);
            //if (!updateonly)
            //{ 
            //Color temp = Selectbtn.GetComponent<Image>().color;
            //temp.a = 1f;
            //Selectbtn.GetComponent<Image>().color = temp;
            //SelectText.text = "SELECT";
            //    }

        }
        else
        {
            Lock.SetActive(true);
            Price.text = PrimaryPrices[pIndex - 1].ToString();
            ButtonText.text = "Buy";
            BuyBtn.SetActive(true);
            //Selectbtn.SetActive(false);

        }
        if (pIndex == 5 || pIndex == 7 || pIndex == 8 || pIndex == 10 || pIndex == 13)
        {
            if (PlayerPrefs.GetString("VIP") != "true")
            {
                //vipGuntext.SetActive(true);
                ButtonText.text = "";
                BuyBtn.SetActive(false);
                //vipBtn.SetActive(true);
            }
            else
            {
                //vipGuntext.SetActive(false);
                //vipBtn.SetActive(false);
                PlayerPrefs.SetString("PWepStatus" + pIndex, "Unlocked");
            }

        }
        else
        {
            //vipGuntext.SetActive(false);
            //vipBtn.SetActive(false);
        }

        if (pIndex == PlayerPrefs.GetInt("SelWeapon1"))
        {
            ButtonText.text = "Selected";
            //			Debug.Log("Selected");
        }
    }
    public GameObject BuyBtn, vipBtn;
    public void updateSWeapon(bool updateonly, bool inc)
    {
        if (!updateonly)
        {
            if (inc)
            {
                sIndex = Mathf.Clamp(sIndex + 1, 1, sWeapons.Length);
            }
            else
            {
                sIndex = Mathf.Clamp(sIndex - 1, 1, sWeapons.Length);

            }

        }
        if (sIndex == sWeapons.Length)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }
        if (sIndex == 1)
        {
            prevBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
        }
        HideAllWeaponsAndSpecs();
        sWeapons[sIndex - 1].SetActive(true);
        sSpecs[sIndex - 1].SetActive(true);

        if (PlayerPrefs.GetString("SWepStatus" + sIndex) == "Unlocked")
        {
            Lock.SetActive(false);
            ButtonText.text = "Select";
        }
        else
        {
            Lock.SetActive(true);
            Price.text = SecPrices[sIndex - 1].ToString();
            ButtonText.text = "Buy";
        }
        if (sIndex == PlayerPrefs.GetInt("SelWeapon2"))
        {
            ButtonText.text = "Selected";
        }
    }
    public void updateMWeapon(bool updateonly, bool inc)
    {
        if (!updateonly)
        {
            if (inc)
            {
                mIndex = Mathf.Clamp(mIndex + 1, 1, mWeapons.Length);
            }
            else
            {
                mIndex = Mathf.Clamp(mIndex - 1, 1, mWeapons.Length);

            }

        }
        if (mIndex == pWeapons.Length)
        {
            nextBtn.SetActive(false);
        }
        else
        {
            nextBtn.SetActive(true);
        }
        if (mIndex == 1)
        {
            prevBtn.SetActive(false);
        }
        else
        {
            prevBtn.SetActive(true);
        }
        HideAllWeaponsAndSpecs();
        mWeapons[mIndex - 1].SetActive(true);
        mSpecs[mIndex - 1].SetActive(true);

        if (PlayerPrefs.GetString("MWepStatus" + mIndex) == "Unlocked")
        {
            Lock.SetActive(false);
            ButtonText.text = "Select";
        }
        else
        {
            Lock.SetActive(true);
            Price.text = MeleePrices[pIndex - 1].ToString();
            ButtonText.text = "Buy";
        }
        if (mIndex == PlayerPrefs.GetInt("SelWeapon3"))
        {
            ButtonText.text = "Selected";
        }
    }
    public void HideAllWeaponsAndSpecs()
    {
        for (int i = 0; i < pWeapons.Length; i++)
        {
            pWeapons[i].SetActive(false);
            pSpecs[i].SetActive(false);
        }
        for (int i = 0; i < sWeapons.Length; i++)
        {
            sWeapons[i].SetActive(false);
            sSpecs[i].SetActive(false);
        }
        for (int i = 0; i < mWeapons.Length; i++)
        {
            mWeapons[i].SetActive(false);
            mSpecs[i].SetActive(false);

        }
    }

    public void BuyButtonAction()
    {
        switch (ActiveUpdate)
        {
            case 1:
                if (ButtonText.text == "Select")
                {
                    PlayerPrefs.SetInt("SelWeapon1", pIndex);
                    ButtonText.text = "Selected";
                }
                else if (ButtonText.text == "Buy")
                {
                    Debug.Log("Buy1" + PlayerPrefs.GetInt("TotalMoney"));
                    if (PlayerPrefs.GetInt("TotalMoney") >= PrimaryPrices[pIndex - 1])
                    {
                        Debug.Log("Buy2");
                        PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney") - PrimaryPrices[pIndex - 1]);
                        PlayerPrefs.SetString("PWepStatus" + pIndex, "Unlocked");

                        PlayerPrefs.SetInt("SelWeapon1", pIndex);
                        updatePWeapon(true, false);
                        ButtonText.text = "Selected";
                        Debug.Log(PlayerPrefs.GetString("PWepStatus2"));
                        PlayerPrefs.SetString("PWepStatus" + pIndex, "Unlocked");
                    }
                    else
                    {
                        ShowStore();
                    }
                   
                        
                }
                break;
            case 2:
                if (ButtonText.text == "Select")
                {
                    PlayerPrefs.SetInt("SelWeapon2", sIndex);
                    ButtonText.text = "Selected";
                }
                else if (ButtonText.text == "Buy")
                {
                    if (PlayerPrefs.GetInt("TotalMoney") >= PrimaryPrices[sIndex - 1])
                    {
                        PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney") - PrimaryPrices[sIndex - 1]);
                        PlayerPrefs.SetString("SWepStatus" + sIndex, "Unlocked");
                        PlayerPrefs.SetInt("SelWeapon2", sIndex);
                        updateSWeapon(true, false);
                        ButtonText.text = "Selected";
                        
                    }
                }
                break;
            case 3:
                if (ButtonText.text == "Select")
                {
                    PlayerPrefs.SetInt("SelWeapon3", mIndex);
                    ButtonText.text = "Selected";
                }
                else if (ButtonText.text == "Buy")
                {
                    if (PlayerPrefs.GetInt("TotalMoney") > PrimaryPrices[mIndex - 1])
                    {
                        PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney") - PrimaryPrices[mIndex - 1]);

                        PlayerPrefs.SetString("MWepStatus" + mIndex, "Unlocked");
                        PlayerPrefs.SetInt("SelWeapon3", mIndex);
                        updateMWeapon(true, false);
                        ButtonText.text = "Selected";
                    }
                }
                break;
        }
    }
}
