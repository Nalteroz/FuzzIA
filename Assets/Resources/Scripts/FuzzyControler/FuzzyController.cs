using System.Collections;
using System.Collections.Generic;

public class FuzzyController
{
    public List<InputDomain> ImputDomainsList { get; private set; }
    public List<OutputDomain> OutputDomainsList { get; private set; }
    public List<FuzzyRule> RulesList { get; private set; }
    public Dictionary<string, InputDomain> ImputDomainsDictionary { get; private set; }
    public Dictionary<string, OutputDomain> OutputDomainsDictionary { get; private set; }


    public FuzzyController()
    {
        ImputDomainsDictionary = new Dictionary<string, InputDomain>();
        OutputDomainsDictionary = new Dictionary<string, OutputDomain>();
        ImputDomainsList = new List<InputDomain>();
        OutputDomainsList = new List<OutputDomain>();
        RulesList = new List<FuzzyRule>();
    }

    public InputDomain AddImputDomain(string name, Range range)
    {
        InputDomain NewDomain;
        string LowName = name.ToLower();
        if (ImputDomainsDictionary.ContainsKey(LowName))
        {
            throw new System.ArgumentException("Erro on creation of ImputDomain. Already exist a domain with the name " + name);
        }
        else
        {
            NewDomain = new InputDomain(this, LowName, range);
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
        InputDomain Domain;
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
    public void RemoveImputDomain(InputDomain domain)
    {
        InputDomain Domain;
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
    public FuzzyRule AddRule(List<RuleParameter> conditions, string operation, OutputSet output)
    {
        FuzzyRule NewRule = new FuzzyRule(conditions, operation, output);
        RulesList.Add(NewRule);
        return NewRule;
    }
    public void AddRule(FuzzyRule rule)
    {
        RulesList.Add(rule);
    }
    public void RemoveRule(FuzzyRule rule)
    {
        RulesList.Remove(rule);
    }
    public void ClearAllRules()
    {
        RulesList.Clear();
    }
    public void FulfillAllRules()
    {
        foreach (OutputDomain domain in OutputDomainsList)
        {
            domain.ClearOutputList();
        }
        foreach (FuzzyRule rule in RulesList)
        {
            rule.FulfillRule();
        }
    }
}
