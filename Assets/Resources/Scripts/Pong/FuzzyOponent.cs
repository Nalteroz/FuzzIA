using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyOponent : PongBar
{
    public GameObject BallObject;

    FuzzyController Controller = new FuzzyController();
    InputDomain Input1, Input2;
    OutputDomain Output;
    FuzzyRule[] Rules = new FuzzyRule[4];
    float Defused, NullValue = float.NegativeInfinity;

    // Use this for initialization
    void Start ()
    {
        Input1 = Controller.AddImputDomain("Distance", new Range(0, 20));
        Input2 = Controller.AddImputDomain("yPosition", new Range(-5, 5));
        Output = Controller.AddOutputDomain("Action", new Range(-1, 1));
        Input1.AddSet("Far", new float[] { 12, 15, 20, 20});
        Input1.AddSet("Mid", new float[] { 5, 10, 15 });
        Input1.AddSet("Close", new float[] { 0, 0, 5, 8 });
        Input2.AddSet("Up", new float[] {0, 5, 5 });
        Input2.AddSet("Down", new float[] { -5, -5, 0});
        Output.AddSet("MoveUp", new float[] { 0, 1, 1 });
        Output.AddSet("MoveDown", new float[] { -1, -1, 0 });
        Output.AddSet("GetCenter", new float[] { -0.5f, 0, 0.5f });
        Rules[0] = Controller.AddRule("if distance is close and yposition is down then action is movedown");
        Rules[0] = Controller.AddRule("if distance is mid and yposition is down then action is movedown");
        Rules[1] = Controller.AddRule("if distance is close and yposition is up then action is moveup");
        Rules[1] = Controller.AddRule("if distance is mid and yposition is up then action is moveup");
    }
	
	// Update is called once per frame
	void Update ()
    {
        Input1.SetX(Vector3.Distance(transform.position, BallObject.transform.position));
        Input2.SetX(BallObject.transform.position.y - transform.position.y);
        Controller.FulfillAllRules();
        Defused = Output.Defuzzyfication();
        if(Defused != NullValue) MakeAction(Defused);
    }

    void MakeAction(float decision)
    {
        if (decision > 0) MoveUp();
        else if (decision < 0) MoveDown();
    }
    
}
