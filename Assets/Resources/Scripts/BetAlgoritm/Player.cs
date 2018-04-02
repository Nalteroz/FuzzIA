using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public House HousePointer { get; private set; }
    public float Bankroll { get; private set; }
    public Addiction[] Addictions { get; private set; }
    public bool isBroken { get; private set; }

    OutputDomain DomainPointer;

    public Player(House house, OutputDomain domain, float bankroll = 3000)
    {
        HousePointer = house;
        isBroken = false;
        DomainPointer = domain;
        Bankroll = bankroll;
        Addictions = new Addiction[DomainPointer.Sets.Count];
        int TotalAdictionLayers = DomainPointer.Sets.Count;
        int AddictionsLeght = HousePointer.EventPointer.Possibilities.Count;
        for (int i = 0; i < Addictions.Length; i++)
        {
            Addictions[i] = new Addiction(AddictionsLeght);
        }
    }

    public string Str()
    {
        string Out = "Bankroll: " + Bankroll.ToString();
        Out += " - isBroken: " + isBroken.ToString();
        Out += "\n Addictions:\n";
        for (int i = 0; i < Addictions.Length; i++) Out += "Index: " + i + ":"+ Addictions[i].Str();
        return Out;
    }

    public int Recomend(int setidx)
    {
        return Addictions[setidx].FavoriteIndex;
    }

    public List<Bet> MakeBets(List<int> SetRecomendations, int OutSetIdx, OddList SetOdds)
    {
        List<Bet> Bets = new List<Bet>();
        Bet bet;
        foreach (int recomendation in SetRecomendations)
        {
            bet = MakeBet(recomendation, OutSetIdx, SetOdds);
            if(bet!=null) Bets.Add(bet);
        }
        return Bets;
    }
    public Bet MakeBet(int PossibilitieIdx, int OutSetIdx, OddList odds)
    {
        float ChanceOfWin = odds.GetChanceOfWin(PossibilitieIdx), CurrentBet = 0, PercentOfBankroll = 0;
        float CurrentAddiction = Addictions[OutSetIdx].Tendings[PossibilitieIdx];
        if (CurrentAddiction >= ChanceOfWin && !isBroken)
        {
            PercentOfBankroll = ((CurrentAddiction * odds.Odds[PossibilitieIdx] - 1) / (odds.Odds[PossibilitieIdx] - 1)); //Critério de kelly.
            CurrentBet = Bankroll * PercentOfBankroll;
            CurrentBet = Math.Min(Bankroll, Math.Max(HousePointer.MinRisk, CurrentBet));
            Bankroll -= CurrentBet;
            if (Bankroll <= 0) isBroken = true;
        }
        if (CurrentBet > 0)
        {
            return new Bet(this, PossibilitieIdx, CurrentBet);
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

    static Random Rnd = new Random();

    public Addiction(int NumberOfTendings)
    {
        Tendings = new float[NumberOfTendings];
        FavoriteIndex = 0;
        for (int i = 0; i < Tendings.Length; i++)
        {
            Tendings[i] = (float)Rnd.NextDouble();
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
        Out += "]Favorite: {Index:" + FavoriteIndex.ToString() + ", Value: " + Tendings[FavoriteIndex].ToString() + "}\n";
        return Out;
    }
}
