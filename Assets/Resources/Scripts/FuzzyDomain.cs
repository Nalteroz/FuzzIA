using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FuzzyDomain
{
    
    public string Name;
    public Range DomainRange { get; private set; }
    public List<FuzzySet> FuzzySets { get; private set; }
    public Dictionary<string, FuzzySet> SetsDictionary { get; private set; }
    public List<FuzzyValue> CurrentMembership { get; private set; }

    public FuzzyDomain(string name, Range range)
    {
        FuzzySets = new List<FuzzySet>();
        SetsDictionary = new Dictionary<string, FuzzySet>();
        Name = name;
        DomainRange = range;
    }

    public void SetRange(Range range)
    {
        float Smaller = float.PositiveInfinity, Biggest = float.NegativeInfinity;
        if (range.Begin <= DomainRange.Begin && range.End >= DomainRange.End)
        {
            DomainRange = range;
        }
        else
        {
            foreach(FuzzySet Set in FuzzySets)
            {
                if (Set.SetRange.Begin < Smaller) Smaller = Set.SetRange.Begin;
                if (Set.SetRange.End > Biggest) Biggest = Set.SetRange.End;
            }
            if(range.Begin > Smaller || range.End < Biggest)
            {
                throw new System.ArgumentException("Erro in SetRange on "+Name+". Can't set a range smaller ou bigger then the range of the sets.");
            }
            else
            {
                DomainRange = range;
            }
        }
    }
    public FuzzySet AddSet(string name, float[] vertices)
    {
        FuzzySet NewSet;
        string setname = name.ToLower();
        if (!SetsDictionary.ContainsKey(setname))
        {
            foreach (float vertice in vertices)
            {
                if (!DomainRange.IsInTheRange(vertice))
                {
                    throw new System.ArgumentException("Erro in AddSet on " + Name + ": The vertices is not in the range.");
                }
            }
            NewSet = new FuzzySet(this, setname, vertices);
            FuzzySets.Add(NewSet);
            SetsDictionary.Add(setname, NewSet);
            return NewSet;
        }
        else throw new System.ArgumentException("Erro on AddSet to "+ Name +". A set with the name "+ setname +" already exist.");
    }
    public void AddSet(FuzzySet set)
    {
        if (!SetsDictionary.ContainsKey(set.Name) && DomainRange.IsInTheRange(set.SetRange))
        {
            FuzzySets.Add(set);
            SetsDictionary.Add(set.Name, set);
        }
        else throw new System.ArgumentException("Erro on CreateSet on "+Name+". The set already exist or is not in the range of the domain.");
    }
    public FuzzySet RemoveSet(string name)
    {
        FuzzySet Set;
        if (!SetsDictionary.ContainsKey(name))
        {
            Set = SetsDictionary[name];
            SetsDictionary.Remove(Set.Name);
            FuzzySets.Remove(Set);
            return Set;
        }
        else throw new System.ArgumentException("Erro in RemoveSet on " + Name + ": The set name "+ name +" do no exist in the domain.");
    }
    public void RemoveSet(FuzzySet Set)
    {
        if (SetsDictionary.ContainsKey(Set.Name))
        {
            SetsDictionary.Remove(Set.Name);
            FuzzySets.Remove(Set);
        }
        else throw new System.ArgumentException("Erro on remove set on"+ Name +": The set not exist!");
    }
    public FuzzySet GetSet(string name)
    {
        if(SetsDictionary.ContainsKey(name))
        {
            return SetsDictionary[name];
        }
        else throw new System.ArgumentException("Erro in GetSet on "+ Name +": There is no set with the name " + name);
    }
    public void SetMembership()
    {
        List<FuzzyValue> Answer = new List<FuzzyValue>();
        float result = 0;
        foreach(FuzzySet Set in FuzzySets)
        {
            result = Set.IsInDomain();
            if (result > 0) Answer.Add(new FuzzyValue(Set, result));
        }
        CurrentMembership = Answer;
    }
}


