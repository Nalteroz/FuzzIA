using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryDodger : PlayerMovment
{
    public float SightDistance = 1;

    FuzzyController FzCtrl;
    InputDomain Input1, Input2;
    OutputDomain Output;
    Game GameCtlr;

    float Defused, NullValue = float.NegativeInfinity;
    GameObject AimBullet;

    void Start ()
    {
        FzCtrl = new FuzzyController();
        Input1 = FzCtrl.AddInputDomain("Distance", new Range(0, Distance));
        Input2 = FzCtrl.AddInputDomain("Orientation", new Range(-1, 1));
        Output = FzCtrl.AddOutputDomain("Move", new Range(-1, 1));

        Input1.AddSet("Close", new float[] {0, SightDistance / 4, SightDistance / 2});
        Input1.AddSet("Middle", new float[] { SightDistance / 4, SightDistance / 2, (3* SightDistance) /4 });
        Input1.AddSet("Far", new float[] { SightDistance / 2, (3* SightDistance) /4, SightDistance });

        Input2.AddSet("Right", new float[] { -1, -1, 0});
        Input2.AddSet("Mid", new float[] { -0.5f, -0, 0.5f });
        Input2.AddSet("Left", new float[] { 0, 1, 1});

        Output.AddSet("Left", new float[] { -1, -1, 0});
        Output.AddSet("Right", new float[] { 0, 1, 1});

    }
	
	void Update ()
    {

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        AimBullet = collision.gameObject;
    }

    float CheckDistance(GameObject Bullet)
    {
        return (transform.position - Bullet.transform.position).magnitude;
    }
}
