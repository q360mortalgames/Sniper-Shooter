using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour 
{

	public Image SoundButton,MusicButton;
	public Sprite[] SoundSprites, MusicSprites;
	public static bool SoundOn = true;
    

    public Image Low, High;
	void Start () 
	{
		if (!SoundOn) 
		{
			SoundButton.sprite = SoundSprites [1];
		}
	}
	

	void Update () 
	{
		
	}
	public void Close()
	{
		
		AudioManager.instance.PlaySound (3);
        Menu.instace.ShowMenu();

        gameObject.SetActive (false);

	}
	public void ToggleSound()
	{
		Debug.Log ("Toggle");
		if (SoundOn)
		{
			SoundOn = false;
			SoundButton.sprite = SoundSprites [1];
			AudioListener.volume = 0;
		} 
		else 
		{
			SoundOn = true;
			SoundButton.sprite = SoundSprites [0];
			AudioListener.volume = 1;
		   
		}
	}
    bool MusicOn = true;
    public void ToggleMusic()
    {
        Debug.Log("Toggle");
        if (MusicOn)
        {
            MusicOn = false;
            MusicButton.sprite = MusicSprites[1];
            AudioListener.volume = 0;

        }
        else
        {
            MusicOn = true;
            MusicButton.sprite = MusicSprites[0];
            AudioListener.volume = 1;

        }
    }
    public void Loww()
    {
        High.color = Color.gray;
        Low.color = Color.white;
    }
    public void Highh()
    {
        Low.color = Color.gray;
        High.color = Color.white;

    }
    public void Privacy()
    {
        Application.OpenURL("");
    }
}
