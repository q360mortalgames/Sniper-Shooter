using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPPurchaseComplete : MonoBehaviour
{
	public static event EventHandler OnEventTriggered;
	public void TriggerEvent()
	{
		OnEventTriggered?.Invoke(this, EventArgs.Empty);
	}
	public void OnPurchaseComplete(Product product)
	{
		Debug.Log($"OnPurchaseComplete: {product.definition.id}");

		switch (product.definition.id)
		{
			case "com.eurotrain.noads":
				AdsManager.Instance.OnPurchasedRemoveAds();
				break;

			case "unlock_guns":
				UnlockAllGuns();
				GameManager.ShowToast("Unlocked All Guns");
				break;

			case "unlock_levels":
				PlayerPrefs.SetInt("LevelsUnlocked", 24);
				GameManager.ShowToast("Unlocked All Levels");
				break;

			case "unlock_levels_guns":
				UnlockAllGuns();
				PlayerPrefs.SetInt("LevelsUnlocked", 24);
				GameManager.ShowToast("Unlocked All Guns and Levels");
				break;

			case "coins_100000":
				PlayerPrefs.SetInt("TotalMoney", PlayerPrefs.GetInt("TotalMoney") + 100000);
				GameManager.ShowToast("+100000");
				break;

			default:
				break;
		}
		TriggerEvent();
		Time.timeScale = 1;
	}

	void UnlockAllGuns()
    {
		for (int i = 0; i < 10; i++)
		{
			PlayerPrefs.SetString("PWepStatus" + (i + 1), "Unlocked");
		}
	}
}
