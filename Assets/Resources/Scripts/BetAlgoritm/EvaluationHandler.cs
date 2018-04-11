using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EvaluationHandler
{
    Event Event;
    Func<float> EvaluatePredictionMethod;
    public List<float> EvaluationResults;
    int EvaluationType, StepCount;
    public bool EvaluationComplete { get; private set;}

    public EvaluationHandler(Event eventpointer, Func<float> EvaluateMethod, int EvaluateByPass = 0)
    {
        Event = eventpointer;
        EvaluatePredictionMethod = EvaluateMethod;
        EvaluationResults = new List<float>();
        EvaluationType = EvaluateByPass;
        StepCount = 0;
        EvaluationComplete = false;
    }
    public string Str()
    {
        string Out = "Evaluations: [";
        foreach (float result in EvaluationResults)
        {
            Out += result + "; ";
        }
        Out += "] WinnerIndex: "+GetWinnerIdx()+"\n";
        return Out;
    }
    public void StepEvaluation()
    {
        if (!EvaluationComplete)
        {
            if (EvaluationType > 0)
            {
                for (int Step = StepCount * EvaluationType, StartIdx = Step; (Step < StartIdx + EvaluationType && Step < Event.RecomendationRules.Count); Step++)
                {
                    EvaluationResults.Add(EvaluateRecomendation(Event.RecomendationRules[Step]));
                }
                StepCount++;
                if (EvaluationResults.Count == Event.RecomendationRules.Count)
                {
                    EvaluationComplete = true;
                }
            }
            else if (EvaluationType == 0)
            {
                for (int Step = 0; Step < Event.RecomendationRules.Count; Step++)
                {
                    EvaluationResults.Add(EvaluateRecomendation(Event.RecomendationRules[Step]));
                }
                EvaluationComplete = true;
            }
        }
    }
    public void ResetEvaluation()
    {
        EvaluationComplete = false;
        EvaluationResults = new List<float>();
    }
    float EvaluateRecomendation(List<FuzzyRule> recomendation)
    {
        List<FuzzyRule> MainRules = Event.ControllerPointer.RulesList;
        Event.ControllerPointer.AddRule(recomendation);
        float Evaluiation = EvaluatePredictionMethod();
        Event.ControllerPointer.AddRule(MainRules);
        return Evaluiation;
    }
    public List<float> GetResult()
    {
        return EvaluationResults;
    }
    public int GetWinnerIdx()
    {
        int WinnerIdx = -1;
        float WinnerValue = 0;
        for(int i = 0; i < EvaluationResults.Count; i ++)
        {
            if(EvaluationResults[i] > WinnerValue)
            {
                WinnerIdx = i;
                WinnerValue = EvaluationResults[i];
            }
        }
        return WinnerIdx;
    }
}
