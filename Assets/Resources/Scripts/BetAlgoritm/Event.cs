using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        List<Possibilitie> CurrentPossibilities = new List<Possibilitie>();

        
    }

    public List<Possibilitie> GetCombinations(InputDomain Domain1, InputDomain Domain2)
    {
        List<Possibilitie> Result = new List<Possibilitie>();
        for(int Set1Idx = 0; Set1Idx < Domain1.Sets.Count; Set1Idx++)
        {
            for(int Set2Idx = 0; Set2Idx < Domain2.Sets.Count; Set2Idx++)
            {
                Result.Add(new Possibilitie(Domain1.Sets[Set1Idx], Domain2.Sets[Set2Idx]));
            }
        }
        return Result;
    }
    public List<Possibilitie> GetCombinations(InputDomain Domain, List<Possibilitie> Possibilities)
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
