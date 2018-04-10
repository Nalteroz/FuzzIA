using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    FuzzyController controller = new FuzzyController();
    InputDomain indomain1, indomain2;
    OutputDomain outdomain;
    OutputSet outset;
    Game game;
	// Use this for initialization
	void Start ()
    {
        indomain1 = controller.AddImputDomain("p1", new Range(-1, 1));
        indomain1.AddSet("errado1", new float[] { -1, -1, 0});
        indomain1.AddSet("certo1", new float[] { 0, 1, 1 });
        indomain1.SetX(1f);
        indomain1 = controller.AddImputDomain("p2", new Range(-1, 1));
        indomain1.AddSet("errado2", new float[] { -1, -1, 0 });
        indomain1.AddSet("certo2  ", new float[] { 0, 1, 1 });
        indomain1.SetX(1f);
        //indomain = controller.AddImputDomain("C", new Range(-1, 1));
        //indomain.AddSet("c", new float[] { -1, -1, 0 });
        outdomain = controller.AddOutputDomain("R", new Range(-1, 1));
        outset = outdomain.AddSet("certo", new float[] { 0, 1, 1 });
        outset = outdomain.AddSet("errado", new float[] { -1, -1, 0 });
        game = new Game(controller, 10, Avaliation);
        game.RunCompleteGame();
        Debug.Log(game.Event.Str());
        Debug.Log(game.GameHouse.Str());



    }

    float Avaliation()
    {
        controller.FulfillAllRules();
        return outdomain.Defuzzyfication();
    }
	
}
