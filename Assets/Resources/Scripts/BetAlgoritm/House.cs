using System;
using System.Collections.Generic;

public class House
{
    public Event EventPointer { get; private set; }
    public float[] Odds { get; private set; }
    public List<Player> Players { get; private set; }

    public House(Event Event)
    {
        EventPointer = Event;
        Odds = new float[Event.Possibilities.Count];
        SetOdds();
    }

    public void SetOdds()
    {
        for(int i =0; i < EventPointer.Possibilities.Count; i++)
        {
            Odds[i] = 1f / EventPointer.GetProbabilitieOfWin(i);
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