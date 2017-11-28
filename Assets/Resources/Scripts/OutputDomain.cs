using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDomain : FuzzyDomain
{
    private FuzzyController Controller;
    
    public OutputDomain(FuzzyController controller, string name, Range range) 
        : base( name, range)
    {
        Controller = controller;
    }
    
    public float Defuzzyfication()
    {
        float DefuzzedValue = float.PositiveInfinity;

        SetMembership();

        if(FuzzySets.Count == 0)
        {
            throw new System.InvalidOperationException("Erro on defuzzing "+ Name +". There is no sets in this domain.");
        }
        else if(CurrentMembership.Count == 0)
        {
            return float.NegativeInfinity;
        }
        else
        {
            float A = 0, B = 0;
            CenterOfArea CenterOfArea;
            foreach (FuzzyValue SetValue in CurrentMembership)
            {
                Debug.Log(SetValue.Str());
                CenterOfArea = SetValue.Set.GetCenterOfArea();
                Debug.Log(CenterOfArea.Str());
                A += CenterOfArea.xPosition * CenterOfArea.Mass;
                B += CenterOfArea.Mass;
            }
            Debug.Log("Total A:" + A);
            Debug.Log("Total B:" + B);
            DefuzzedValue = A / B;

            return DefuzzedValue;
        }
    }
}
