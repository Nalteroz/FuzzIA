using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public float Distance = 2, AngularVelocity = 1;

    float angle = 0;
	// Use this for initialization
	
    public void MoveLeft()
    {
        angle += AngularVelocity;
        GetToPosition(angle);
    }
    public void MoveRight()
    {
        angle -= AngularVelocity;
        GetToPosition(angle);
    }
    void GetToPosition(float angle)
    {
        transform.position =  new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * Distance;
    }
}
