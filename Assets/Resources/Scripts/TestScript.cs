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
        indomain1 = controller.AddImputDomain("p1", new Range(-1, 1));
        indomain1.AddSet("errado1", new float[] { -1, -1, 0});
        indomain1.AddSet("certo1", new float[] { 0, 1, 1 });
        indomain1.SetX(1f);
        indomain1 = controller.AddImputDomain("p2", new Range(-1, 1));
        indomain1.AddSet("errado2", new float[] { -1, -1, 0 });
        indomain1.AddSet("certo2  ", new float[] { 0, 1, 1 });
        indomain1.SetX(1f);
        //indomain = controller.AddImputDomain("C", new Range(-1, 1));
        //indomain.AddSet("c", new float[] { -1, -1, 0 });
        outdomain = controller.AddOutputDomain("R", new Range(-1, 1));
        outset = outdomain.AddSet("certo", new float[] { 0, 1, 1 });
        outset = outdomain.AddSet("errado", new float[] { -1, -1, 0 });
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
        Debug.Log(Handler.GetWinnerIdx());


    }

    float Avaliation()
    {
        controller.FulfillAllRules();
        Debug.Log(outdomain.Str());
        return outdomain.Defuzzyfication();
    }
	
}
