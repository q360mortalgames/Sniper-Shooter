using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
public class HumanController : MonoBehaviour {
	public int WalkSpeed,RunSpeed;
	[HideInInspector]
	public splineMove SplineMover;

	public float WaitTime = 1.8f;
    public bool FailOnReached, HumanBomber;
    public bool FindCover;
	void Start () 
	{
		//Set Walk Speed 0 if you want the char to be Idle intially

		SplineMover = GetComponent<splineMove> ();
		SplineMover.speed = WalkSpeed;

		if (WalkSpeed > 0) 
		{
            if (FindCover)
                GetComponent<MoveAnimator>().animator.SetBool("FirstIdleDone", true);

            GetComponent<splineMove> ().enabled = true;
            GetComponent<MoveAnimator>().animator.SetBool("Run", false);
            GetComponent<MoveAnimator>().animator.SetBool("Walk", true);
            GetComponent<MoveAnimator>().animator.SetBool("Idle", false);

           

            GetComponent<splineMove>().StartMove();

        }
        if (WalkSpeed > 1)
        {
            Run();
        }
    }
	
	public void Run()
	{
		if (!dead) {

            if (FindCover)
                GetComponent<MoveAnimator>().animator.SetBool("FirstIdleDone", true);

            GetComponent<splineMove> ().enabled = true;
			SplineMover.ChangeSpeed (RunSpeed);
            GetComponent<MoveAnimator>().animator.SetBool("Run",true);
            GetComponent<MoveAnimator>().animator.SetBool("Walk", false);
            GetComponent<MoveAnimator>().animator.SetBool("Idle", false);
            GetComponent<splineMove>().StartMove();

            Debug.Log("Run");

           

        }

    }
	public void Die()
	{
		GetComponent<MoveAnimator>().animator.SetBool("Run",false);
		GetComponent<MoveAnimator>().animator.SetBool("Walk", false);
		GetComponent<MoveAnimator>().animator.SetBool("Idle", false);
		GetComponent<MoveAnimator>().animator.SetBool("Attacking",false);

		SplineMover.Stop();
		GetComponent<MoveAnimator> ().animator.SetTrigger ("Die");
		Destroy (this.gameObject, 5);
		dead = true;
	}

    public GameObject Explosion;
    public void Explode()
    {
        Explosion.SetActive(true);
        Die();
        GameManager.instance.FailReason = failReason.TargetMissed;
        GameManager.instance.Invoke("LevelFail", 2);
    }
    bool alerted,dead;
	public void Alert()
	{
        /*
		if (!alerted) {
			GetComponent<MoveAnimator> ().animator.SetTrigger ("Alert");
			alerted = true;
			Invoke ("Run", WaitTime);

		}
        */
	}
    public void GoToCover()
    {

    }
}
