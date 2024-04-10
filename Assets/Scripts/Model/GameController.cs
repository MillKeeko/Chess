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

    public static List<General.PossibleMove> PossibleBotMovesList;
    public static List<General.PossibleMove> PossiblePlayerMovesList;

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
        PossibleBotMovesList = new List<General.PossibleMove>();
        PossiblePlayerMovesList = new List<General.PossibleMove>();
        TrackingHandler.OnTrackerReadyEvent += StartTurn;
        TrackingHandler.OnTrackerUpdatedEvent += ChangeTurn;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public static void GenerateEnemyTestMovesList()
    {
        if (Turn == BotTag) 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = General.CompilePossibleMoves(PlayerTag);
        }
        else 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = General.CompilePossibleMoves(BotTag);
        }
    }

    private void StartTurn()
    {
        //  Generate moves for turn's pieces
        EmptyLists();
        CheckHandler.SetIsInCheck();
        PossibleBotMovesList = General.CompilePossibleMoves(BotTag);
        PossiblePlayerMovesList = General.CompilePossibleMoves(PlayerTag);
        CheckHandler.SetIsInCheck();

        if (CheckHandler.IsInCheck)
        {
            if (Turn == BotTag) 
            {
                PossibleBotMovesList.Clear();
                PossibleBotMovesList = General.CompilePossibleMoves(BotTag);
                Debug.Log(BotTag + " has " + PossibleBotMovesList.Count + " possible moves after check.");
            }
            else 
            {
                PossiblePlayerMovesList.Clear();
                PossiblePlayerMovesList = General.CompilePossibleMoves(PlayerTag);
                Debug.Log(BotTag + " has " + PossiblePlayerMovesList.Count + " possible moves after check.");
            }
        }
        
        //  Trigger bot move if bot's turn
        if (Turn == BotTag) 
        {
            OnBotMoveEvent?.Invoke();
        }
    }

    private void EmptyLists()
    {   
        PossibleBotMovesList.Clear();
        PossiblePlayerMovesList.Clear();
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
