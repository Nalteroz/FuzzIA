using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PongBall : MonoBehaviour
{
    public float VelocityScale = 10, xEdgeLimit = 12;
    public Vector2 Direction = new Vector2(1, 1);
    public Text PlayerTextPoints, OponentTextPoints;

	// Use this for initialization
	void Start ()
    {
        Direction.Normalize();		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (Vector3) Direction * VelocityScale * Time.deltaTime;
        VelocityScale += Time.deltaTime * 0.2f;
        CheckBallOut(transform.position.x);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Direction = Vector2.Reflect(Direction, collision.contacts[0].normal);
    }

    void CheckBallOut(float xPosition)
    {
        float Pontuation;
        if(xPosition > xEdgeLimit)
        {
            Pontuation = float.Parse(PlayerTextPoints.text);
            Pontuation++;
            PlayerTextPoints.text = Pontuation.ToString();
            transform.position = Vector3.zero;
            VelocityScale = 10;
        }
        else if (xPosition < -xEdgeLimit)
        {
            Pontuation = float.Parse(OponentTextPoints.text);
            Pontuation++;
            OponentTextPoints.text = Pontuation.ToString();
            transform.position = Vector3.zero;
            VelocityScale = 10;
        }
    }

}
