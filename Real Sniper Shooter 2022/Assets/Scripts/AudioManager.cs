using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public AudioSource[] Sources;//Ambience,BG,Sounds
	public static AudioManager instance;
	public AudioClip[] BGClips,SoundClips;
	void Awake () 
	{
		if (!instance)
			instance = this;
		else
			Destroy (this.gameObject);

		DontDestroyOnLoad (this.gameObject);
		Sources = GetComponents<AudioSource> ();
	}
	
	void Update () 
	{
		
	}
	public void StopMusic(bool mute)
	{
		Sources [1].mute = mute;
	}
	public void PlaySound(int sound)
	{
		Sources [2].Stop ();
		Sources [2].clip = SoundClips [sound];
		Sources [2].Play ();
	}
}
