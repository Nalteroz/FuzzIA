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
    public List<List<List<int>>> TurnRecomendations { get; private set; }
    public List<List<List<Bet>>> TurnBets { get; private set; }


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
        string Out = "N of players per domain: " + Players[0].Count;
        Out += "Players:\n";
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
        if (TurnRecomendations != null)
        {
            Out += "\nRecomendations:";
            for (int i = 0; i < TurnRecomendations.Count; i++)
            {
                for (int j = 0; j < TurnRecomendations[i].Count; j++)
                {
                    Out += "\n[" + i + "][" + j + "]{ ";
                    for (int k = 0; k < TurnRecomendations[i][j].Count; k++)
                    {
                        Out += TurnRecomendations[i][j][k] + ", ";
                    }
                    Out += "}";
                }
            }
        }
        if (TurnBets != null)
        {
            Out += "\nBets:";
            for (int i = 0; i < TurnBets.Count; i++)
            {
                for (int j = 0; j < TurnBets[i].Count; j++)
                {
                    Out += "\n[" + i + "][" + j + "]{ ";
                    for (int k = 0; k < TurnBets[i][j].Count; k++)
                    {
                        Out += TurnBets[i][j][k].Str();
                    }
                    Out += "}";
                }
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
    public void GetRecomendations()
    {
        TurnRecomendations = new List<List<List<int>>>();
        int Recomentation = -1;
        for (int DomainIndex = 0; DomainIndex < ControllerPointer.OutputDomainsList.Count; DomainIndex++)
        {
            List<List<int>> DomainRecomentations = new List<List<int>>();
            for (int SetIndex = 0; SetIndex < ControllerPointer.OutputDomainsList[DomainIndex].Sets.Count; SetIndex++)
            {
                List<int> SetRecomendations = new List<int>();
                foreach (Player player in Players[DomainIndex])
                {
                    Recomentation = player.Recomend(SetIndex);
                    if (!SetRecomendations.Contains(Recomentation)) SetRecomendations.Add(Recomentation);
                }
                DomainRecomentations.Add(SetRecomendations);
            }
            TurnRecomendations.Add(DomainRecomentations);
        }
    }
    public void GetBets()
    {
        TurnBets = new List<List<List<Bet>>>();
        for (int DomainIndex = 0; DomainIndex < ControllerPointer.OutputDomainsList.Count; DomainIndex++)
        {
            List<List<Bet>> DomainBets = new List<List<Bet>>();
            for (int SetIndex = 0; SetIndex < ControllerPointer.OutputDomainsList[DomainIndex].Sets.Count; SetIndex++)
            {
                List<Bet> SetBets = new List<Bet>();
                foreach (Player player in Players[DomainIndex])
                {
                    SetBets.AddRange(player.MakeBets(TurnRecomendations[DomainIndex][SetIndex], SetIndex, Odds[DomainIndex][SetIndex]));
                }
                DomainBets.Add(SetBets);
            }
            TurnBets.Add(DomainBets);
        }
    }
    
}

public class Bet
{
    public Player Player { get; private set; }
    public int PossibilitieIdx { get; private set; }
    public float BetValue { get; private set; }

    public Bet(Player player, int possibilitieidx, float value)
    {
        Player = player;
        PossibilitieIdx = possibilitieidx;
        BetValue = value;
    }

    public string Str()
    {
        string Out = "\nPlayer: " + Player.Str();
        Out += "Possibilitie index: " + PossibilitieIdx;
        Out += "\nBet value: " + BetValue + "\n";
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