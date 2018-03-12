using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public float Bankroll { get; private set; }
    public List<Addiction> Tendings { get; private set; }
    
    public Player(Event Event, float bankroll)
    {
        
    }
	
}

public class Addiction
{
    public float[] Tendings;
    private Random Rnd;

    public Addiction(int PossibilitiesCount)
    {
        Tendings = new float[PossibilitiesCount];
        Rnd = new Random();
        FillTendings();
    }
    private void FillTendings()
    {
        for(int i = 0; i < Tendings.Length; i++)
        {
            Tendings[i] = (float) Rnd.NextDouble();
        }
    }
    public string Str()
    {
        string Out = "[";
        for (int i = 0; i < Tendings.Length; i++) Out += Tendings[i].ToString() + "; ";
        Out += "]";
        return Out;
    }
}
