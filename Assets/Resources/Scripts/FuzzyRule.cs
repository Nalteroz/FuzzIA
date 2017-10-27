using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule
{
    private FuzzyController Controller;
    private List<RuleParameter> Conditions;
    private string Operation;
    private FuzzyValue Output;
    private Dictionary<string, Range> Intensities = new Dictionary<string, Range>();
    private Dictionary<string, LogicOperation> Operations = new Dictionary<string, LogicOperation>();

    public enum LogicOperation
    {
        And,
        Or,
    }

    public void SetIntensity()
    {
        Intensities.Add("very little", new Range(0, 0.2f));
        Intensities.Add("little", new Range(0.2f, 0.4f));
        Intensities.Add("Average", new Range(0.4f, 0.6f));
        Intensities.Add("much", new Range(0.6f, 0.8f));
        Intensities.Add("very much", new Range(0.8f, 1));
    }

    public void SetOperations()
    {
        Operations.Add("and", LogicOperation.And);
        Operations.Add("or", LogicOperation.Or);
    }

    public bool IsIntensity(string word)
    {
        if (Intensities.ContainsKey(word)) return true;
        else return false;
    }

    public bool IsOperation(string word)
    {
        if (Operations.ContainsKey(word)) return true;
        else return false;
    }

    public bool IsSet(string word)
    {
        if (Controller.SetsDictionary.ContainsKey(word)) return true;
        else return false;
    }


    public void SentenceHandler(string sentence)
    {
        int State = 0, WordIdx = 0;
        bool End = false;
        List<RuleParameter> conditions = new List<RuleParameter>();
        FuzzyValue output = null;
        string RuleOperation = null;
        string LowerSentence = sentence.ToLower();
        string[] SplitedSentence = LowerSentence.Split(' ');

        while(!End)
        {
            switch (State)
            {
                case 0:
                    if (SplitedSentence[WordIdx] == "if")
                    {
                        State++;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 1:
                    if (SplitedSentence[WordIdx] == "not" ||IsIntensity(SplitedSentence[WordIdx]) || IsSet(SplitedSentence[WordIdx]))
                    {
                        bool notflag = false;
                        Range intensity = null;
                        FuzzySet set;
                        RuleParameter NewParameter;
                        if(SplitedSentence[WordIdx] == "not")
                        {
                            notflag = true;
                            WordIdx++;
                        }
                        if (IsIntensity(SplitedSentence[WordIdx]))
                        {
                            intensity = Intensities[SplitedSentence[WordIdx]];
                            WordIdx++;
                        }
                        if (IsSet(SplitedSentence[WordIdx]))
                        {
                            set = Controller.SetsDictionary[SplitedSentence[WordIdx]];
                            NewParameter = new RuleParameter(set, intensity, notflag);
                            conditions.Add(NewParameter);
                        }
                        else
                        {
                            SentenceErro(SplitedSentence[WordIdx]);
                            return;
                        }
                        State++;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 2:
                    if (IsOperation(SplitedSentence[WordIdx]) && (Operation==null || Operation == SplitedSentence[WordIdx]))
                    {
                        RuleOperation = SplitedSentence[WordIdx];
                        State++;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 3:
                    if (SplitedSentence[WordIdx] == "not" || IsIntensity(SplitedSentence[WordIdx]) || IsSet(SplitedSentence[WordIdx]))
                    {
                        State=1;
                    }
                    else if(SplitedSentence[WordIdx] == "then")
                    {
                        State++;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 4:
                    if (IsSet(SplitedSentence[WordIdx]))
                    {
                        FuzzySet set;
                        set = Controller.SetsDictionary[SplitedSentence[WordIdx]];
                        output = new FuzzyValue(set);
                        State++;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 5:
                    if (WordIdx == SplitedSentence.Length)
                    {
                        Conditions = conditions;
                        Operation = RuleOperation;
                        Output = output;
                        End = true;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
            }
        }
    }

    private void SentenceErro(string erro)
    {
        throw new System.ArgumentException("Erro in sentence of the rule: Invalid sintaxe on " + erro);
    }
    
}

public class RuleParameter
{
    public bool NotFlag = false;
    public Range Intensity;
    public FuzzySet Set { get; private set; }

    public RuleParameter(FuzzySet set, Range intensity = null, bool notflag = false)
    {
        Set = set;
        Intensity = intensity;
        NotFlag = notflag;
    }

    public float IsTrue()
    {
        float Value = Set.IsInDomain();
        if ((Value >= Intensity.Begin && Value <= Intensity.End) || Intensity == null)
        {
            return Value;
        }
        else return 0;
        
    }

}


