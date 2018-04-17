using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryPong : PongBar
{
    public PongBall BallObject;

    FuzzyController FzCtrl;
    InputDomain Input1, Input2;
    OutputDomain Output;
    Game GameCtlr;
    float Defused, NullValue = float.NegativeInfinity;
	// Use this for initialization
	void Start ()
    {
        FzCtrl = new FuzzyController();
        Input1 = FzCtrl.AddInputDomain("Distance", new Range(0, 20));
        Input2 = FzCtrl.AddInputDomain("yPosition", new Range(-5, 5));
        Output = FzCtrl.AddOutputDomain("Action", new Range(-1, 1));
        Input1.AddSet("Middle", new float[] { 7, 10, 13 });
        Input1.AddSet("VeryClose", new float[] { 0, 0, 3 });
        Input1.AddSet("Close", new float[] { 2, 5, 8 });
        Input1.AddSet("VeryFar", new float[] { 17, 20, 20 });
        Input1.AddSet("Far", new float[] { 12, 15, 18 });
        Input2.AddSet("Up", new float[] { 0, 5, 5 });
        Input2.AddSet("Down", new float[] { -5, -5, 0 });
        Output.AddSet("MoveUp", new float[] { 0, 1, 1 });
        Output.AddSet("MoveDown", new float[] { -1, -1, 0 });

        GameCtlr = new Game(FzCtrl, -1, PredictedDistance);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Input1.SetX(Vector3.Distance(transform.position, BallObject.transform.position));
        Input2.SetX(BallObject.transform.position.y - transform.position.y);
        GameCtlr.PlayTurn();
        FzCtrl.FulfillAllRules();
        MakeAction();
        GameCtlr.ResetTurnCount();
    }

    void MakeAction()
    {
        Defused = Output.Defuzzyfication();
        if (Defused != NullValue)
        {
            if (Defused > 0) MoveUp();
            else if (Defused < 0) MoveDown();
        }
    }

    public float PredictedDistance()
    {
        Vector3 SavedPosition = transform.position;
        float RealDistance = Vector3.Distance(transform.position, BallObject.transform.position), PredictedDistance;
        FzCtrl.FulfillAllRules();
        MakeAction();
        PredictedDistance = Vector3.Distance(transform.position, BallObject.transform.position);
        transform.position = SavedPosition;
        return (RealDistance - PredictedDistance);
    }
}
