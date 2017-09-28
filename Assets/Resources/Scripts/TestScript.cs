using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzySector MyFS;
    Vector2[] Vert = new Vector2[3];
    

	// Use this for initialization
	void Start ()
    {
        Vert[0] = new Vector2(0, 0);
        Vert[1] = new Vector2(1, 1);
        Vert[2] = new Vector2(0, 0);
        MyFS = new FuzzySector("Teste", Vert);
        Debug.Log(MyFS.IsInDomain(1));
	}
	
}
