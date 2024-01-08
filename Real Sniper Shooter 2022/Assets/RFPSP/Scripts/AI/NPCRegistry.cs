﻿//NPCRegistry.cs by Azuline Studios© All Rights Reserved
//Manages registry of all NPCs in scene and assigns targets,
//taking NPC faction alignments into account.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCRegistry : MonoBehaviour
{
	private FPSRigidBodyWalker FPSWalker;
	[Tooltip("List containing references to all NPCs' AI.cs components, for use in other scripts.")]
	public List<AI> Npcs = new List<AI>();
	private GameObject playerObj;
	private Transform playerTransform;
	private float nearestNpcDist;
	private float NpcDist = 0.0f;
	private float playerDist = 0.0f;
	private float playerDistMod;//reduce player search distance when player is crouching or prone
	private RaycastHit hit;
	public List<GameObject> indicators = new List<GameObject> ();
	int totalNPCs;

	public static NPCRegistry instance; 
	void Start () 
	{
		instance = this;
		playerObj = Camera.main.transform.GetComponent<CameraControl>().playerObj;
		FPSWalker = playerObj.GetComponent<FPSRigidBodyWalker>();
		playerTransform = playerObj.transform;
	}
	public float LookAtAngle(Vector3 fromPosition, Vector3 fromForward, Vector3 toPosition)
	{

		return (Vector3.Cross(fromForward, (toPosition - fromPosition).normalized).y < 0.0f) ?
			-Vector3.Angle(fromForward, (toPosition - fromPosition).normalized) :
			Vector3.Angle(fromForward, (toPosition - fromPosition).normalized);

	}
	public  Vector3 HorizontalVector(Vector3 value)
	{

		value.y = 0.0f;
		return value;

	}
	public void Update()
	{

		for (int i = 0; i < Npcs.Count; i++) 
		{


            /*

				if(Vector3.Distance(Npcs[i].transform.position, playerTransform.position)<25)
				{
					indicators[i].SetActive(true);
					float angle=LookAtAngle(HorizontalVector(playerTransform.position),HorizontalVector(playerTransform.forward),HorizontalVector(Npcs[i].transform.position));
					indicators[i].SetActive(true);
					indicators [i].transform.rotation=Quaternion.Euler (0, 0, -angle);
			
				}
				else
				{
					indicators[i].SetActive(false);

				}
                */
		}
	}
	public void RegisterIndicator()
	{
		GameObject newInd = Instantiate (GameManager.instance.EnemyIndicator,GameManager.instance.CanvasMain.transform);
		newInd.transform.position = GameManager.instance.CanvasMain.transform.position;
		indicators.Add (newInd);
	}
	//Remove an NPC from the NPC registry
	public void UnregisterNPC(AI NpcAI){
		for(int i = 0; i < Npcs.Count; i++)
		{
			if(Npcs[i] == NpcAI)
			{
				Npcs.RemoveAt(i);
				Destroy (indicators [i],0);
				indicators.RemoveAt (i);
			}
		}
	}
	
	public void MoveFolowingNpcs(Vector3 position){
		bool moveFxPlayed = false;
		for(int i = 0; i < Npcs.Count; i++){
			if(!Npcs[i].leadPlayer && Npcs[i].factionNum == 1 && (Npcs[i].followPlayer || Npcs[i].orderedMove)){
				Npcs[i].GoToPosition(position, true);
				Npcs[i].followPlayer = false;
				if(!moveFxPlayed && (Npcs[i].moveToFx1 || Npcs[i].moveToFx2)){
					if(Random.value > 0.5f){
						Npcs[i].vocalFx.clip = Npcs[i].moveToFx1;
					}else{
						Npcs[i].vocalFx.clip = Npcs[i].moveToFx2;
					}
					Npcs[i].vocalFx.pitch = Random.Range(0.94f, 1f);
					Npcs[i].vocalFx.spatialBlend = 0f;
					Npcs[i].vocalFx.PlayOneShot(Npcs[i].vocalFx.clip);
					moveFxPlayed = true;
				}
			}
		}
	}

	public void HideAllEnemeis()
	{
		for (int i = 0; i < Npcs.Count; i++) 
		{
			Npcs [i].gameObject.SetActive (false);
		}
	}
	//Find the closest hostile target for this NPC, taking into account faction alignments and player stance
	public void FindClosestTarget(GameObject NPC, AI NpcAIcomponent, Vector3 myPos, float distance, int myFaction){

		nearestNpcDist = distance;
		AI nearestNpcAIcomponent = null;

		playerDist = Vector3.Distance(myPos, playerTransform.position);

		//calculate range based on player stance/sneaking
		if(!NpcAIcomponent.heardPlayer){
			if(FPSWalker.crouched){
				playerDistMod = distance * NpcAIcomponent.sneakRangeMod;//reduce NPC's attack range by sneakRangeMod amount when player is crouched
			}else if(FPSWalker.prone){
				playerDistMod = distance * (NpcAIcomponent.sneakRangeMod * 0.75f);//reduce NPC's attack range further when player is prone
			}else{
				playerDistMod = distance;
			}
		}else{
			playerDistMod = distance;
		}

		for(int i = 0; i < Npcs.Count; i++){

			NpcDist = Vector3.Distance(myPos, Npcs[i].myTransform.position);

			if(NpcDist < distance && NpcDist < nearestNpcDist && NpcAIcomponent != Npcs[i]){
				//check if potential target's faction is one that this NPC is hostile to
				if(
					   (myFaction == 1 && Npcs[i].factionNum == 2)
				    || (myFaction == 1 && Npcs[i].factionNum == 3)
				    || (myFaction == 2 && Npcs[i].factionNum == 1)
				    || (myFaction == 2 && Npcs[i].factionNum == 3)
				    || (myFaction == 3 && Npcs[i].factionNum != 3)
				){
					nearestNpcDist = NpcDist;
					nearestNpcAIcomponent = Npcs[i];
				}
			}
		}
		//determine if player is closer than the nearest hostile NPC
		if((myFaction != 1
		&& playerDist < playerDistMod 
		&& playerDist < nearestNpcDist)
		|| NpcAIcomponent.huntPlayer){
			//set NPC target to player
			NpcAIcomponent.target = playerTransform;
			NpcAIcomponent.targetEyeHeight = FPSWalker.capsule.height * 0.25f;
			NpcAIcomponent.TargetAIComponent = null;

		}else{
			//set NPC target to nearest Hostile NPC
			if(nearestNpcAIcomponent){
				NpcAIcomponent.TargetAIComponent = nearestNpcAIcomponent;
				NpcAIcomponent.targetEyeHeight = nearestNpcAIcomponent.eyeHeight;
				NpcAIcomponent.target = nearestNpcAIcomponent.myTransform;
				NpcAIcomponent.lastVisibleTargetPosition = nearestNpcAIcomponent.myTransform.position + (nearestNpcAIcomponent.myTransform.up * nearestNpcAIcomponent.eyeHeight); 
			}

		}

	}

}