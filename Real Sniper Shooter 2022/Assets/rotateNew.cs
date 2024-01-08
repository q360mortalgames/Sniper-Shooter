using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateNew : MonoBehaviour
{
    public bool ChangeSize;
    public float Sizeto;
    // Start is called before the first frame update
    void Start()
    {
        if (!ChangeSize)
        {
            iTween.RotateBy(gameObject, iTween.Hash("z", 10, "time", 50, "islocal", true, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
        }
        if(ChangeSize)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 2, "islocal", true, "easetype", iTween.EaseType.linear));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.localPosition, transform.forward, Time.deltaTime * 2);
    }
}
