using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Cell : MonoBehaviour
{
	public CellState State;
	public GameObject ParticleActive,ParticleCharging;
	public GameObject CellObj;
	public TextMesh TimerText;
	public static Cell instance;

	public AudioSource ASource;

	public AudioClip ChargeClip,ChargeLoop,ReadyClip,CollectClip;
	bool collected;
	public float ChargeTime=120,timeleft;

	public GameObject CellRegionNPCs;
	void Start () 
	{
		instance = this;	
		ParticleCharging.SetActive (false);
		ParticleActive.SetActive (false);
		timeleft = ChargeTime;
		ASource = GetComponent<AudioSource> ();
	}
	
	void Update () 
	{
		if (State == CellState.Activated)
		{
			CellObj.transform.RotateAround (CellObj.transform.position, Vector3.up, Time.deltaTime * 20);
			if (!ASource.isPlaying)
			{
				ASource.clip = ChargeLoop;
				ASource.Play ();
				ASource.loop = true;
			}
			timeleft -= Time.deltaTime;
			if (timeleft <= 0) 
			{
				timeleft = 0;
				CellReady ();
			}
		}
		if (State==CellState.Activated) 
		{
			Vector3 Pos = CellObj.transform.position;
			Pos.y = CellObj.transform.position.y + 0.006f * Mathf.Sin (2f*Time.time);
			CellObj.transform.position = Pos;
			CellObj.transform.RotateAround (CellObj.transform.position, Vector3.up, Time.deltaTime * 20);

//			GameManager.instance.TextPop("Cell ACTIVATED, collect

		}
//		if (State == CellState.Charging) 
//		{
//			timeleft -= Time.deltaTime;
//			if (timeleft <= 0) 
//			{
//				timeleft = 0;
//				Activate ();
//			}
//		}
	}
	public void StartCharging()
	{
		GameManager.instance.PauseForAction (false);
		ASource.clip = ChargeClip;
		ASource.Play ();
//		Debug.Log ("startCharging");
		ParticleCharging.SetActive (true);
		State = CellState.Activated;
		DirectionManager.instance.ShowArrow (false);
		GameManager.instance.chargingHUD.SetActive (true);

		if(CellRegionNPCs)
			CellRegionNPCs.SetActive (true);

	}
	public void Collect()
	{
		GameManager.instance.PauseForAction (false);

		Debug.Log ("CollectCell");
		GetComponent<Collider> ().enabled = false;
		State = CellState.Collected;
		collected = true;
		CellObj.SetActive (false);
		ASource.clip = CollectClip;
		ASource.Play ();
		ASource.loop = false;
		for (int i=0;i<NPCRegistry.instance.Npcs.Count;i++) 
		{
			NPCRegistry.instance.Npcs[i].huntPlayer=false;
		}
		CellRegionNPCs.SetActive (false);
//		SceneManager.LoadScene ("LevelComplete");
		GameManager.instance.LevelCompleted();

	}
	public void CellReady()
	{
		Debug.Log ("ActivateCell");
		ASource.Stop ();
		AudioSource.PlayClipAtPoint (ReadyClip, Camera.main.transform.position);
		DirectionManager.instance.ShowArrow (true);

		State = CellState.Charged;
		ParticleCharging.SetActive (false);
		ParticleActive.SetActive (true);
		GameManager.instance.chargingHUD.SetActive (false);
		GameManager.instance.CollectText.SetActive (true);

	}
	void OnTriggerEnter ( Collider col  )
	{
		if (col.gameObject.tag == "Player" )
		{
			if (State == CellState.Static) {
				GameManager.instance.CellActivator.gameObject.SetActive (true);
			}

		}
	}
	void OnTriggerStay( Collider col  )
	{
		if (col.gameObject.tag == "Player") 
		{
			if (State == CellState.Static)
			{
				GameManager.instance.CellActivator.gameObject.SetActive (true);
				GameManager.instance.PauseForAction (true);
			}
			else
			{
				DirectionManager.instance.ShowArrow (false);
			}
			if (!collected&&State==CellState.Charged)
			{
				GameManager.instance.CollectText.SetActive (false);
				GameManager.instance.CellCollector.gameObject.SetActive (true);
				GameManager.instance.PauseForAction (true);

			}

		}
	}
	void OnTriggerExit ( Collider col  )
	{
		if (col.gameObject.tag == "Player") 
		{
			if (State != CellState.Activated)
			{
				DirectionManager.instance.ShowArrow (true);
			}
		}
	}
}
