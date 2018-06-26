using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    Matrix4x4 M = Matrix4x4.zero;
    Vector2 v = Vector2.up, vt;
	// Use this for initialization
	void Start () {
        M.m01 = 1;
        M.m10 = 1;

        vt = M.MultiplyVector(v);
        Debug.Log(v);
        Debug.Log(M);
        Debug.Log(vt);
	}
	
	
}
