using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    FuzzyController controller = new FuzzyController();
    Event Ev;
    InputDomain indomain;
    OutputDomain outdomain;
    House Hou;
    OutputSet outset;
    Player[] P = new Player[3];
    Addiction[] ad = new Addiction[3];
	// Use this for initialization
	void Start ()
    {
        indomain = controller.AddImputDomain("A", new Range(-1, 1));
        indomain.AddSet("a", new float[] { -1, -1, 0});
        //indomain.AddSet("b", new float[] { 0, 0, 1 });
        indomain = controller.AddImputDomain("B", new Range(-1, 1));
        indomain.AddSet("b", new float[] { -1, -1, 0 });
        //indomain.AddSet("e", new float[] { 0, 0, 1 });
        indomain = controller.AddImputDomain("C", new Range(-1, 1));
        indomain.AddSet("c", new float[] { -1, -1, 0 });
        outdomain = controller.AddOutputDomain("X", new Range(-1, 1));
        outset = outdomain.AddSet("x", new float[] { -1, -1, 0 });
        outset = outdomain.AddSet("y", new float[] { -0.5f, 0, 0.5f });
        Ev = new Event(controller);
        Hou = new House(Ev);
        Debug.Log(Hou.Str());
        //P[0] = new Player(Hou, outdomain);
        //P[1] = new Player(Hou, outdomain);
        //P[2] = new Player(Hou, outdomain);
        //Debug.Log(P[0].Str());
        //Debug.Log(P[1].Str());
        //Debug.Log(P[2].Str());
        //ad[0] = new Addiction(3);
        //ad[1] = new Addiction(3);
        //ad[2] = new Addiction(3);
        //Debug.Log(ad[0].Str());
        //Debug.Log(ad[1].Str());
        //Debug.Log(ad[2].Str());




    }
	
}
