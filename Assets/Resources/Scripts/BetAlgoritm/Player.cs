﻿using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public House HousePointer { get; private set; }
    public float[] Wallet { get; private set; }
    public List<List<Addiction>> Addictions { get; private set; }
    public List<List<int>> Recomendation { get; private set; }
    public bool isBroken { get; private set; }

    bool[] EmptyPockets;

    public Player(House house, float walletbankroll = 2000)
    {
        HousePointer = house;
        isBroken = false;
        Wallet = new float[house.ControllerPointer.OutputDomainsList.Count];
        EmptyPockets = new bool[Wallet.Length];
        for (int i = 0; i < Wallet.Length; i++)
        {
            Wallet[i] = walletbankroll;
            EmptyPockets[i] = false;
        }
        Addictions = new List<List<Addiction>>();
        for (int DomIdx = 0; DomIdx < house.ControllerPointer.OutputDomainsList.Count; DomIdx++)
        {
            List<Addiction> DomainList = new List<Addiction>();
            for (int SetIdx = 0; SetIdx < house.ControllerPointer.OutputDomainsList[DomIdx].Sets.Count; SetIdx++)
            {
                DomainList.Add(new Addiction(house.EventPointer.Possibilities.Count));
            }
            Addictions.Add(DomainList);
        }
        SetRecomendation();
    }

    public string Str()
    {
        string Out = "Wallet: [" ;
        for (int i = 0; i < Wallet.Length; i++)
        {
            Out += Wallet[i] + " ";
        }
        Out += "]\nIsBroken: " + isBroken.ToString();
        Out += "\nAddictions:\n";
        for (int i = 0; i < Addictions.Count; i++)
        {
            for (int j = 0; j < Addictions[i].Count; j++)
            { 
                Out += "Adiction ["+ i + " "+ j + "]{" + Addictions[i][j].Str() + "}\n";
            }
        }
        return Out;
    }

    void SetRecomendation()
    {
        Recomendation = new List<List<int>>();
        for (int DomIdx = 0; DomIdx < Addictions.Count; DomIdx++)
        {
            List<int> DomainList = new List<int>();
            for (int SetIdx = 0; SetIdx < Addictions[DomIdx].Count; SetIdx++)
            {
                DomainList.Add(Addictions[DomIdx][SetIdx].FavoriteIndex);
            }
            Recomendation.Add(DomainList);
        }
    }

    public List<Bet> MakeBet()
    {
        List<Bet> Bets = new List<Bet>();
        Bet bet;
        for (int i = 0; i < HousePointer.TurnRecomendations.Count; i++)
        {
            bet = CalculeRecomentationBet(i);
            if(bet != null) Bets.Add(bet);
        }
        return Bets;
    }
    public Bet CalculeRecomentationBet(int recomendationIdx)
    {
        float BetValue = 0;
        for (int DomainIdx = 0; DomainIdx < Addictions.Count; DomainIdx++)
        {
            for (int SetIdx = 0; SetIdx < Addictions[DomainIdx].Count; SetIdx++)
            {
                BetValue += CalculeBetValue(DomainIdx, SetIdx, HousePointer.TurnRecomendations[recomendationIdx][DomainIdx][SetIdx], HousePointer.Odds[DomainIdx][SetIdx]);
            }
        }

        if (BetValue > 0) return new Bet(this, recomendationIdx, BetValue);
        else return null;
    }
    float CalculeBetValue(int DomainIdx, int SetIdx, int PossibilitieIdx, OddList odds)
    {
        float ChanceOfWin = odds.GetChanceOfWin(PossibilitieIdx), CurrentBet = 0, PercentOfBankroll = 0;
        float CurrentAddiction = Addictions[DomainIdx][SetIdx].Tendings[PossibilitieIdx];
        if (CurrentAddiction >= ChanceOfWin && !EmptyPockets[DomainIdx])
        {
            PercentOfBankroll = ((CurrentAddiction * odds.Odds[PossibilitieIdx] - 1) / (odds.Odds[PossibilitieIdx] - 1)); //Critério de kelly.
            CurrentBet = Wallet[DomainIdx] * PercentOfBankroll;
            CurrentBet = Math.Min(Wallet[DomainIdx], Math.Max(HousePointer.MinRisk, CurrentBet));
            Wallet[DomainIdx] -= CurrentBet;
            if (Wallet[DomainIdx] <= 0) EmptyPockets[DomainIdx] = true;
            isBroken = CheckIfBroken();
        }
        return CurrentBet;
    }

    bool CheckIfBroken()
    {
        for (int i = 0; i < EmptyPockets.Length; i++)
        {
            if (!EmptyPockets[i]) return false;
        }
        return true;
    }

    public Player NewPlayer()
    {
        return new Player(HousePointer);
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
