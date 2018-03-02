using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    Binary a = 0, b = "10";
	// Use this for initialization
	void Start ()
    {
        
        Debug.Log((string)a);
        Debug.Log((string)b);
        Debug.Log((string)(a-b));
    }
	
}
