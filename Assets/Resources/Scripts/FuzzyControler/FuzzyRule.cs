using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyRule
{
    public List<RuleParameter> ConditionsList { get; private set; }
    public string RuleOperation { get; private set; }
    public OutputSet OutputSet { get; private set; }
    private Dictionary<string, Range> IntensitiesDictionary = new Dictionary<string, Range>();
    private Dictionary<string, LogicOperation> OperationsDictionary = new Dictionary<string, LogicOperation>();

    public FuzzyRule(FuzzyController controller, string sentence)
    {
        SetIntensity();
        SetOperations();
        SentenceHandler(controller, sentence);
    }
    public FuzzyRule(List<RuleParameter> conditions, string operation, OutputSet output)
    {
        ConditionsList = conditions;
        RuleOperation = operation;
        OutputSet = output;
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
        foreach (RuleParameter condition in ConditionsList) Out += condition.Str();
        Out += "Operation: " + RuleOperation + "\n";
        Out += "Output:\n";
        Out += OutputSet.Str();
        return Out;
    }
    private void SetIntensity()
    {
        IntensitiesDictionary.Add("verylittle", new Range(0, 0.2f));
        IntensitiesDictionary.Add("little", new Range(0.2f, 0.4f));
        IntensitiesDictionary.Add("rasoable", new Range(0.4f, 0.6f));
        IntensitiesDictionary.Add("very", new Range(0.6f, 0.8f));
        IntensitiesDictionary.Add("verymuch", new Range(0.8f, 1));
    }
    private void SetOperations()
    {
        OperationsDictionary.Add("and", LogicOperation.And);
        OperationsDictionary.Add("or", LogicOperation.Or);
    }
    private bool IsImputDomain(FuzzyController Controller, string word)
    {
        if (Controller.ImputDomainsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    private bool IsOutputDomain(FuzzyController Controller, string word)
    {
        if (Controller.OutputDomainsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    private bool IsInputSet(InputDomain domain, string word)
    {
        if (domain.SetsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    private bool IsOutputSet(OutputDomain domain, string word)
    {
        if (domain.SetsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    private bool IsIntensity(string word)
    {
        if (IntensitiesDictionary.ContainsKey(word)) return true;
        else return false;
    }
    private bool IsOperation(string word)
    {
        if (OperationsDictionary.ContainsKey(word)) return true;
        else return false;
    }
    public void SentenceHandler(FuzzyController Controller, string sentence)
    {
        int State = 0, WordIdx = 0;
        bool End = false;
        List<RuleParameter> NewConditions = new List<RuleParameter>();
        OutputSet NewOutput = null;
        string RuleOperation = null;
        string LowerSentence = sentence.ToLower();
        string[] SplitedSentence = LowerSentence.Split(' ');

        InputDomain CurrentImputDomain = null;
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
                    if(IsImputDomain(Controller, SplitedSentence[WordIdx]) && SplitedSentence[WordIdx + 1] == "is")
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
                    InputSet CurrentSet = null;
                    RuleParameter NewParameter = null;

                    if (SplitedSentence[WordIdx] == "not" || IsIntensity(SplitedSentence[WordIdx]) || IsInputSet(CurrentImputDomain, SplitedSentence[WordIdx]))
                    {

                        if(SplitedSentence[WordIdx] == "not")
                        {
                            NotFlag = true;
                            WordIdx++;
                        }
                        if (IsIntensity(SplitedSentence[WordIdx]))
                        {
                            CurrentIntensity = IntensitiesDictionary[SplitedSentence[WordIdx]];
                            WordIdx++;
                        }
                        if (IsInputSet(CurrentImputDomain, SplitedSentence[WordIdx]))
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
                    if (IsOutputDomain(Controller, SplitedSentence[WordIdx]) && SplitedSentence[WordIdx + 1] == "is")
                    {
                        CurrentOutputDomain = Controller.OutputDomainsDictionary[SplitedSentence[WordIdx]];
                        if(IsOutputSet(CurrentOutputDomain, SplitedSentence[WordIdx + 2]))
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
                        ConditionsList = NewConditions;
                        this.RuleOperation = RuleOperation;
                        OutputSet = NewOutput;
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
        float Result = ConditionsList[0].IsTrue(), CurrentValue;
        if(RuleOperation == "and")
        {
            for(int index = 1; index < ConditionsList.Count; index++)
            {
                CurrentValue = ConditionsList[index].IsTrue();
                if(CurrentValue < Result)
                {
                    Result = CurrentValue;
                }
            }
        }
        else if(RuleOperation == "or")
        {
            for (int index = 1; index < ConditionsList.Count; index++)
            {
                CurrentValue = ConditionsList[index].IsTrue();
                if (CurrentValue > Result)
                {
                    Result = CurrentValue;
                }
            }
        }
        OutputSet.AddValue(Result);
    }
    
}

public class RuleParameter
{
    public bool NotFlag { get; private set; }
    public Range Intensity { get; private set; }
    public InputSet Set { get; private set; }

    public RuleParameter(InputSet set, Range intensity = null, bool notflag = false)
    {
        Set = set;
        Intensity = intensity;
        NotFlag = notflag;
    }
    public void SetParameter(InputSet set, Range intensity = null, bool notflag = false)
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
            if(NotFlag)
            {
                return 1 - Value;
            }
            else return Value;
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


