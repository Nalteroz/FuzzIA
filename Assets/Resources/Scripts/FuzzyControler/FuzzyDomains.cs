using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FuzzyDomain
{
    public FuzzyController Controller { get; protected set; }
    public string Name { get; protected set; }
    public Range DomainRange { get; protected set; }

    public FuzzyDomain(FuzzyController controller, string name, Range range)
    {
        Controller = controller;
        Name = name;
        DomainRange = range;
    }
}

public class InputDomain : FuzzyDomain
{
    public List<InputSet> Sets { get; private set; }
    public Dictionary<string, InputSet> SetsDictionary { get; private set; }
    public List<FuzzyValue> CurrentMembership { get; private set; }
    public float X { get; private set; }

    public InputDomain(FuzzyController controller, string name, Range range)
        : base(controller, name, range)
    {
        Sets = new List<InputSet>();
        SetsDictionary = new Dictionary<string, InputSet>();
        CurrentMembership = new List<FuzzyValue>();
        X = float.NegativeInfinity;
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
            foreach (FuzzySet Set in Sets)
            {
                if (Set.SetRange.Begin < Smaller) Smaller = Set.SetRange.Begin;
                if (Set.SetRange.End > Biggest) Biggest = Set.SetRange.End;
            }
            if (range.Begin > Smaller || range.End < Biggest)
            {
                throw new System.ArgumentException("Erro in SetRange on " + Name + ". Can't set a range smaller ou bigger then the range of the sets.");
            }
            else
            {
                DomainRange = range;
            }
        }
    }
    public InputSet AddSet(string name, float[] vertices)
    {
        InputSet NewSet;
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
            NewSet = new InputSet(this, setname, vertices);
            Sets.Add(NewSet);
            SetsDictionary.Add(setname, NewSet);
            return NewSet;
        }
        else throw new System.ArgumentException("Erro on AddSet to " + Name + ". A set with the name " + setname + " already exist.");
    }
    public InputSet RemoveSet(string name)
    {
        InputSet Set;
        if (SetsDictionary.ContainsKey(name))
        {
            Set = SetsDictionary[name];
            SetsDictionary.Remove(Set.Name);
            Sets.Remove(Set);
            return Set;
        }
        else throw new System.ArgumentException("Erro in RemoveSet on " + Name + ": The set name " + name + " do no exist in the domain.");
    }
    public InputSet GetSet(string name)
    {
        if (SetsDictionary.ContainsKey(name))
        {
            return SetsDictionary[name];
        }
        else throw new System.ArgumentException("Erro in GetSet on " + Name + ": There is no set with the name " + name);
    }
    public string Str()
    {
        string Out = "";
        Out += "Domain name: " + Name + "\n";
        Out += "Range: " + DomainRange.Str() + "\n";
        Out += "FuzzySets: " + "\n";
        foreach (InputSet set in Sets) Out += set.Str();
        Out += "Current membership: " + "\n";
        foreach (FuzzyValue value in CurrentMembership) Out += value.Str();
        return Out;
    }
    public void SetX(float value)
    {
        foreach (InputSet set in Sets)
        {
            set.SetX(value);
        }
        SetMembership();
    }
    void SetMembership()
    {
        List<FuzzyValue> Answer = new List<FuzzyValue>();
        float result = 0;
        foreach (InputSet Set in Sets)
        {
            result = Set.IsInDomain();
            if (result > 0) Answer.Add(new FuzzyValue(Set, result));
        }
        CurrentMembership = Answer;
    }
}

public class OutputDomain : FuzzyDomain
{
    public List<OutputSet> Sets { get; private set; }
    public Dictionary<string, OutputSet> SetsDictionary { get; private set; }
    private List<FuzzyValue> OutputList;

    public OutputDomain(FuzzyController controller, string name, Range range)
        : base(controller, name, range)
    {
        Sets = new List<OutputSet>();
        SetsDictionary = new Dictionary<string, OutputSet>();
        OutputList = new List<FuzzyValue>();
    }

    public string Str()
    {
        string Out = "";
        Out += "Domain name: " + Name + "\n";
        Out += "Range: " + DomainRange.Str() + "\n";
        Out += "FuzzySets: " + "\n";
        foreach (OutputSet set in Sets) Out += set.Str();
        Out += "Current output: " + "\n";
        foreach (FuzzyValue value in OutputList) Out += value.Str();
        return Out;
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
            foreach (FuzzySet Set in Sets)
            {
                if (Set.SetRange.Begin < Smaller) Smaller = Set.SetRange.Begin;
                if (Set.SetRange.End > Biggest) Biggest = Set.SetRange.End;
            }
            if (range.Begin > Smaller || range.End < Biggest)
            {
                throw new System.ArgumentException("Erro in SetRange on " + Name + ". Can't set a range smaller ou bigger then the range of the sets.");
            }
            else
            {
                DomainRange = range;
            }
        }
    }
    public OutputSet AddSet(string name, float[] vertices)
    {
        OutputSet NewSet;
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
            NewSet = new OutputSet(this, setname, vertices);
            Sets.Add(NewSet);
            SetsDictionary.Add(setname, NewSet);
            return NewSet;
        }
        else throw new System.ArgumentException("Erro on AddSet to " + Name + ". A set with the name " + setname + " already exist.");
    }
    public OutputSet RemoveSet(string name)
    {
        OutputSet Set;
        if (SetsDictionary.ContainsKey(name))
        {
            Set = SetsDictionary[name];
            SetsDictionary.Remove(Set.Name);
            Sets.Remove(Set);
            return Set;
        }
        else throw new System.ArgumentException("Erro in RemoveSet on " + Name + ": The set name " + name + " do no exist in the domain.");
    }
    public OutputSet GetSet(string name)
    {
        if (SetsDictionary.ContainsKey(name))
        {
            return SetsDictionary[name];
        }
        else throw new System.ArgumentException("Erro in GetSet on " + Name + ": There is no set with the name " + name);
    }
    public void AddOutput(FuzzyValue output)
    {
        OutputList.Add(output);
    }
    public void ClearOutputList()
    {
        OutputList.Clear();
    }
    public float Defuzzyfication()
    {
        float DefuzzedValue = float.PositiveInfinity;

        if (Sets.Count == 0)
        {
            throw new System.InvalidOperationException("Erro on defuzzing " + Name + ". There is no sets in this domain.");
        }
        else if (OutputList.Count == 0)
        {
            return float.NegativeInfinity;
        }
        else
        {
            float A = 0, B = 0;
            CenterOfArea CenterOfArea;
            foreach (FuzzyValue SetValue in OutputList)
            {
                SetValue.Set.SetXbyY(SetValue.Value);
                CenterOfArea = SetValue.Set.GetCenterOfArea();
                A += CenterOfArea.xPosition * CenterOfArea.Mass;
                B += CenterOfArea.Mass;
            }
            DefuzzedValue = A / B;
            return DefuzzedValue;
        }
    }
}


