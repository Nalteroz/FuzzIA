using System;
using System.Collections.Generic;

public class House
{
    public Event EventPointer { get; private set; }
    public FuzzyController ControllerPointer { get; private set; }
    public List<List<Player>> Players { get; private set; }
    public List<List<OddList>> Odds { get; private set; }


    public House(Event Event, int nOfPlayers)
    {
        EventPointer = Event;
        ControllerPointer = Event.ControllerPointer;
        InitializePlayers(nOfPlayers);
    }

    public void InitializePlayers(int PlayersPerDomain)
    {
        Players = new List<List<Player>>();
        for(int DomainIdx = 0; DomainIdx < ControllerPointer.OutputDomainsList.Count; DomainIdx++)
        {
            List<Player> PlayerList = new List<Player>();
            for (int i = 0; i < PlayersPerDomain; i++)
            {
                PlayerList.Add(new Player(this, DomainIdx));
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
    public uint PossibilitieIdx { get; private set; }
    public float BetValue { get; private set; }

    public Bet(Player player, uint possibilitieidx, float value)
    {
        Player = player;
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