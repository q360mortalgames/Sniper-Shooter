using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour {
   

	public int TargetKills=1;
	public GameObject[] FirstShotAlerts;

	public GameObject[] FirstShotAttackers;
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
    public void FirstShot()
    {
        for(int i=0;i<FirstShotAlerts.Length;i++)
        {
            FirstShotAlerts[i].GetComponent<HumanController>().Run();
			
        }
        for (int i = 0; i < FirstShotAttackers.Length; i++)
        {
            
            FirstShotAttackers[i].GetComponent<AI>().StartShooting();
        }
    }
}
