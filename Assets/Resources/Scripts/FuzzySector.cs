using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FuzzySector
{
    public string Name;
    private Vector2[] Vertices;
    private Function[] Functions = new Function[2];
    public Types Type{get; private set;}

    public FuzzySector(string Name)
    {
        this.Name = Name;
    }
    public FuzzySector(string Name, Vector2[] Vertices)
    {
        this.Name = Name;
        SetVertices(Vertices);
    }

    public enum Types
    {
        Triangle,
        Trapezium
    };

    public void SetVertices(Vector2[] Vertices)
    {
        if(Vertices.Length <3 || Vertices.Length > 4)
        {
            Debug.LogError("Erro in set vertices. Size of vertex incorrect!");
            return;
        }
        else
        {
            this.Vertices = Vertices;
            if (Vertices.Length == 3)
            {
                Type = Types.Triangle;
                Functions[0] = new Function(Vertices[0], Vertices[1]);
                Functions[1] = new Function(Vertices[1], Vertices[2]);
            }
            else if (Vertices.Length == 4)
            {
                Type = Types.Trapezium;
                Functions[0] = new Function(Vertices[0], Vertices[1]);
                Functions[1] = new Function(Vertices[1], Vertices[2]);
            }
        }
    }

    public float IsInDomain(float value)
    {
        if(Vertices[0].x > value || Vertices[Vertices.Length - 1].x < value)
        {
            return 0;
        }
        else
        {
            if(Type == Types.Triangle)
            {
                if(value <= Vertices[1].x)
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
                if (value <= Vertices[1].x)
                {
                    return Functions[0].CalculeValue(value);
                }
                else if( value >= Vertices[2].x)
                {
                    return Functions[1].CalculeValue(value);
                }
                else
                {
                    return Vertices[2].y;
                }
            }
        }

        return 0;
    }
    
}
