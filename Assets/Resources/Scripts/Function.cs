using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function
{
    public float a { get; private set; }
    public float b { get; private set; }

    public Function(Vector2 v1, Vector2 v2)
    {
        SetCoeficients(v1, v2);
    }

    public void SetCoeficients(Vector2 v1, Vector2 v2)
    {
        a = (v2.y - v1.y) / (v2.x - v1.x);
        b = v1.y - a * v1.x;
    }

    public float CalculeValue(float x)
    {
        return a*x + b;
    }
}
