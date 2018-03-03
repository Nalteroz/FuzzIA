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
	// Use this for initialization
	void Start ()
    {
        indomain = controller.AddImputDomain("A", new Range(-1, 1));
        indomain.AddSet("a", new float[] { -1, -1, 0});
        indomain.AddSet("b", new float[] { 0, 0, 1 });
        indomain.AddSet("c", new float[] { 0, 0, 1 });
        indomain = controller.AddImputDomain("B", new Range(-1, 1));
        indomain.AddSet("d", new float[] { -1, -1, 0 });
        indomain.AddSet("e", new float[] { 0, 0, 1 });
        indomain.AddSet("f", new float[] { 0, 0, 1 });
        Ev = new Event(controller);
        Ev.SetPossibilitiesByCombination();
    }
	
}
