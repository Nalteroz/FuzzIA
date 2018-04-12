using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PongBar : MonoBehaviour
{
    public float VelocityScale = 10, yPositionEdge = 5;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    protected void MoveUp()
    {
        if(transform.position.y < yPositionEdge) transform.position += Vector3.up * VelocityScale * Time.deltaTime;
    }
    protected void MoveDown()
    {
        if (transform.position.y > -yPositionEdge) transform.position += Vector3.down * VelocityScale * Time.deltaTime;
    }
}
