using System;
using System.Collections.Generic;
using UnityEngine;

public class House
{
    public Event EventPointer { get; private set; }
    public FuzzyController ControllerPointer { get; private set; }
    public List<Player> Players { get; private set; }
    public List<List<OddList>> Odds { get; private set; }
    public float MinRisk { get; private set; }
    public List<List<List<int>>> TurnRecomendations { get; private set; }
    public List<Bet> TurnBets { get; private set; }


    public House(Event Event, int nOfPlayers = 3, int MinimalRisk = 10)
    {
        EventPointer = Event;
        ControllerPointer = Event.ControllerPointer;
        MinRisk = MinimalRisk;
        InitializePlayers(nOfPlayers);
        InitializeOdds();
    }

    public string Str()
    {
        string Out = "N of players: " + Players.Count;
        Out += "\nPlayers:\n";
        for (int i = 0; i < Players.Count; i++)
        {
            Out += "Player " + i +": " + Players[i].Str();   
        }
        Out += "Odds:\n";
        for (int i = 0; i < Odds.Count; i++)
        {
            for (int j = 0; j < Odds[i].Count; j++)
            {
                Out += "\nOdd [" + i + "][" + j + "]" + Odds[i][j].Str();
            }
        }
        if (TurnRecomendations != null)
        {
            Out += "\nRecomendations:";
            for (int i = 0; i < TurnRecomendations.Count; i++)
            {
                Out += "\nRec [" + i + "]{ ";
                for (int j = 0; j < TurnRecomendations[i].Count; j++)
                {
                    Out += "[";
                    for (int k = 0; k < TurnRecomendations[i][j].Count; k++)
                    {
                        Out += TurnRecomendations[i][j][k] + ", ";
                    }
                    Out += "]";
                }
                Out += "}\n";
            }
        }
        if (TurnBets != null)
        {
            Out += "\nBets:";
            for (int i = 0; i < TurnBets.Count; i++)
            {
                Out += TurnBets[i].Str();
            }
        }
        return Out;
    }


    public void InitializePlayers(int nOfPlayers)
    {
        Players = new List<Player>();
        for (int i = 0; i < nOfPlayers; i++)
        {
            Players.Add(new Player(this));
        }
    }
    public void InitializeOdds()
    {
        Odds = new List<List<OddList>>();
        int DomainsCount = ControllerPointer.OutputDomainsList.Count, OddsLenght = EventPointer.Possibilities.Count;
        for(int d = 0; d < DomainsCount; d++)
        {
            int SetsCount = ControllerPointer.OutputDomainsList[d].Sets.Count;
            List<OddList> OddsList = new List<OddList>();
            for(int s = 0; s < SetsCount; s++)
            {
                OddsList.Add(new OddList(OddsLenght));
            }
            Odds.Add(OddsList);
        }
    }
    public void GetRecomendations()
    {
        TurnRecomendations = new List<List<List<int>>>();
        foreach (Player player in Players)
        {
            TurnRecomendations.Add(player.Recomendation);
        }
    }
    public void GetBets()
    {
        TurnBets = new List<Bet>();
        for (int i = 0; i < Players.Count; i++)
        {
            TurnBets.AddRange(Players[i].MakeBet());
        }
    }
    public void PayOdds(EvaluationHandler handler)
    {
        int WinIdx = handler.GetWinnerIdx(), WinnerPossibilitieIdx; ;
        foreach (Bet bet in TurnBets)
        {
            if(bet.RecomentationIdx == WinIdx)
            {
                float Montant = 0;
                for (int DomainIdx = 0; DomainIdx < bet.IndividualValues.Length; DomainIdx++)
                {
                    for (int SetIdx = 0; SetIdx < bet.IndividualValues[DomainIdx].Length; SetIdx++)
                    {
                        WinnerPossibilitieIdx = TurnRecomendations[WinIdx][DomainIdx][SetIdx];
                        Montant += bet.IndividualValues[DomainIdx][SetIdx] * Odds[DomainIdx][SetIdx].Odds[WinnerPossibilitieIdx];
                    }
                }
                bet.Player.RecievePayment(Montant);
            }
        }
        for (int DomainIdx = 0; DomainIdx < TurnRecomendations[WinIdx].Count; DomainIdx++)
        {
            for (int SetIdx = 0; SetIdx < TurnRecomendations[WinIdx][DomainIdx].Count; SetIdx++)
            {
                WinnerPossibilitieIdx = TurnRecomendations[WinIdx][DomainIdx][SetIdx];
                Odds[DomainIdx][SetIdx].CountAWin(WinnerPossibilitieIdx);
            }
        }
    }
    public void RenewPlayers()
    {
        for(int playeridx = 0; playeridx < Players.Count; playeridx++)
        {
            if (Players[playeridx].isBroken) Players[playeridx] = new Player(this);
        }
    }
    
}

public class Bet
{
    public Player Player { get; private set; }
    public int RecomentationIdx { get; private set; }
    public float TotalValue { get; private set; }
    public float[][] IndividualValues;

    public Bet(Player player, int recomentationidx, float totalvalue, float[][] values)
    {
        Player = player;
        RecomentationIdx = recomentationidx;
        TotalValue = totalvalue;
        IndividualValues = values;
    }

    public string Str()
    {
        string Out = "\nPlayer: " + Player.Str();
        Out += "Recomentation index: " + RecomentationIdx;
        Out += "\nBet value: " + TotalValue + "\n";
        Out += "\nIndividual values:\n";
        foreach(float[] domain in IndividualValues)
        {
            Out += "[";
            foreach (float setvalue in domain)
            {
                Out += setvalue + ", ";
            }
            Out += "]\n";
        }
        return Out;
    }
}

public class OddList
{
    public float[] Odds { get; private set; }
    public int[] WinCount { get; private set; }

    private uint TotalOfGames = 0;

    public OddList(int OddsCount)
    {
        InitializeCount(OddsCount);
        Odds = new float[OddsCount];
        SetOdds();
    }

    private void InitializeCount(int size)
    {
        WinCount = new int[size];
        for (int i = 0; i < size; i++) WinCount[i] = 0;
    }
    public float GetChanceOfWin(int idx)
    {
        if (idx < WinCount.Length) return ((1f + WinCount[idx]) / (WinCount.Length + TotalOfGames));
        else throw new System.ArgumentOutOfRangeException("Erro in GetChanceOfWin: Idx out of the range.");
    }
    public void CountAWin(int idx)
    {
        if (idx < WinCount.Length)
        {
            WinCount[idx]++;
            TotalOfGames++;
            SetOdds();
        }
        else throw new System.ArgumentOutOfRangeException("Erro in CountAWin: Argument Out of the range.");
    }
    private void SetOdds()
    {
        for(int i = 0; i < Odds.Length; i++)
        {
            Odds[i] = 1f / GetChanceOfWin(i);
        }
    }
    public float GetOdd(int oddindex)
    {
        if (oddindex < Odds.Length) return Odds[oddindex];
        else throw new System.ArgumentOutOfRangeException("Erro in GetOdd: Argument Out of the range");
    }
    public string Str()
    {
        string Out = "WinCount: [";
        for (int i = 0; i < WinCount.Length; i++) Out += WinCount[i].ToString() + ", ";
        Out += "]\nChances of Win: [";
        for (int i = 0; i < WinCount.Length; i++) Out += GetChanceOfWin(i).ToString() + ", ";
        Out += "]\nOdds: [";
        for (int i = 0; i < WinCount.Length; i++) Out += Odds[i].ToString() + ", ";
        Out += "]";
        return Out;
    }
}