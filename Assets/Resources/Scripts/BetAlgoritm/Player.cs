using System.Collections;
using System.Collections.Generic;
using System;

public class Player
{
    public House HousePointer { get; private set; }
    public float Wallet { get; private set; }
    public List<List<Addiction>> Addictions { get; private set; }
    public List<List<int>> Recomendation { get; private set; }
    public bool isBroken { get; private set; }
    
    int WalletNumber;

    public Player(House house, float walletbankroll = 1500)
    {
        HousePointer = house;
        isBroken = false;
        WalletNumber = house.ControllerPointer.OutputDomainsList.Count;
        Wallet = walletbankroll;
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
        string Out = "Wallet: " + Wallet ;
        Out += "\nIsBroken: " + isBroken.ToString();
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
    public void RecievePayment(float montant)
    {
        Wallet += montant;
        if(montant > 0) isBroken = false;
    }
    public Bet CalculeRecomentationBet(int recomendationIdx)
    {
        float BetValue = 0;
        float[][] IndividualValues = new float[Addictions.Count][];
        for (int DomainIdx = 0; DomainIdx < Addictions.Count; DomainIdx++)
        {
            IndividualValues[DomainIdx] = new float[Addictions[DomainIdx].Count];
            for (int SetIdx = 0; SetIdx < Addictions[DomainIdx].Count; SetIdx++)
            {
                IndividualValues[DomainIdx][SetIdx] = CalculeBetValue(DomainIdx, SetIdx, HousePointer.TurnRecomendations[recomendationIdx][DomainIdx][SetIdx], HousePointer.Odds[DomainIdx][SetIdx]);
                BetValue += IndividualValues[DomainIdx][SetIdx];
            }
        }

        if (BetValue > 0) return new Bet(this, recomendationIdx, BetValue, IndividualValues);
        else return null;
    }
    float CalculeBetValue(int DomainIdx, int SetIdx, int PossibilitieIdx, OddList odds)
    {
        float ChanceOfWin = odds.GetChanceOfWin(PossibilitieIdx), CurrentBet = 0, PercentOfBankroll = 0;
        float CurrentAddiction = Addictions[DomainIdx][SetIdx].Tendings[PossibilitieIdx];
        if (CurrentAddiction >= ChanceOfWin && !isBroken)
        {
            PercentOfBankroll = ((CurrentAddiction * odds.Odds[PossibilitieIdx] - 1) / (odds.Odds[PossibilitieIdx] - 1)); //Critério de kelly.
            CurrentBet = (Wallet / WalletNumber) * PercentOfBankroll;
            CurrentBet = Math.Min(Wallet, Math.Max(HousePointer.MinRisk, CurrentBet));
            Wallet -= CurrentBet;
            if (Wallet <= 0) isBroken = true;
        }
        return CurrentBet;
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
