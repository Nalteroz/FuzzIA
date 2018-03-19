using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public House HousePointer { get; private set; }
    public float Bankroll { get; private set; }
    public List<Addiction> Addictions { get; private set; }

    private InputDomain DomainPointer;
    
    public Player(House house, int DomainIdx, float bankroll = 1000)
    {
        Bankroll = bankroll;
        HousePointer = house;
        Addictions = new List<Addiction>();
        int TotalAdictionLayers = HousePointer.ControllerPointer.ImputDomainsList[DomainIdx].Sets.Count;
        int AddictionsLeght = HousePointer.EventPointer.Possibilities.Count;
        for (int i = 0; i < TotalAdictionLayers; i++) Addictions.Add(new Addiction(AddictionsLeght));
    }
	
}

public class Addiction
{
    public float[] Tendings { get; private set; }
    public int FavoriteIndex { get; private set; }

    private Random Rnd;

    public Addiction(int NumberOfTendings)
    {
        Tendings = new float[NumberOfTendings];
        Rnd = new Random();
        FavoriteIndex = 0;
        RandonlyFillTendings();
    }
    private void RandonlyFillTendings()
    {
        int FavoriteIdx = 0;
        for(int i = 0; i < Tendings.Length; i++)
        {
            Tendings[i] = (float) Rnd.NextDouble();
            if(Tendings[i] > Tendings[FavoriteIdx]) FavoriteIndex = i;
        }
    }
    public string Str()
    {
        string Out = "[";
        for (int i = 0; i < Tendings.Length; i++) Out += Tendings[i].ToString() + "; ";
        Out += "]\nFavorite: {" + FavoriteIndex.ToString() + " ," + Tendings[FavoriteIndex].ToString() + "}";
        return Out;
    }
}
