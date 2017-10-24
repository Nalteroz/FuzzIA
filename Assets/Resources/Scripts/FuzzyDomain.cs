using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyDomain
{
    public int ID;
    public string Name;
    public float Var{ get; private set; }
    public Range Range { get; private set; }
    public List<FuzzySet> FuzzySets { get; private set; }
    public List<FuzzyValue> CurrentMembership { get; private set; }

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

    public void SetValue(float value)
    {
        if(value < Range.Begin || value > Range.End)
        {
            throw new System.FormatException("Erro in set value. The value is not on the range!");
        }
        else
        {
            Var = value;
            foreach(FuzzySet Set in FuzzySets)
            {
                Set.SetValue(Var);
            }
            CurrentMembership = GetMembership();
        }
    }
    public string str()
    {
        string Out = "";
        Out += "Domain name: " + Name + "\n";
        Out += "Range: " + "[" + Range.Begin.ToString() + ";" + Range.End.ToString() + "]" + "\n";
        foreach(FuzzySet Set in FuzzySets)
        {
            Out += Set.str();
        }
        return Out;
    }
    public void SetRange(float begin, float end)
    {
        Range = new Range(begin, end);
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
    public FuzzySet GetSet(string name)
    {
        FuzzySet Set = FuzzySets.Find(set => set.Name == name);
        if(Set!=null)
        {
            return Set;
        }
        else
        {
            throw new System.ArgumentException("Erro in GetSet: There is no set with the name " + name);
        }
    }
    public List<FuzzyValue> GetMembership()
    {
        List<FuzzyValue> Answer = new List<FuzzyValue>();
        float result = 0;
        foreach(FuzzySet Set in FuzzySets)
        {
            result = Set.IsInDomain();
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


