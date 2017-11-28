using System.Collections;
using System.Collections.Generic;

public class ImputDomain : FuzzyDomain
{
    private FuzzyController Controller;
    public float X { get; private set; }

    public ImputDomain(FuzzyController controller, string name, Range range) 
        : base(name, range)
    {
        Controller = controller;
        X = float.NegativeInfinity;
    }

    public void SetX(float value)
    {
        if (DomainRange.IsInTheRange(value))
        {
            foreach (FuzzySet set in FuzzySets)
            {
                set.SetX(value);
            }
            SetMembership();
        }
        else throw new System.ArgumentException("Erro in SetX on "+ Name +": The value is not on the range.");
    }
}
