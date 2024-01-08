
using UnityEngine;
using System.Collections;

public class RefillAmmo : MonoBehaviour
{
	
	private GameObject weaponObj;
	private WeaponBehavior WeaponBehaviorComponent;
	private PlayerWeapons PlayerWeaponsComponent;
	public AudioClip pickupSound;
	public AudioClip fullSound;

	public bool PlayerIn=false,Active=true;
	public float FillStatus=0;


	void Start () 
	{
		Active = true;
		weaponObj = Camera.main.transform.GetComponent<CameraControl>().weaponObj;
		PlayerWeaponsComponent = weaponObj.GetComponentInChildren<PlayerWeapons>();
		

	}
	public void Focusing()
	{
		if (Active && PlayerIn) 
		{
			FillStatus += Time.deltaTime * 0.3f;

			if (FillStatus >= 1) 
			{
				Refill ();
			}
			GameManager.instance.ammoFRefillStatus (FillStatus);
		}

	}
	public void Refill()
	{
		Debug.Log ("Refill");
		Active = false;
		for (int i = 0; i < PlayerWeaponsComponent.weaponOrder.Length; i++)	
		{
			if (PlayerWeaponsComponent.weaponOrder [i].GetComponent<WeaponBehavior> ().ammo < PlayerWeaponsComponent.weaponOrder [i].GetComponent<WeaponBehavior> ().maxAmmo) {
				PlayerWeaponsComponent.weaponOrder [i].GetComponent<WeaponBehavior> ().ammo = PlayerWeaponsComponent.weaponOrder [i].GetComponent<WeaponBehavior> ().maxAmmo;
			} 
		}
		GameManager.instance.RefillCircle.gameObject.SetActive (false);


	}
	void OnTriggerEnter ( Collider col  )
	{

		if(col.gameObject.tag == "Player"&&Active)
		{
			FillStatus = 0;
			PlayerIn = true;
			GameManager.instance.RefillCircle.gameObject.SetActive (true);

		}
	}
	void OnTriggeExit ( Collider col  )
	{
		if(col.gameObject.tag == "Player")
		{
			FillStatus = 0;
			PlayerIn = false;
			GameManager.instance.RefillCircle.gameObject.SetActive (false);
		}
	}

}