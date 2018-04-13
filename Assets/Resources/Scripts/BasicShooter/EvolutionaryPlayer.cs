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
	}
	
	void Update ()
    {
		
	}
}
