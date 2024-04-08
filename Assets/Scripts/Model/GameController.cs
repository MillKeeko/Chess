using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public static string PlayerTag { get; private set; }
    public static string BotTag { get; private set; }
    public static string Turn { get; private set; }

    private int _pieceSetupCount = 0;
    private int _turnCount = 0;

    //  
    //  Events & Delegates
    //  
    public delegate void OnTurnStart();
    public static event OnTurnStart OnTurnStartEvent;
    public delegate void OnBotMove();
    public static event OnBotMove OnBotMoveEvent;
    //public delegate void OnChangeTurn(string turn);
    //public static event OnChangeTurn OnChangeTurnEvent;

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
        
        TrackingHandler.OnTrackerReadyEvent += TriggerPieceSetup;
        TrackingHandler.OnTrackerUpdatedEvent += TriggerPieceSetup;

        Piece.OnPieceSetupCompleteEvent += UpdatePieceSetupCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TriggerPieceSetup()
    {
        OnTurnStartEvent?.Invoke();
    }

    // THE BUG IS HERE
    // SETUP COUNT STOPPED AT 14 WHEN TOTALPIECES WAS 15
    // WHYYYYYYYYYYY
    private void UpdatePieceSetupCount()
    {   
        int totalPieces = TrackingHandler.TrackerCount;

        _pieceSetupCount++;
        //Debug.Log(totalPieces);
        //Debug.Log("Piece setup count " + _pieceSetupCount);
        //Debug.Log("Turn Count " + _turnCount);
        if (_pieceSetupCount >= totalPieces)
        {
            //Debug.Log("Updated all pieces");
            _pieceSetupCount = 0;
            if (_turnCount == 0) 
            {
                _turnCount++;
                CheckBotFirstTurn();
            }
            else ChangeTurn();
        }
    }

    private void CheckBotFirstTurn()
    {
        //Debug.Log("Gamecontroller checking bot first move.");
        if (Turn == BotTag)
        {
            //Debug.Log("Bot goes first.");
            OnBotMoveEvent?.Invoke();
        } 
    }

    private void ChangeTurn()
    {
        //Debug.Log("Changing Turn");
        Debug.Log("Turn was " + Turn);
        if (Turn == Constants.WHITE_TAG) Turn = Constants.BLACK_TAG;
        else Turn = Constants.WHITE_TAG;
        Debug.Log("Turn is " + Turn);
        _turnCount++;
        //OnChangeTurnEvent?.Invoke(Turn);
        if (Turn == BotTag) 
        {
            OnBotMoveEvent?.Invoke();
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
}
