using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzyDomain MyFS;
    

	// Use this for initialization
	void Start ()
    {
        MyFS = new FuzzyDomain("Teste", new float[] { 0, 1, 2, 2});
        Debug.Log(MyFS.Type);
        Debug.Log(MyFS.IsInDomain(0.5f));
	}
	
}
