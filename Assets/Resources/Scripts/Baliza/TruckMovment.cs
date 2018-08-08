using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovment : MonoBehaviour
{
    public float Velocity = 4, TurnVelocity = 4;
    Vector2 TurnVector = Vector2.up;


	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        MoveFoward(1);
	}
    
    public enum Orientation { Left, Right}

    public void MoveFoward(int Orientation)
    {
        transform.position += transform.up * Orientation * Velocity * Time.deltaTime;

    }

    public void Turn(Orientation Direction)
    {
        if(Direction == Orientation.Left)
        {
            TurnVector = RotateVector(TurnVector, TurnVelocity);
        }
        else if (Direction == Orientation.Right)
        {
            TurnVector = - RotateVector(TurnVector, TurnVelocity);
        }
            
    }

    Vector2 RotateVector(Vector2 v, float angle)
    {
        return new Vector2((Mathf.Cos(angle) * v.x - Mathf.Sin(angle) * v.y), (Mathf.Sin(angle) * v.x + Mathf.Cos(angle) * v.y));
    }
    
}
