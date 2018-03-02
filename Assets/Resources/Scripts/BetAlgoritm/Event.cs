using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Event
{
    public FuzzyController FuzzyController;
    public List<FuzzyRule> Possibilities { get; private set; }

    public Event(FuzzyController controller)
    {
        FuzzyController = controller;
    }

    public void SetPossibilitiesByCombination()
    {
        float DomainCount = FuzzyController.ImputDomainsList.Count;
        List<InputDomain> DomainsList = FuzzyController.ImputDomainsList;
        List<List<Possibilitie>> CurrentPossibilities = new List<List<Possibilitie>>();
        Dictionary<string, List<Possibilitie>> CombinationsDictionary = new Dictionary<string, List<Possibilitie>>();
        for(int i = 0; i < DomainCount; i++)
        {
            CurrentPossibilities.Add(GetPossibilities(DomainsList[i]));
        }
    }
    public int nOfCombinations(int n)
    {
        return 2 ^ n;
    }
    public int Factorial(int n)
    {
        if (n == 0) return 1;
        else if (n > 0) return n * Factorial(n - 1);
        else throw new System.ArgumentException("Erro in event: Invalid number on factorial.");
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
}
