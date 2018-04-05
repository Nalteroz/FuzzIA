using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Event
{
    public FuzzyController ControllerPointer { get; private set; }
    public List<Possibilitie> Possibilities { get; private set; }

    List<List<FuzzyRule>> RecomendationRules;

    public Event(FuzzyController controller)
    {
        ControllerPointer = controller;
        SetPossibilitiesByCombination();
    }

    public string Str()
    {
        string Out = "Total of Possibilities: " + Possibilities.Count.ToString();
        Out += "\nPossibilities:"+"\n";
        foreach(Possibilitie poss in Possibilities)
        {
            Out += poss.str();
        }
        if (RecomendationRules != null)
        {
            Out += "\nRecomendations rules:\n";
            for (int PlayerRecIdx = 0; PlayerRecIdx < RecomendationRules.Count; PlayerRecIdx++)
            {
                Out += "Player " + PlayerRecIdx + ":\n";
                for (int RuleIdx = 0; RuleIdx < RecomendationRules[PlayerRecIdx].Count; RuleIdx++)
                {
                    Out += RecomendationRules[PlayerRecIdx][RuleIdx].Str();
                }
            }
        }
        return Out;
    }
    public void SetPossibilitiesByCombination()
    {
        int DomainCount = ControllerPointer.ImputDomainsList.Count, Idx = -1, TotalOfCombinations;
        string Representation;
        InputDomain Domain;
        List<InputDomain> DomainsList = ControllerPointer.ImputDomainsList;
        List<List<Possibilitie>> PossibilitiesList = new List<List<Possibilitie>>();
        List<Possibilitie> CurrentPossibilities;
        List<Possibilitie> FinalPossibilities = new List<Possibilitie>();
        Dictionary<string, List<Possibilitie>> CombinationsDictionary = new Dictionary<string, List<Possibilitie>>();
        
        TotalOfCombinations = (int)Math.Pow(2, DomainCount) - 1;
        FinalPossibilities.Add(new Possibilitie());
        for (Binary CombinationCount = 1; (int)CombinationCount <= TotalOfCombinations; CombinationCount += 1)
        {
            CombinationCount.Normalize(DomainCount);
            Idx = CombinationCount.LastBitOn();
            Domain = DomainsList[Idx];
            Representation = (string)CombinationCount;
            Representation = Representation.Remove(Idx, 1);
            Representation = "0" + Representation;
            if (CombinationsDictionary.ContainsKey(Representation))
            {
                CurrentPossibilities = GetPossibilities(Domain, CombinationsDictionary[Representation]);
                PossibilitiesList.Add(CurrentPossibilities);
                CombinationsDictionary.Add((string)CombinationCount, CurrentPossibilities);
            }
            else
            {
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

    public void GetRecomendationsRules(List<List<List<int>>> Recomendations)
    {
        RecomendationRules = new List<List<FuzzyRule>>();
        for (int PlayerRecIdx = 0; PlayerRecIdx < Recomendations.Count; PlayerRecIdx++)
        {
            List<FuzzyRule> PlayerRules = new List<FuzzyRule>();
            for (int DomainIdx = 0; DomainIdx < Recomendations[PlayerRecIdx].Count; DomainIdx++)
            {
                for (int SetIdx = 0; SetIdx <  Recomendations[PlayerRecIdx][DomainIdx].Count; SetIdx++)
                {
                    PlayerRules.Add(GetRule(Recomendations[PlayerRecIdx][DomainIdx][SetIdx], ControllerPointer.OutputDomainsList[DomainIdx].Sets[SetIdx]));
                }
            }
            RecomendationRules.Add(PlayerRules);
        }
    }
    public FuzzyRule GetRule(int PossibilitieIdx, OutputSet outputset)
    {
        FuzzyRule Rule = new FuzzyRule(Possibilities[PossibilitieIdx].ParametersList, "and", outputset);
        return Rule;
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

    public Possibilitie()
    {
        ParametersList = new List<RuleParameter> ();
    }
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
        if (ParametersList != null) return new FuzzyRule(ParametersList, operation, output);
        else return null;
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

public class RandomPickIndex
{
    public List<int> PickList { get; private set; }
    private System.Random Rnd;

    public RandomPickIndex(int IdxCount)
    {
        Rnd = new System.Random();
        PickList = new List<int>(IdxCount);
        for (int i = 0; i < IdxCount; i++) PickList.Add(i);
        ShuffleList(PickList);
    }

    public void ShuffleList(List<int> list)
    {
        for (int idx = 0; idx < list.Count; idx++)
        {
            int RndIdx = Rnd.Next(list.Count);
            while (idx == RndIdx) RndIdx = Rnd.Next(list.Count);
            Swap(list, idx, RndIdx);
        }
    }
    private void Swap(List<int> list, int a, int b)
    {
        int t = list[a];
        list[a] = list[b];
        list[b] = t;
    }
    public int GetNext()
    {
        if (PickList.Count > 0)
        {
            int Idx = PickList[0];
            PickList.RemoveAt(0);
            return Idx;
        }
        else return -1;
    }
    public string Str()
    {
        string Out = "[";
        for (int i = 0; i < PickList.Count; i++) Out += PickList[i].ToString() + "; ";
        Out += "]";
        return Out;
    }
}
