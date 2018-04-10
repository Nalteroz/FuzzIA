using System.Collections;
using System.Collections.Generic;
using System;

public class Game
{

    public FuzzyController FzController { get; private set; }
    public Event Event { get; private set; }
    public House GameHouse { get; private set; }
    public EvaluationHandler EvaluationHdlr { get; private set; }
    public int TotalOfTurns { get; private set; }
    public int CurrentTurn { get; private set; }
    public bool GameFinish { get; private set; }

    Func<float> EvaluationFunction;


    public Game(FuzzyController controller, int nOfTurns, Func<float> evaluationFunction)
    {
        FzController = controller;
        TotalOfTurns = nOfTurns;
        CurrentTurn = 0;
        EvaluationFunction = evaluationFunction;
        GameFinish = false;
        Event = new Event(FzController);
        GameHouse = new House(Event);
        EvaluationHdlr = new EvaluationHandler(Event, EvaluationFunction);
	}

    public void RunCompleteGame()
    {
        for (CurrentTurn = 0; CurrentTurn < TotalOfTurns; )
        {
            PlayTurn();
        }
    }
	
    public void PlayTurn()
    {
        if(CurrentTurn < TotalOfTurns)
        {
            GameCicle();
            if(TotalOfTurns != -1)CurrentTurn++;
        }
        if (CurrentTurn == TotalOfTurns) GameFinish = true;
    }
    public void ResetTurnCount()
    {
        CurrentTurn = 0;
        GameFinish = false;
    }
    void GameCicle()
    {
        GameHouse.GetRecomendations();
        GameHouse.GetBets();
        Event.GetRecomendationsRules(GameHouse);
        EvaluationHdlr.StepEvaluation();
        UseWinnerRule();
        GameHouse.PayOdds(EvaluationHdlr);
        GameHouse.RenewPlayers();
    }

    void UseWinnerRule()
    {
        int winneridx = EvaluationHdlr.GetWinnerIdx();
        if (winneridx >= 0)
        {
            List<FuzzyRule> WinnerRules = Event.RecomendationRules[winneridx];
            List<FuzzyRule> CopyList = new List<FuzzyRule>();
            Event.ControllerPointer.AddRule(CopyList);
        }
    }
    
}
