using System;
using System.Collections.Generic;
using UnityEngine;

public class House
{
    public Event EventPointer { get; private set; }
    public FuzzyController ControllerPointer { get; private set; }
    public List<List<Player>> Players { get; private set; }
    public List<List<OddList>> Odds { get; private set; }
    public float MinRisk { get; private set; }


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
        string Out = "Players:\n";
        for (int i = 0; i < Players.Count; i++)
        {
            for (int j = 0; j < Players[i].Count; j++)
            {
                Out += "Player[" + i + "][" + j + "]";
                Out += Players[i][j].Str(); 
            }
        }
        Out += "Odds:\n";
        for (int i = 0; i < Odds.Count; i++)
        {
            for (int j = 0; j < Odds[i].Count; j++)
            {
                Out += "\nOdd [" + i + "][" + j + "]" + Odds[i][j].Str();
            }
        }
        return Out;
    }


    public void InitializePlayers(int PlayersPerDomain)
    {
        Players = new List<List<Player>>();
        for(int OutDomainIdx = 0; OutDomainIdx < ControllerPointer.OutputDomainsList.Count; OutDomainIdx++)
        {
            List<Player> PlayerList = new List<Player>();
            for (int i = 0; i < PlayersPerDomain; i++)
            {
                PlayerList.Add(new Player(this, ControllerPointer.OutputDomainsList[OutDomainIdx]));
            }
            Players.Add(PlayerList);
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
    
}

public class Bet
{
    public Player Player { get; private set; }
    public OutputSet OutSet { get; private set;}
    public int PossibilitieIdx { get; private set; }
    public float BetValue { get; private set; }

    public Bet(Player player, OutputSet setbetted, int possibilitieidx, float value)
    {
        Player = player;
        OutSet = setbetted;
        PossibilitieIdx = possibilitieidx;
        BetValue = value;
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