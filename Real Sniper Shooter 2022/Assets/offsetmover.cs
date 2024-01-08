using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetmover : MonoBehaviour
{
    // Start is called before the first frame update
    public float scrollSpeed = 0.5f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    //float offset = 0;

    void Update()
        {
        float offset = Time.time * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(0, offset);
        //rend.material.mainTextureOffset.Set(0, offset+0.1f);

    }
}

