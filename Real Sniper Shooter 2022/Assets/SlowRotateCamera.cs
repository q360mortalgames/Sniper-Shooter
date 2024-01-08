using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotateCamera : MonoBehaviour
{
    public float RotateValue;
    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateTo(gameObject, iTween.Hash("y", RotateValue, "time", 6, "islocal", true, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong));
    }

    // Update is called once per frame
    void Update()
    {
        //if(transform.localEulerAngles.y)
        //Vector3 newRotation = new Vector3(0, -110, 0);

        //    transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, newRotation, Time.time *10);
        //iTween.RotateTo(gameObject, iTween.Hash("y", -110, "time", 1f, "islocal", true, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong));
    }
}
