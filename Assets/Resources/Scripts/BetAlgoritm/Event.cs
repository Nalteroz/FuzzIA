using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Event
{
    public FuzzyController FuzzyController;
    public List<Possibilitie> Possibilities { get; private set; }

    public Event(FuzzyController controller)
    {
        FuzzyController = controller;
    }

    public void SetPossibilitiesByCombination()
    {
        int DomainCount = FuzzyController.ImputDomainsList.Count, Idx = -1;
        string Representation, output = "";
        double TotalOfCombinations = Math.Pow(2, DomainCount);
        InputDomain Domain;
        List<InputDomain> DomainsList = FuzzyController.ImputDomainsList;
        List<List<Possibilitie>> PossibilitiesList = new List<List<Possibilitie>>();
        List<Possibilitie> CurrentPossibilities;
        List<Possibilitie> FinalPossibilities = new List<Possibilitie>();
        Dictionary<string, List<Possibilitie>> CombinationsDictionary = new Dictionary<string, List<Possibilitie>>();

        for(Binary CombinationCount = 1; (int)CombinationCount < TotalOfCombinations; CombinationCount += 1)
        {
            CombinationCount.Normalize(DomainCount);
            output += ("Combination Count: " + (string)CombinationCount) + "\n"; 
            Idx = CombinationCount.LastBitOn();
            output += ("Idx of LastBitOn: " + Idx.ToString()) + "\n";
            Domain = DomainsList[Idx];
            output += ("Domain name of idx: " + Domain.Name) + "\n";
            Representation = (string)CombinationCount;
            output += ("Representation before change last bit: " + Representation) + "\n";
            Representation = Representation.Remove(Idx, 1);
            Representation = "0" + Representation;
            output += ("Representation after change last bit: " + Representation) + "\n";
            if (CombinationsDictionary.ContainsKey(Representation))
            {
                output += ("Dictionary contains key.") + "\n";
                CurrentPossibilities = GetPossibilities(Domain, CombinationsDictionary[Representation]);
                PossibilitiesList.Add(CurrentPossibilities);
                CombinationsDictionary.Add((string)CombinationCount, CurrentPossibilities);
            }
            else
            {
                output += ("Dictionary dont contains key.") + "\n";
                CurrentPossibilities = GetPossibilities(Domain);
                PossibilitiesList.Add(CurrentPossibilities);
                CombinationsDictionary.Add((string)CombinationCount, CurrentPossibilities);
            }
        }
        foreach(List<Possibilitie> ParcialList in PossibilitiesList)
        {
            FinalPossibilities.AddRange(ParcialList);
        }
        Possibilities = FinalPossibilities;
    }

    public List<Possibilitie> GetPossibilities(InputDomain domain)
    {
        List<Possibilitie> Result = new List<Possibilitie>();
        for(int SetIdx = 0; SetIdx < domain.Sets.Count; SetIdx++)
        {
            Result.Add(new Possibilitie(domain.Sets[SetIdx]));
        }
        return Result;
    }
    public List<Possibilitie> GetPossibilities(InputDomain Domain, List<Possibilitie> Possibilities)
    {
        List<Possibilitie> Result = new List<Possibilitie>();
        for(int SetIdx = 0; SetIdx < Domain.Sets.Count; SetIdx++)
        {
            for(int PossInx = 0; PossInx < Possibilities.Count; PossInx++)
            {
                Result.Add(new Possibilitie(Domain.Sets[SetIdx], Possibilities[PossInx]));
            }
        }
        return Result;
    }
}

public class Possibilitie
{
    public List<RuleParameter> ParametersList { get; private set; }

    public Possibilitie(InputSet set)
    {
        ParametersList = new List<RuleParameter> { new RuleParameter(set) };
    }
    public Possibilitie(InputSet set1, InputSet set2)
    {
        ParametersList = new List<RuleParameter> { new RuleParameter(set1), new RuleParameter(set2) };
    }
    public Possibilitie(InputSet set, Possibilitie possibilitie)
    {
        ParametersList = new List<RuleParameter> { new RuleParameter(set) };
        ParametersList.AddRange(possibilitie.ParametersList);
    }
    public string str()
    {
        string Out = "[";
        foreach(RuleParameter parameter in ParametersList)
        {
            Out += parameter.Set.Name + ", ";
        }
        Out += "]";
        return Out;
    }
    public FuzzyRule MakeRule(string operation, OutputSet output)
    {
        return new FuzzyRule(ParametersList, operation, output);
    }
}

public class Binary
{
    private int Value;
    private string Representation;

    public Binary(int value)
    {
        Value = value;
        Representation = Convert.ToString(value, 2);
    }
    public Binary(string representation)
    {
        Representation = representation;
        Value = Convert.ToInt32(representation, 2);
    }

    public static implicit operator Binary(string b)
    {
        return new Binary(b);
    }
    public static implicit operator Binary(int b)
    {
        return new Binary(b);
    }

    public static explicit operator int(Binary b)
    {
        return b.Value;
    }
    public static explicit operator string(Binary b)
    {
        return b.Representation;
    }

    public static Binary operator +(Binary a, Binary b)
    {
        return new Binary(a.Value + b.Value);
    }
    public static Binary operator +(Binary a, int b)
    {
        return new Binary(a.Value + b);
    }
    public static Binary operator -(Binary a, int b)
    {
        return new Binary(a.Value - b);
    }
    public static Binary operator -(Binary a, Binary b)
    {
        return new Binary(a.Value - b.Value);
    }

    public void Normalize(int nOfBits)
    {
        string emptyspace = "";
        if (nOfBits > Representation.Length)
        {
            for (int i = Representation.Length; i < nOfBits; i++)
                emptyspace += "0";
            Representation = Representation.Insert(0, emptyspace);
        }
    }
    public int LastBitOn()
    {
        for(int i = 0; i < Representation.Length; i++)
        {
            if (Representation[i] == '1') return i;
        }
        return -1;
    }
}
