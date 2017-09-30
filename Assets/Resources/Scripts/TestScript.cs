using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FuzzyDomain MyFD = new FuzzyDomain("Distance", 0, 10);
    List<FuzzyOutput> Answer;
    

	// Use this for initialization
	void Start ()
    {
        MyFD.AddSet("Close", new float[] { 0, 0, 2, 5});
        MyFD.AddSet("Medium", new float[] { 3, 5, 7 });
        MyFD.AddSet("Far", new float[] { 5, 8, 10, 10 });
        Debug.Log(MyFD.str());
        Answer = MyFD.GetAnswer(4);
        foreach(FuzzyOutput Out in Answer)
        {
            Debug.Log("["+Out.SetName + ";" + Out.SetValue.ToString()+"]");
        }
    }
	
}
