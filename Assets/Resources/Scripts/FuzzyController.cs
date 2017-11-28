using System.Collections;
using System.Collections.Generic;

public class FuzzyController
{
    private List<ImputDomain> ImputDomainsList;
    private List<OutputDomain> OutputDomainsList;
    private List<FuzzyRule> RulesList;
    public Dictionary<string, ImputDomain> ImputDomainsDictionary { get; private set; }
    public Dictionary<string, OutputDomain> OutputDomainsDictionary { get; private set; }


    public FuzzyController()
    {
        ImputDomainsDictionary = new Dictionary<string, ImputDomain>();
        OutputDomainsDictionary = new Dictionary<string, OutputDomain>();
        ImputDomainsList = new List<ImputDomain>();
        OutputDomainsList = new List<OutputDomain>();
        RulesList = new List<FuzzyRule>();
    }

    public ImputDomain AddImputDomain(string name, Range range)
    {
        ImputDomain NewDomain;
        string LowName = name.ToLower();
        if (ImputDomainsDictionary.ContainsKey(LowName))
        {
            throw new System.ArgumentException("Erro on creation of ImputDomain. Already exist a domain with the name " + name);
        }
        else
        {
            NewDomain = new ImputDomain(this, LowName, range);
            ImputDomainsList.Add(NewDomain);
            ImputDomainsDictionary.Add(LowName, NewDomain);
        }
        return NewDomain;
    }
    public OutputDomain AddOutputDomain(string name, Range range)
    {
        OutputDomain NewDomain;
        string LowName = name.ToLower();
        if (OutputDomainsDictionary.ContainsKey(LowName))
        {
            throw new System.ArgumentException("Erro on creation of ImputDomain. Already exist a domain with the name " + name);
        }
        else
        {
            NewDomain = new OutputDomain(this, LowName, range);
            OutputDomainsList.Add(NewDomain);
            OutputDomainsDictionary.Add(LowName, NewDomain);
        }
        return NewDomain;
    }
    public void RemoveImputDomain(string name)
    {
        ImputDomain Domain;
        if (ImputDomainsDictionary.ContainsKey(name))
        {
            Domain = ImputDomainsDictionary[name];
            ImputDomainsDictionary.Remove(name);
            ImputDomainsList.Remove(Domain);
        }
        else
        {
            throw new System.ArgumentNullException("Erro on RemoveImputDomain: The domain name "+ name +" not exist!");
        }
    }
    public void RemoveImputDomain(ImputDomain domain)
    {
        ImputDomain Domain;
        if (ImputDomainsDictionary.ContainsKey(domain.Name))
        {
            Domain = ImputDomainsDictionary[domain.Name];
            ImputDomainsDictionary.Remove(Domain.Name);
            ImputDomainsList.Remove(Domain);
        }
        else
        {
            throw new System.ArgumentNullException("Erro on RemoveImputDomain: The domain name " + domain.Name + " not exist!");
        }
    }
    public void RemoveOutputDomain(string name)
    {
        OutputDomain Domain;
        if (OutputDomainsDictionary.ContainsKey(name))
        {
            Domain = OutputDomainsDictionary[name];
            OutputDomainsDictionary.Remove(name);
            OutputDomainsList.Remove(Domain);
        }
        else
        {
            throw new System.ArgumentNullException("Erro on RemoveImputDomain: The domain name " + name + " not exist!");
        }
    }
    public void RemoveOutputDomain(OutputDomain domain)
    {
        OutputDomain Domain;
        if (OutputDomainsDictionary.ContainsKey(domain.Name))
        {
            Domain = OutputDomainsDictionary[domain.Name];
            OutputDomainsDictionary.Remove(Domain.Name);
            OutputDomainsList.Remove(Domain);
        }
        else
        {
            throw new System.ArgumentNullException("Erro on RemoveOutputDomain: The domain name " + domain.Name + " not exist!");
        }
    }
    public FuzzyRule AddRule(string sentence)
    {
        FuzzyRule NewRule = new FuzzyRule(this, sentence);
        RulesList.Add(NewRule);
        return NewRule;
    }
    public void FulfillAllRules()
    {
        foreach (FuzzyRule rule in RulesList)
        {
            rule.FulfillRule();
        }
    }
}
