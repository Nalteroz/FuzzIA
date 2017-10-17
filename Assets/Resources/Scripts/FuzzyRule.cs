using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule
{
    public List<FuzzyValue> Conditions { get; private set; }
    public FuzzySet Output { get; private set; }
    public LogicOperation Operation { get; private set; }

    public enum LogicOperation
    {
        And,
        Or,
    }
    public FuzzyRule(List<FuzzyValue> Conditions, LogicOperation Operation, FuzzySet Output)
    {
        if(Conditions.Count == 0)
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

