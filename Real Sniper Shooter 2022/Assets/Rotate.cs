﻿using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float Speed=1f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        //transform.Rotate(transform.forward *Speed);

        transform.RotateAround(transform.position, transform.up, Time.deltaTime * Speed);
    }
}
