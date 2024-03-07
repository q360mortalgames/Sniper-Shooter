using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store : MonoBehaviour {

    public GameObject Menupanel;
    public Text cashText;

    private void OnEnable()
    {
        // Subscribe to the event when the script is enabled
        IAPPurchaseComplete.OnEventTriggered += HandleEvent;
    }

    private void HandleEvent(object sender, EventArgs e)
    {
        cashText.text = "" + PlayerPrefs.GetInt("TotalMoney");
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the script is disabled
        IAPPurchaseComplete.OnEventTriggered -= HandleEvent;
    }

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
