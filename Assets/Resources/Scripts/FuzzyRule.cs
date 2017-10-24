using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule
{
    public FuzzyValue[] Conditions { get; private set; }
    public FuzzySet Output { get; private set; }
    public LogicOperation Operation { get; private set; }
    public Dictionary<string, Range> Intensities = new Dictionary<string, Range>();

    public enum LogicOperation
    {
        And,
        Or,
    }

    public enum Intensity
    {
        VeryLow,
        Low,
        Average,
        High,
        VeryHigh
    }

    public void SetIntensity()
    {
        Intensities["VeryLow"] = new Range(0, 0.2f);
        Intensities["Low"] = new Range(0.2f, 0.4f);
        Intensities["Average"] = new Range(0.4f, 0.6f);
        Intensities["High"] = new Range(0.6f, 0.8f);
        Intensities["VeryHigh"] = new Range(0.8f, 1);
    }

    public FuzzyRule(FuzzyValue[] Conditions, LogicOperation Operation, FuzzySet Output)
    {
        if(Conditions.Length == 0)
        {
            throw new System.ArgumentException("Erro on create rule: Invalid argument!");
        }
        else
        {
            this.Conditions = Conditions;
            this.Operation = Operation;
            this.Output = Output;
        }
    }

    public FuzzyValue FulfillRule()
    {
        FuzzyValue Answer = new FuzzyValue(Output, 0);
        float NotValue = 0, DomainValue = 0;


        if (Conditions[0].NotFlag) Answer.Value = 1 - Conditions[0].Set.IsInDomain();
        else Answer.Value = Conditions[0].Set.IsInDomain();

        switch(Operation)
        {
            case LogicOperation.And:
                foreach(FuzzyValue Condition in Conditions)
                {
                    DomainValue = Condition.Set.IsInDomain();
                    if (Condition.NotFlag)
                    {
                        NotValue = 1 - DomainValue;
                        if (NotValue > Answer.Value) Answer.Value = NotValue;
                    }
                    else
                    {
                        if (DomainValue > Answer.Value) Answer.Value = DomainValue;
                    }
                }
                break;
            case LogicOperation.Or:
                foreach (FuzzyValue Condition in Conditions)
                {
                    DomainValue = Condition.Set.IsInDomain();
                    if (Condition.NotFlag)
                    {
                        NotValue = 1 - DomainValue;
                        if (NotValue < Answer.Value) Answer.Value = NotValue;
                    }
                    else
                    {
                        if (DomainValue < Answer.Value) Answer.Value = DomainValue;
                    }
                }
                break;
            default:
                Answer.Value = 0;
                break;
        }

        return Answer;
    }
    
}

public class RuleImput
{
    public Range Intensity;
    public FuzzySet Set;

    public RuleImput(FuzzySet set, Range intensity = null)
    {
        Intensity = intensity;
        Set = set;
    }

    public float isTrue()
    {
        float Value = Set.IsInDomain();
        if ((Value >= Intensity.Begin && Value <= Intensity.End) || Intensity == null)
        {
            return Value;
        }
        else return 0;
        
    }

}


