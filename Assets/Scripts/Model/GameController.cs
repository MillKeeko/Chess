using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Bugs
//  -   I could have taken a pawn with a queen while in check by the pawn but it only let me take with the king
//      -   I have recreated this, seems to only happen when in check by a pawn.
//  -   I've placed a pawn forward to the 6th rank and received a nullreferenceexception at UpdateTrackerEnPassant, the pawn stopped working
//  -   I've gotten a stalemate when I checkmated the bot on the 8th rank with a rook on the 7th and a rook on the 8th
//  -   Bot cannot promote itself, player has to choose it's promotion.
//  -   When bot castles queenside it will put the king in the B file. Not sure about Kingside yet. Doesn't happen for player.

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public static string PlayerTag { get; private set; }
    public static string BotTag { get; private set; }
    public static string Turn { get; private set; }
    
    public static int TurnMoveCount;

    //  
    //  Events & Delegates
    //  
    public delegate void OnBotMove();
    public static event OnBotMove OnBotMoveEvent;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        Turn = Constants.WHITE_TAG;
        RandomTeams();
        
        TurnMoveCount = 0;
        TrackingHandler.OnTrackerReadyEvent += StartTurn;
        TrackingHandler.OnTrackerUpdatedEvent += ChangeTurn;
    }

    private void ChangeTurn()
    {
        if (Turn == Constants.WHITE_TAG) 
        {
            //Debug.Log("Starting Black Turn");
            Turn = Constants.BLACK_TAG;
        }
        else 
        {
            Turn = Constants.WHITE_TAG;
            //Debug.Log("Starting White Turn");
        }
        StartTurn();
    }

    private void StartTurn()
    {
        MoveGenerator.SetupMoveLists();
    
        //  Game Over Logic
        if (TurnMoveCount == 0)
        {
            GameOver();
        }
        else
        {
            //  Trigger bot move if bot's turn, else do nothing just wait for player move
            if (Turn == BotTag) OnBotMoveEvent?.Invoke();
        }
    }

    private void GameOver()
    {
        Debug.Log("Turn = " + Turn + " and in check? " + CheckHandler.IsInCheck);
        if (CheckHandler.IsInCheck)
        {
            Debug.Log("Checkmate");
        }
        else 
        {
            Debug.Log("Stalemate");
        }
    }   

    private void RandomTeams()
    {
        int playerSide = Random.Range(0,2); 
        if (playerSide == 0) 
        {   
            PlayerTag = Constants.WHITE_TAG;
            BotTag = Constants.BLACK_TAG;
        }
        else if (playerSide == 1) 
        {
            PlayerTag = Constants.BLACK_TAG;
            BotTag = Constants.WHITE_TAG;
        }
    }

    public static string GetEnemyTag()
    {
        string enemyTag = null;

        if (Turn == Constants.WHITE_TAG) enemyTag = Constants.BLACK_TAG;
        else enemyTag = Constants.WHITE_TAG;

        return enemyTag;    
    }
}
