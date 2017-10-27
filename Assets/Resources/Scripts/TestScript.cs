using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzyController Controller = new FuzzyController();
    FuzzyDomain Distance = new FuzzyDomain("Distance", 0, 10);
    FuzzyDomain High = new FuzzyDomain("High", 0, 10);
    FuzzyDomain Action = new FuzzyDomain("Action", 0, 1);
    FuzzyRule Rule;

    List<FuzzyValue> Answer;
    

	// Use this for initialization
	void Start ()
    {
        Distance.AddSet("Close", new float[] { 0, 0, 2, 5});
        Distance.AddSet("Medium", new float[] { 3, 5, 7 });
        Distance.AddSet("Far", new float[] { 5, 8, 10, 10 });
        High.AddSet("Low", new float[] { 0, 0, 2, 5 });
        High.AddSet("Average", new float[] { 3, 5, 7 });
        High.AddSet("High", new float[] { 5, 8, 10, 10 });
        Action.AddSet("Shoot", new float[] { 0, 0, 1 });
        Action.AddSet("Run", new float[] { 0, 1, 1 });
        Debug.Log(Distance.str());
        Distance.SetValue(4);
        High.SetValue(6);
        

    }
	
}
