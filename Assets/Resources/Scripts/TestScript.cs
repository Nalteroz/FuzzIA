using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzyController Controller = new FuzzyController();
    ImputDomain Imput1, Imput2;
    OutputDomain Output;
    FuzzyRule Rule1, Rule2;
    

	// Use this for initialization
	void Start ()
    {
        Imput1 = Controller.AddImputDomain("Distance", new Range(0, 10));
        Imput2 = Controller.AddImputDomain("Velocity", new Range(0, 10));
        Output = Controller.AddOutputDomain("You", new Range(0, 10));
        Imput1.AddSet("Close", new float[] { 0, 0, 3, 5});
        Imput1.AddSet("Medium", new float[] { 3, 5, 7 });
        Imput1.AddSet("Far", new float[] { 5, 7, 10, 10 });
        Imput2.AddSet("Slow", new float[] { 0, 0, 3, 5 });
        Imput2.AddSet("Average", new float[] { 3, 5, 7 });
        Imput2.AddSet("Fast", new float[] { 5, 7, 10, 10 });
        Output.AddSet("Walk", new float[] { 0, 0, 3, 5});
        Output.AddSet("Run", new float[] { 3, 5, 7 });
        Output.AddSet("Dash", new float[] { 5, 7, 10, 10 });
        Controller.AddRule("if distance is close and velocity is average then You is dash");
        Imput1.SetX(4.5f);
        Imput2.SetX(6.5f);
        Controller.FulfillAllRules();
        Debug.Log(Output.Defuzzyfication());


    }
	
}
