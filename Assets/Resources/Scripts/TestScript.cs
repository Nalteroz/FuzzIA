using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class TestScript : MonoBehaviour
{
    FuzzyDomain MyFD = new FuzzyDomain("Distance", 0, 10);
    List<FuzzyOutput> Answer;
    

	// Use this for initialization
	void Start ()
    {
        MyFD.AddFuzzySet("Close", new float[] { 0, 0, 2, 5});
        MyFD.AddFuzzySet("Medium", new float[] { 3, 5, 7 });
        MyFD.AddFuzzySet("Far", new float[] { 5, 8, 10, 10 });
        Debug.Log(MyFD.str());
        Answer = MyFD.GetAnswer(4);
        foreach(FuzzyOutput Out in Answer)
        {
            Debug.Log("["+Out.SetName + ";" + Out.SetValue.ToString()+"]");
        }

	}
=======
using System;

public class TestScript : MonoBehaviour
{
    Binary a = 0, b = "10";
	// Use this for initialization
	void Start ()
    {
        
        Debug.Log((string)a);
        Debug.Log((string)b);
        Debug.Log((string)(a-b));
    }
>>>>>>> 29e3aca... Criação do Evento iniciado
	
}
