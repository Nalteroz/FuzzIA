using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function
{
    public float a { get; private set; }
    public float b { get; private set; }

    public Function(Vector v1, Vector v2)
    {
        SetCoeficients(v1, v2);
    }

    public void SetCoeficients(Vector v1, Vector v2)
    {
        if (v1.x == v2.x)
        {
            a = 0;
            b = 1;
        }
        else
        {
            a = (v2.y - v1.y) / (v2.x - v1.x);
            b = v1.y - a * v1.x;
        }
    }

    public float CalculeValue(float x)
    {
        return a*x + b;
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
	
}
