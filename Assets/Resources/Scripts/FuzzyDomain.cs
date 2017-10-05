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
            throw new System.InvalidOperationException("Erro on create domain: Range incorrect in FuzzyDomain!");
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
            throw new System.InvalidOperationException("Erro on add set: A set with the name " + name + " already exist!");
        }
        else
        {
            FuzzySets.Add(new FuzzySet(name, vertices));
        }
    }
    public void RemoveSet(string name)
    {
        FuzzySet Set = FuzzySets.Find(f => f.Name == name);
        RemoveSet(Set);
    }
    public void RemoveSet(FuzzySet Set)
    {
        if (Set == null)
        {
            throw new System.InvalidOperationException("Erro on remove set: The set not exist!");
        }
        else
        {
            FuzzySets.Remove(Set);
        }
    }
    public List<FuzzyValue> GetMembership(float value)
    {
        List<FuzzyValue> Answer = new List<FuzzyValue>();
        float result = 0;
        if(value < Range[0] || value > Range[1])
        {
            throw new System.InvalidOperationException("Erro on get answer of domain: The value is not on the range value of " + Name);
        }
        foreach(FuzzySet Set in FuzzySets)
        {
            result = Set.IsInDomain(value);
            if (result > 0) Answer.Add(new FuzzyValue(Set, result));
        }
        return Answer;
    }
}

public class FuzzyValue
{
    public bool NotFlag = false;
    public FuzzySet Set;
    public float Value;

    public FuzzyValue(FuzzySet Set, float value)
    {
        this.Set = Set;
        Value = value;
    }
}
