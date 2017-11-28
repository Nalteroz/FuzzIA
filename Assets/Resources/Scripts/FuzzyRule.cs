using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule
{
    private FuzzyController Controller;
    private List<RuleParameter> Conditions;
    private string Operation;
    private FuzzySet Output;
    public Dictionary<string, Range> Intensities = new Dictionary<string, Range>();
    private Dictionary<string, LogicOperation> Operations = new Dictionary<string, LogicOperation>();

    public FuzzyRule(FuzzyController controller, string sentence)
    {
        Controller = controller;
        SetIntensity();
        SetOperations();
        SentenceHandler(sentence);
    }

    public enum LogicOperation
    {
        And,
        Or,
    }
    public string Str()
    {
        string Out = "";
        Out += "Conditions:\n";
        foreach (RuleParameter condition in Conditions) Out += condition.Str();
        Out += "Operation: " + Operation + "\n";
        Out += "Output:\n";
        Out += Output.Str();
        return Out;
    }
    private void SetIntensity()
    {
        Intensities.Add("verylittle", new Range(0, 0.2f));
        Intensities.Add("little", new Range(0, 0.4f));
        Intensities.Add("rasoable", new Range(0.4f, 0.6f));
        Intensities.Add("much", new Range(0.6f, 1));
        Intensities.Add("verymuch", new Range(0.8f, 1));
    }
    private void SetOperations()
    {
        Operations.Add("and", LogicOperation.And);
        Operations.Add("or", LogicOperation.Or);
    }
    public bool IsImputDomain(string word)
    {
        if (Controller.ImputDomainsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    public bool IsOutputDomain(string word)
    {
        if (Controller.OutputDomainsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    public bool IsSet(FuzzyDomain domain, string word)
    {
        if (domain.SetsDictionary.ContainsKey(word)) return true;
        else return false;
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
    public void SentenceHandler(string sentence)
    {
        int State = 0, WordIdx = 0;
        bool End = false;
        List<RuleParameter> NewConditions = new List<RuleParameter>();
        FuzzySet NewOutput = null;
        string RuleOperation = null;
        string LowerSentence = sentence.ToLower();
        string[] SplitedSentence = LowerSentence.Split(' ');

        ImputDomain CurrentImputDomain = null;
        OutputDomain CurrentOutputDomain = null;

        while (!End)
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
                    if(IsImputDomain(SplitedSentence[WordIdx]) && SplitedSentence[WordIdx + 1] == "is")
                    {
                        CurrentImputDomain = Controller.ImputDomainsDictionary[SplitedSentence[WordIdx]];
                        WordIdx += 2;
                        State++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 2:
                    bool NotFlag = false;
                    Range CurrentIntensity = null;
                    FuzzySet CurrentSet;
                    RuleParameter NewParameter;

                    if (SplitedSentence[WordIdx] == "not" ||IsIntensity(SplitedSentence[WordIdx]) || IsSet(CurrentImputDomain, SplitedSentence[WordIdx]))
                    {

                        if(SplitedSentence[WordIdx] == "not")
                        {
                            NotFlag = true;
                            WordIdx++;
                        }
                        if (IsIntensity(SplitedSentence[WordIdx]))
                        {
                            CurrentIntensity = Intensities[SplitedSentence[WordIdx]];
                            WordIdx++;
                        }
                        if (IsSet(CurrentImputDomain, SplitedSentence[WordIdx]))
                        {
                            CurrentSet = CurrentImputDomain.SetsDictionary[SplitedSentence[WordIdx]];
                            NewParameter = new RuleParameter(CurrentSet, CurrentIntensity, NotFlag);
                            NewConditions.Add(NewParameter);
                            CurrentImputDomain = null;
                            State++;
                            WordIdx++;
                        }
                        else
                        {
                            SentenceErro(SplitedSentence[WordIdx]);
                            return;
                        }
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 3:
                    if (SplitedSentence[WordIdx] == "then")
                    {
                        State++;
                        WordIdx++;
                    }
                    else if (IsOperation(SplitedSentence[WordIdx]) && (RuleOperation==null || RuleOperation == SplitedSentence[WordIdx]))
                    {
                        RuleOperation = SplitedSentence[WordIdx];
                        State = 1;
                        WordIdx++;
                    }
                    else
                    {
                        SentenceErro(SplitedSentence[WordIdx]);
                        return;
                    }
                    break;
                case 4:
                    if (IsOutputDomain(SplitedSentence[WordIdx]) && SplitedSentence[WordIdx + 1] == "is")
                    {
                        CurrentOutputDomain = Controller.OutputDomainsDictionary[SplitedSentence[WordIdx]];
                        if(IsSet(CurrentOutputDomain, SplitedSentence[WordIdx + 2]))
                        {
                            NewOutput = CurrentOutputDomain.SetsDictionary[SplitedSentence[WordIdx + 2]];
                        }
                        else
                        {
                            SentenceErro(SplitedSentence[WordIdx + 2]);
                            return;
                        }
                        State++;
                        WordIdx += 3;
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
                        Conditions = NewConditions;
                        Operation = RuleOperation;
                        Output = NewOutput;
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
        throw new System.ArgumentException("Erro on create rule: The sentence have invalid sintaxe on " + erro);
    }
    public void FulfillRule()
    {
        float Result = Conditions[0].IsTrue(), CurrentValue;
        if(Operation == "and")
        {
            for(int index = 1; index < Conditions.Count; index++)
            {
                CurrentValue = Conditions[index].IsTrue();
                if(CurrentValue < Result)
                {
                    Result = CurrentValue;
                }
            }
        }
        else if(Operation == "or")
        {
            for (int index = 1; index < Conditions.Count; index++)
            {
                CurrentValue = Conditions[index].IsTrue();
                if (CurrentValue > Result)
                {
                    Result = CurrentValue;
                }
            }
        }
        Debug.Log(Result);
        Output.SetXbyY(Result);
    }
    
}

public class RuleParameter
{
    public bool NotFlag = false;
    public Range Intensity = null;
    public FuzzySet Set { get; private set; }

    public RuleParameter(FuzzySet set, Range intensity = null, bool notflag = false)
    {
        Set = set;
        Intensity = intensity;
        NotFlag = notflag;
    }
    public void SetParameter(FuzzySet set, Range intensity = null, bool notflag = false)
    {
        Set = set;
        Intensity = intensity;
        NotFlag = notflag;
    }
    public float IsTrue()
    {
        float Value = Set.IsInDomain();
        if (Intensity == null || Intensity.IsInTheRange(Value))
        {
            return Value;
        }
        else return 0;
        
    }
    public string Str()
    {
        string Out = "";
        Out += "Not flag: " + NotFlag.ToString() + "\n";
        if(Intensity!=null) Out += "Intensity: " + Intensity.Str() + "\n";
        Out += "Set: " + Set.Str() + "\n";
        return Out;
    }
}


