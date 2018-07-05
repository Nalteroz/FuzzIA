using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMovment : MonoBehaviour
{
    public float Velocity = 4, TunrVelocity = 4;


	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        MoveFoward(1);
	}
    

    public void MoveFoward(int Orientation)
    {
        transform.position += transform.up * Orientation * Velocity * Time.deltaTime;
    }

    public void Turn(int Orientation)
    {
        transform.Rotate(new Vector3(0, 0, 1 * TunrVelocity * Orientation));
    }
}
