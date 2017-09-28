using System.Collections.Generic;
using UnityEngine;


public class FuzzySet
{
    public string Name;
    private List<Vector> VerticesPoints = new List<Vector>();
    private Function[] Functions = new Function[2];
    public Types Type{get; private set;}
    
    public FuzzySet(string Name, float[] Vertices)
    {
        this.Name = Name;
        SetVertices(Vertices);
    }

    public string str()
    {
        string Out = "";
        Out += "Set name: " + Name + ". Type: " + Type.ToString() + "\n";
        Out += "Vertices: ";
        foreach(Vector v in VerticesPoints)
        {
            Out+= "[" + v.x.ToString() + ";" + v.y.ToString() + "]";
        }
        Out += "\n";
        return Out;
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
            Debug.LogError("Erro in set vertices. Size of vertex incorrect!");
            return;
        }
        else
        {
            if (Vertices.Length == 3)
            {
                Type = Types.Triangle;
                VerticesPoints.Add(new Vector(Vertices[0], 0));
                VerticesPoints.Add(new Vector(Vertices[1], 1));
                VerticesPoints.Add(new Vector(Vertices[2], 0));
                Functions[0] = new Function(VerticesPoints[0], VerticesPoints[1]);
                Functions[1] = new Function(VerticesPoints[1], VerticesPoints[2]);
            }
            else if (Vertices.Length == 4)
            {
                Type = Types.Trapezium;
                VerticesPoints.Add(new Vector(Vertices[0], 0));
                VerticesPoints.Add(new Vector(Vertices[1], 1));
                VerticesPoints.Add(new Vector(Vertices[2], 1));
                VerticesPoints.Add(new Vector(Vertices[3], 0));
                Functions[0] = new Function(VerticesPoints[0], VerticesPoints[1]);
                Functions[1] = new Function(VerticesPoints[2], VerticesPoints[3]);
            }
        }
    }

    public float IsInDomain(float value)
    {
        if (VerticesPoints[0].x > value || VerticesPoints[VerticesPoints.Count - 1].x < value)
        {
            return 0;
        }
        else
        {
            if(Type == Types.Triangle)
            {
                if(value <= VerticesPoints[1].x)
                {
                    return Functions[0].CalculeValue(value);
                }
                else
                {
                    return Functions[1].CalculeValue(value);
                }
            }
            else if(Type == Types.Trapezium)
            {
                if (value <= VerticesPoints[1].x)
                {
                    return Functions[0].CalculeValue(value);
                }
                else if( value >= VerticesPoints[2].x)
                {
                    return Functions[1].CalculeValue(value);
                }
                else
                {
                    return VerticesPoints[2].y;
                }
            }
        }
        return 0;
    }
    
}
