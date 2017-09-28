using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyDomain
{
    public string Name;
    public float[] Range { get; private set; }
    public List<FuzzySet> FuzzySets = new List<FuzzySet>();

    public FuzzyDomain(string Name, float RangeBegin, float RangeEnd)
    {
        this.Name = Name;
        if (RangeBegin <= RangeEnd) SetRange(RangeBegin, RangeEnd);
        else
        {
            Debug.LogError("Range incorrect in FuzzyDomain. Inverting");
            SetRange(RangeEnd, RangeBegin);
        }
    }

    public string str()
    {
        string Out = "";
        Out += "Domain name: " + Name + "\n";
        Out += "Range: " + "[" + Range[0].ToString() + ";" + Range[1].ToString() + "]" + "\n";
        foreach(FuzzySet Set in FuzzySets)
        {
            Out += Set.str();
        }
        return Out;
    }

    public void SetRange(float begin, float end)
    {
        Range = new float[] { begin, end };
    }

    public void AddFuzzySet(string name, float[] vertices)
    {
        if(FuzzySets.Exists(f => f.Name == name ))
        {
            Debug.LogError("The fuzzyset already exist in the list.");
            return;
        }
        else
        {
            FuzzySets.Add(new FuzzySet(name, vertices));
        }
    }

    public List<FuzzyOutput> GetAnswer(float value)
    {
        List<FuzzyOutput> Answer = new List<FuzzyOutput>();
        float result = 0;
        foreach(FuzzySet Set in FuzzySets)
        {
            result = Set.IsInDomain(value);
            if (result > 0) Answer.Add(new FuzzyOutput(Set.Name, result));
        }
        return Answer;
    }

}

public class FuzzyOutput
{
    public string SetName;
    public float SetValue;

    public FuzzyOutput(string name, float value)
    {
        SetName = name;
        SetValue = value;
    }
}
