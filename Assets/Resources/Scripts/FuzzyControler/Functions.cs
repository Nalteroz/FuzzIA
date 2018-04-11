public class Function
{
    public bool EdgeFunction { get; private set; }
    public Vector Vector1 { get; private set; }
    public Vector Vector2 { get; private set; }
    public float a { get; private set; }
    public float b { get; private set; }

    public Function(Vector v1, Vector v2)
    {
        Vector1 = v1;
        Vector2 = v2;
        SetCoeficients(v1, v2);
    }

    public string Str()
    {
        string Out = "";
        Out = "y = " + a.ToString() + "*x + " + b.ToString();
        return Out;
    }
    public void SetCoeficients(Vector v1, Vector v2)
    {
        if (v1.x == v2.x)
        {
            a = 0;
            b = 1;
            EdgeFunction = true;
        }
        else
        {
            a = (v2.y - v1.y) / (v2.x - v1.x);
            b = v1.y - a * v1.x;
            EdgeFunction = false;
        }
    }
    public float CalculeY(float x)
    {
        return a*x + b;
    }
    public float CalculeX(float y)
    {
        if (a == 0 && b == 1) return Vector1.x;
        else if (y >= -1e-6f && y <= 1) return (y - b) / a;
        else throw new System.ArgumentException("Erro in CalculeX of Function: The y is out of the permited range [0,1]");
    }
}

public class Vector
{
    public float x;
    public float y;
  
    public Vector(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

    public string Str()
    {
        return "[" + x.ToString() + "," + y.ToString() + "]";
    }
	
}

public class Range
{
    public float Begin { get; private set; }
    public float End { get; private set; }


    public Range(float begin, float end)
    {
        SetRange(begin, end);
    }

    public string Str()
    {
        string Out = "";
        Out += "[" + Begin + ", " + End + "]";
        return Out;
    }
    public void SetRange(float begin, float end)
    {
        if (begin == end)
            throw new System.ArgumentException("Erro on create range: Range value invalid.");
        else if(begin > end)
        {
            End = begin;
            Begin = end;
        }
        else
        {
            Begin = begin;
            End = end;
        }
    }
    public bool IsInTheRange(float value)
    {
        if (value >= Begin && value <= End) return true;
        else return false;
    }
    public bool IsInTheRange(Range range)
    {
        if (range.Begin >= Begin && range.End <= End) return true;
        else return false;
    }
}

public class FuzzyValue
{
    public FuzzySet Set;
    public float Value;

    public FuzzyValue(FuzzySet set, float value = 0)
    {
        Set = set;
        Value = value;
    }

    public string Str()
    {
        string Out = "";
        Out += "Set:\n";
        Out += Set.Str();
        Out += "Value: " + Value.ToString() + "\n";
        return Out;
    }
}

public class CenterOfArea
{
    public float xPosition;
    public float Mass;

    public CenterOfArea(float x, float mass)
    {
        xPosition = x;
        Mass = mass;
    }

    public string Str()
    {
        string Out = "";
        Out += "Position x: " + xPosition.ToString() + " ;Mass: " + Mass.ToString() + "\n";
        return Out;
    }
}