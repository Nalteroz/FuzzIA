using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    FuzzyController controller = new FuzzyController();
    Event Ev;
    InputDomain indomain1, indomain2;
    OutputDomain outdomain;
    House Hou;
    OutputSet outset;
    EvaluationHandler Handler;
	// Use this for initialization
	void Start ()
    {
        indomain1 = controller.AddImputDomain("A", new Range(-1, 1));
        indomain1.AddSet("a", new float[] { -1, -1, 0});
        indomain1.AddSet("b", new float[] { 0, 1, 1 });
        indomain1.SetX(-0.5f);
        indomain2 = controller.AddImputDomain("B", new Range(-1, 1));
        indomain2.AddSet("c", new float[] { -1, -1, 0 });
        indomain2.AddSet("d", new float[] { 0, 1, 1 });
        indomain2.SetX(1f);
        //indomain = controller.AddImputDomain("C", new Range(-1, 1));
        //indomain.AddSet("c", new float[] { -1, -1, 0 });
        outdomain = controller.AddOutputDomain("X", new Range(-1, 1));
        outset = outdomain.AddSet("x", new float[] { -1, -1, 0 });
        outset = outdomain.AddSet("y", new float[] { -0.5f, 0, 0.5f });
        Ev = new Event(controller);
        Hou = new House(Ev);
        Hou.GetRecomendations();
        Hou.GetBets();
        Ev.GetRecomendationsRules(Hou);
        Debug.Log(Ev.Str());
        Debug.Log(Hou.Str());
        Handler = new EvaluationHandler(Ev, Avaliation);
        Handler.StepEvaluation();
        Debug.Log(Handler.Str());
        Debug.Log(Handler.EvaluationComplete);


    }

    float Avaliation()
    {
        controller.FulfillAllRules();
        return outdomain.Defuzzyfication();
    }
	
}
