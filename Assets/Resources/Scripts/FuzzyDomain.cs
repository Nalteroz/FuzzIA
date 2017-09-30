using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyDomain
{
    public int ID;
    public string Name;
    public float[] Range { get; private set; }
    public List<FuzzySet> FuzzySets { get; private set; }

    public FuzzyDomain(string Name, float RangeBegin, float RangeEnd)
    {
        FuzzySets = new List<FuzzySet>();
        this.Name = Name;
        if (RangeBegin < RangeEnd) SetRange(RangeBegin, RangeEnd);
        else
        {
            throw new System.InvalidOperationException("Range incorrect in FuzzyDomain!");
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

    public void AddSet(string name, float[] vertices)
    {
        if(FuzzySets.Exists(f => f.Name == name ))
        {
            throw new System.InvalidOperationException("A set with the name " + name + " already exist!");
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
        if(value < Range[0] || value > Range[1])
        {
            throw new System.InvalidOperationException("The value is not on the range of the domain " + Name);
        }
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
