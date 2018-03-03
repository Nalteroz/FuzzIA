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
    OutputSet outset;
    FuzzyRule Rule;
	// Use this for initialization
	void Start ()
    {
        indomain = controller.AddImputDomain("A", new Range(-1, 1));
        indomain.AddSet("a", new float[] { -1, -1, 0});
        //indomain.AddSet("b", new float[] { 0, 0, 1 });
        indomain = controller.AddImputDomain("B", new Range(-1, 1));
        indomain.AddSet("b", new float[] { -1, -1, 0 });
        //indomain.AddSet("e", new float[] { 0, 0, 1 });
        outdomain = controller.AddOutputDomain("X", new Range(-1, 1));
        outset = outdomain.AddSet("x", new float[] { -1, -1, 0 });
        //indomain.AddSet("g", new float[] { 0, 0, 1 });
        Ev = new Event(controller);
        Ev.SetPossibilitiesByCombination();
        Rule = Ev.GetRule(2, outset);
        Debug.Log(Rule.Str());
        Ev.CountAWin(2);
        Debug.Log(Ev.Str());
    }
	
}
