using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletscript : MonoBehaviour
{
    public GameObject Bullet, bulletCamera, shell0, shell1, shell2;
    float distance;
    Vector3 CamLastpos, lookDir;
    float speed = 0.01f;
    public AudioClip deathsound;
  public Vector3 Pos1, Pos2, Pos3, Rotate1, Rotate2, Rotate3;
    // Start is called before the first frame update
    void Start()
    {
        
        distance = Vector3.Distance(this.transform.position, FPSPlayer.instance.WeaponBehaviorComponent.LasttargethitPoint);
        //StartCoroutine("Slowbullet");
        lookDir = new Vector3(25, bulletCamera.transform.localEulerAngles.y-15, bulletCamera.transform.localEulerAngles.z);
        //CamLastpos = new Vector3(bulletCamera.transform.localPosition.x, bulletCamera.transform.localPosition.y+2, bulletCamera.transform.localPosition.z-4);
        CamLastpos = new Vector3(Pos3.x, Pos3.y+0.8f, Pos3.z-2);
        StartCoroutine("UnparentShell");
    }
  public bool Place1=true, Place2, Place3;
    IEnumerator UnparentShell()
    {
        shell0.transform.parent = null;
        yield return new WaitForSeconds(0.7f);
        Place1 = false;
        Place2 = true;
        yield return new WaitForSeconds(0.2f);
        shell1.transform.GetChild(0).gameObject.SetActive(true);
        shell1.transform.parent = null;
        yield return new WaitForSeconds(0.5f);
        shell2.transform.GetChild(0).gameObject.SetActive(true);
        shell2.transform.parent = null;
        yield return new WaitForSeconds(0.5f);
        Place2 = false;
        Place3 = true;
        speed = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        

        this.transform.position = Vector3.MoveTowards(this.transform.position, FPSPlayer.instance.WeaponBehaviorComponent.LasttargethitPoint, Time.deltaTime *speed* distance);
        if (Place2)
        {
            bulletCamera.transform.localEulerAngles = Vector3.Lerp(bulletCamera.transform.localEulerAngles, Rotate2, Time.deltaTime * 8);
            bulletCamera.transform.localPosition = Vector3.Lerp(bulletCamera.transform.localPosition, Pos2, Time.deltaTime * 8);
        }
        else if (Place3)
        {
            bulletCamera.transform.localEulerAngles = Vector3.Lerp(bulletCamera.transform.localEulerAngles, Rotate3, Time.deltaTime * 8);
            bulletCamera.transform.localPosition = Vector3.Lerp(bulletCamera.transform.localPosition, Pos3, Time.deltaTime * 8);
        }
        else if(Place1)
        {
            bulletCamera.transform.localEulerAngles = Vector3.Lerp(bulletCamera.transform.localEulerAngles, Rotate1, Time.deltaTime * 8);
            bulletCamera.transform.localPosition = Vector3.Lerp(bulletCamera.transform.localPosition, Pos1, Time.deltaTime * 8);
        }
        //transform.Translate(Vector3.forward * 5 * Time.deltaTime);
        //transform.Translate(FPSPlayer.instance.WeaponBehaviorComponent.weaponLookDirection * 5 * Time.deltaTime);
        //if (this.transform.position== FPSPlayer.instance.WeaponBehaviorComponent.LasttargethitPoint&& !GameManager.instance.BulletHittedEnemy)
        if (this.transform.position == FPSPlayer.instance.WeaponBehaviorComponent.LasttargethitPoint && !GameManager.instance.BulletHittedEnemy)

        {
            print("rchd");
            if (!GameManager.instance.BulletHittedEnemy)
            {
                //bulletCamera.transform.localEulerAngles = Vector3.Lerp(bulletCamera.transform.localEulerAngles, lookpos, Time.deltaTime * 10);
                //bulletCamera.transform.localEulerAngles = lookDir;
                //bulletCamera.transform.localPosition = CamLastpos;
                Rotate3 = lookDir;
                Pos3 = CamLastpos;
            }
            GameManager.instance.BulletHittedEnemy = true;
            //bulletCamera.transform.localEulerAngles = new Vector3(25, 0, 0);
            //bulletCamera.transform.Rotate(90, 90, 0);
            //if(!FPSPlayer.instance.WeaponBehaviorComponent.lasthitObj.GetComponentInParent<HumanController>().dead)
           
            FPSPlayer.instance.WeaponBehaviorComponent.lasthitObj.GetComponentInParent<HumanController>().Die();
            AudioSource.PlayClipAtPoint(deathsound, GameObject.FindGameObjectWithTag("MainCamera").transform.position);
            Destroy(Bullet);

            //Destroy(this.gameObject, 2f);
            //Vector3 lookpos = new Vector3(FPSPlayer.instance.WeaponBehaviorComponent.lasthitObj.transform.rotation y);
            //bulletCamera.transform.LookAt(FPSPlayer.instance.WeaponBehaviorComponent.lasthitObj.transform);
            
        }
    }
    IEnumerator Slowbullet()
    {
        print("ienumatr");
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(100f);
        //Time.timeScale = 1f;
    }
    void OnCollisionEnter(Collision other)
    {
        //print("toucheeeeeeee");
        //if (other.transform.tag=="Untagged")
        //{
        //    print("toucheeeeeeee");
        //    Destroy(this.gameObject);
        //}
    }
}
