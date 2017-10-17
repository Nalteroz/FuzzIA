using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzyDomain Distance = new FuzzyDomain("Distance", 0, 10);
    FuzzyDomain High = new FuzzyDomain("High", 0, 10);
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
        Debug.Log(Distance.str());
        Answer = Distance.GetMembership(4);
        foreach(FuzzyValue Out in Answer)
        {
            Debug.Log("["+Out.Set.Name + ";" + Out.Value.ToString()+"]");
        }
    }
	
}
