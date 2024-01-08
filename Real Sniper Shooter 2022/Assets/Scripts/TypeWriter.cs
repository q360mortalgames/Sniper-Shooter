using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TypeWriter : MonoBehaviour 
{
	
	public float letterPause = 0.2f;
	public AudioClip sound;
	public Text TextMsg;
	string message;
	public string[] AllMessages;
	public int totalMsgs,msgCount=0;
	public GameObject OkBtn;

	public bool autoClose;
	public bool callName;
	public GameObject HelpRoot;
	public GameObject Tint;
	public GameObject FingerHelp1,FingerHelp2;

	void Start () 
	{
		Tint.SetActive (true);
		Tint.SetActive (true);
		iTween.ScaleFrom(gameObject,iTween.Hash("x",0,"y",0,"time",1));
		iTween.FadeFrom(Tint,iTween.Hash("alpha",0,"time",5));

		totalMsgs = AllMessages.Length;
		if (AllMessages.Length > 0) 
		{
			if(callName)
			{
				AllMessages[0]+=PlayerPrefs.GetString("PlayerName");
			}
			message = AllMessages [0];
			Invoke ("StartTyping", 0.6f);
		}
	}
	public void StartTyping()
	{
		StartCoroutine(TypeText ());
		
	}
	IEnumerator TypeText () 
	{
		foreach (char letter in message.ToCharArray()) {
			TextMsg.text += letter;
			//			if (sound)
			//				audio.PlayOneShot (sound);
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}
		msgCount++;
		yield return new WaitForSeconds (0.1f);
		if (msgCount < totalMsgs) {
			yield return new WaitForSeconds (1f);
			TextMsg.text = "";
			message = AllMessages [msgCount];
			StartCoroutine (TypeText ());
			
		} else 
		{
			if (autoClose) 
			{
//				gameObject.SetActive(false);
//				GameManager.instance.gameState=GameState.Active;
				CloseHelp();
			}
			else
			{
				if(OkBtn)
				OkBtn.SetActive (true);
			}

		}

	}
	public void CloseHelp()
	{
		gameObject.SetActive(false);
		Tint.SetActive (false);

	}
}