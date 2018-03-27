using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public House HousePointer { get; private set; }
    public float Bankroll { get; private set; }
    public List<Addiction> Addictions { get; private set; }
    public bool isBroken { get; private set; }

    private OutputDomain DomainPointer;
    
    public Player(House house, OutputDomain domain, float bankroll = 1000)
    {
        HousePointer = house;
        isBroken = false;
        DomainPointer = domain;
        Bankroll = bankroll;
        Addictions = new List<Addiction>();
        int TotalAdictionLayers = DomainPointer.Sets.Count;
        int AddictionsLeght = HousePointer.EventPointer.Possibilities.Count;
        for (int i = 0; i < TotalAdictionLayers; i++)
        {
            Addictions.Add(new Addiction(AddictionsLeght));
        }
    }

    public string Str()
    {
        string Out = "Bankroll: " + Bankroll.ToString();
        Out += " - isBroken: " + isBroken.ToString();
        Out += "\n Addictions:\n";
        for (int i = 0; i < Addictions.Count; i++) Out += "Index: " + i + ":"+ Addictions[i].Str();
        return Out;
    }

    public List<int> Recomend()
    {
        List<int> Recomendations = new List<int>();
        foreach (Addiction adct in Addictions)
        {
            Recomendations.Add(adct.FavoriteIndex);
        }
        return Recomendations;
    }
    public List<Bet> MakeBet(int PossibilitieIdx, List<OddList> odds)
    {
        List<Bet> Bets = new List<Bet>();
        for (int i = 0; i < DomainPointer.Sets.Count; i++)
        {
            Bets.Add(MakeBet(PossibilitieIdx, i, odds[i]));
        }
        return Bets;
    }
    public Bet MakeBet(int PossibilitieIdx, int OutSetIdx, OddList odds)
    {
        float ChanceOfWin = odds.GetChanceOfWin(PossibilitieIdx), Bet = 0;
        float CurrentAddiction = Addictions[OutSetIdx].Tendings[PossibilitieIdx];
        if (ChanceOfWin >= CurrentAddiction && !isBroken)
        {
            Bet = (CurrentAddiction * odds.Odds[PossibilitieIdx] - 1) / (odds.Odds[PossibilitieIdx] - 1);
            Bet = Math.Min(Bankroll, Math.Max(HousePointer.MinRisk, Bet));
            Bankroll -= Bet;
            if (Bankroll <= 0) isBroken = true;
        }
        if (Bet > 0)
        {
            return new Bet(this, DomainPointer.Sets[OutSetIdx], PossibilitieIdx, Bet);
        }
        else return null;
    }

    public Player NewPlayer()
    {
        return new Player(HousePointer, DomainPointer);
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
        RandonlyFillTendings();
    }
    private void RandonlyFillTendings()
    {
        FavoriteIndex = 0;
        for(int i = 0; i < Tendings.Length; i++)
        {
            Tendings[i] = (float) Rnd.NextDouble();
            if (Tendings[i] > Tendings[FavoriteIndex])
            {
                FavoriteIndex = i;
            }
        }
    }
    public string Str()
    {
        string Out = "[";
        for (int i = 0; i < Tendings.Length; i++) Out += Tendings[i].ToString() + "; ";
        Out += "]\nFavorite: {Index:" + FavoriteIndex.ToString() + ", Value: " + Tendings[FavoriteIndex].ToString() + "}\n";
        return Out;
    }
}
