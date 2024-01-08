using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store : MonoBehaviour {

    public GameObject Menupanel;
    public Text cashText;
	// Use this for initialization
	void Start ()
    {
        cashText.text = "" + PlayerPrefs.GetInt("TotalMoney");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Close()
	{
		
		if (AudioManager.instance) {
			AudioManager.instance.PlaySound (3);
		}
        Time.timeScale = 1;
        Menupanel.SetActive(true);
        gameObject.SetActive(false);

    }
	public void BuyProduct(int product)
	{
	}
    public void VideoForCoins()
    {
        /*AdManager.instance.ShowRewardVideoWithCallback((result) =>  //venkat
        {
            PlayerPrefs.SetInt ("TotalMoney", PlayerPrefs.GetInt("TotalMoney")+500);

        });*/  //venkat
    }
}
