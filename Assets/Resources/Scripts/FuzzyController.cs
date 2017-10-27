using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyController
{
    public List<FuzzyDomain> Domains { get; private set; }
    public Dictionary<string, FuzzySet> SetsDictionary { get; private set; }

    public FuzzyController()
    {
        SetsDictionary = new Dictionary<string, FuzzySet>();
    }

    public FuzzyDomain AddDomain(string name, float RangeBegin, float RangeEnd)
    {
        FuzzyDomain NewDomain = new FuzzyDomain(this, name, RangeBegin, RangeEnd);
        Domains.Add(NewDomain);
        return NewDomain;
    }

    public void RemoveDomain(FuzzyDomain domain)
    {
        if (domain == null || !Domains.Exists(d => d == domain))
        {
            throw new System.InvalidOperationException("Erro on remove set: The set not exist!");
        }
        else
        {
            Domains.Remove(domain);
        }
    }

    public void AddSetOnDictionary(FuzzySet set)
    {
        SetsDictionary.Add(set.Name, set);
    }
    public void RemoveSetOfDictionary(FuzzySet set)
    {
        SetsDictionary.Remove(set.Name);
    }
}
