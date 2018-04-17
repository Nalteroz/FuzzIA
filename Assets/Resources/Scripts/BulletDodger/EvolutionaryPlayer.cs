using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryPlayer : PlayerMovment
{
    FuzzyController FzCtrl;
    InputDomain Input1, Input2;
    OutputDomain Output;
    Game GameCtlr;
    float Defused, NullValue = float.NegativeInfinity;

    void Start ()
    {
        FzCtrl = new FuzzyController();
        Input1 = FzCtrl.AddInputDomain("Distance", new Range(0, Distance));
        Input2 = FzCtrl.AddInputDomain("Orientation", new Range(-1, 1));
        Output = FzCtrl.AddOutputDomain("Move", new Range(-1, 1));

        Input1.AddSet("Close", new float[] {0, Distance/4, Distance/2});
        Input1.AddSet("Middle", new float[] {Distance/4, Distance/2, (3*Distance)/4 });
        Input1.AddSet("Far", new float[] { Distance/2, (3*Distance)/4,  Distance});

        Input2.AddSet("Right", new float[] { -1, -1, 0});
        Input2.AddSet("Mid", new float[] { -0.5f, -0, 0.5f });
        Input2.AddSet("Left", new float[] { 0, 1, 1});

        Output.AddSet("Left", new float[] { -1, -1, 0});
        Output.AddSet("Right", new float[] { 0, 1, 1});
    }
	
	void Update ()
    {

	}
}
