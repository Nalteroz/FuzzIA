using System.Collections.Generic;
using UnityEngine;


public class FuzzySet
{
    public int ID;
    public FuzzyDomain Domain { get; private set; }
    public string Name;
    public Range SetRange { get; private set; }
    public float X { get; private set; }
    private List<Vector> VerticesPoints = new List<Vector>();
    private Function[] Functions = new Function[2];
    public Types Type{get; private set;}
    
    public FuzzySet(FuzzyDomain domain,string name, float[] vertices)
    {
        Domain = domain;
        Name = name;
        X = float.NegativeInfinity;
        SetVertices(vertices);
    }

    public string Str()
    {
        string Out = "";
        Out += "Set name: " + Name + ". Type: " + Type.ToString() + "\n";
        Out += "Vertices: ";
        foreach(Vector v in VerticesPoints)
        {
            Out+= "[" + v.x.ToString() + ";" + v.y.ToString() + "]";
        }
        Out += "\nFunctions: ";
        foreach (Function f in Functions)
        {
            Out += "[" + f.Str() + "]\n";
        }
        Out += "\n";
        return Out;
    }
    public void SetX(float x)
    {
        if(Domain.DomainRange.IsInTheRange(x)) X = x;
        else throw new System.ArgumentException("Erro in SetX on the set "+ Name +": The value is not on the range.");
    }
    public void SetXbyY(float y)
    {
        if(y < 0 || y > 1)
        {
            throw new System.ArgumentException("Erro on SetXbyY on "+Name+". The y is not on the range permited (0/1).");
        }
        else
        {
            if(Functions[0].EdgeFunction) X = Functions[1].CalculeX(y);
            else X = Functions[0].CalculeX(y);

        }
    }
    public enum Types
    {
        Triangle,
        Trapezium
    }
    public void SetVertices(float[] Vertices)
    {
        if(Vertices.Length <3 || Vertices.Length > 4)
        {
            throw new System.InvalidOperationException("Erro on create set: Invalid number of set vertices on " + Name);
        }
        else
        {
            if(Vertices[1] < Vertices[0])
            {
                throw new System.InvalidOperationException("Erro on create set: Invalid values on set vertices of " + Name);
            }
            if (Vertices.Length == 3)
            {
                if (Vertices[2] < Vertices[1])
                {
                    throw new System.InvalidOperationException("Erro on create set: Invalid values on set vertices of " + Name);
                }
                Type = Types.Triangle;
                VerticesPoints.Add(new Vector(Vertices[0], 0));
                VerticesPoints.Add(new Vector(Vertices[1], 1));
                VerticesPoints.Add(new Vector(Vertices[2], 0));
                Functions[0] = new Function(VerticesPoints[0], VerticesPoints[1]);
                Functions[1] = new Function(VerticesPoints[1], VerticesPoints[2]);
            }
            else if (Vertices.Length == 4)
            {
                if (Vertices[2] < Vertices[1] || Vertices[3] < Vertices[2])
                {
                    throw new System.InvalidOperationException("Erro on create set: Invalid values on set vertices of " + Name);
                }
                Type = Types.Trapezium;
                VerticesPoints.Add(new Vector(Vertices[0], 0));
                VerticesPoints.Add(new Vector(Vertices[1], 1));
                VerticesPoints.Add(new Vector(Vertices[2], 1));
                VerticesPoints.Add(new Vector(Vertices[3], 0));
                Functions[0] = new Function(VerticesPoints[0], VerticesPoints[1]);
                Functions[1] = new Function(VerticesPoints[2], VerticesPoints[3]);
            }
            if(SetRange == null) SetRange = new Range(Functions[0].Vector1.x, Functions[1].Vector2.x);
            else SetRange.SetRange(Functions[0].Vector1.x, Functions[1].Vector2.x);
        }
    }
    public float IsInDomain()
    {
        if (VerticesPoints[0].x > X || VerticesPoints[VerticesPoints.Count - 1].x < X)
        {
            return 0;
        }
        else
        {
            if(Type == Types.Triangle)
            {
                if(X <= VerticesPoints[1].x)
                {
                    return Functions[0].CalculeY(X);
                }
                else
                {
                    return Functions[1].CalculeY(X);
                }
            }
            else if(Type == Types.Trapezium)
            {
                if (X <= VerticesPoints[1].x)
                {
                    return Functions[0].CalculeY(X);
                }
                else if( X >= VerticesPoints[2].x)
                {
                    return Functions[1].CalculeY(X);
                }
                else
                {
                    return VerticesPoints[2].y;
                }
            }
        }
        return 0;
    }
    public CenterOfArea GetCenterOfArea()
    {
        CenterOfArea CenterOfArea;
        if (IsInDomain() > 0)
        {
            float x1, x2, h, TotalArea=0, CenterX=0;

            h = IsInDomain();
            if (h == 1)
            {
                CenterX = SetRange.Begin + (SetRange.End - SetRange.Begin) / 2;
                if (Type == Types.Triangle)
                {
                    x1 = SetRange.End - SetRange.Begin;
                    TotalArea = x1 / 2;
                }
                else if (Type == Types.Trapezium)
                {
                    x1 = VerticesPoints[1].x;
                    x2 = VerticesPoints[2].x;
                    TotalArea = (x1 - VerticesPoints[0].x) / 2 + (x2 - x1) + (VerticesPoints[3].x - x2) / 2;
                }
                CenterOfArea = new CenterOfArea(CenterX, TotalArea);
                return CenterOfArea;
            }
            else
            {
                CenterX = SetRange.Begin + (SetRange.End - SetRange.Begin)/2;
                x1 = Functions[0].CalculeX(h);
                x2 = Functions[1].CalculeX(h);
                TotalArea = ( (x1 - SetRange.Begin) * h ) / 2 + (x2 - x1) * h + ( (SetRange.End - x2) * h ) / 2;
                CenterOfArea = new CenterOfArea(CenterX, TotalArea);
                return CenterOfArea;
            }
        }
        else
        {
            CenterOfArea = new CenterOfArea((SetRange.End - SetRange.Begin), 0);
            return CenterOfArea;
        }
    }
}
