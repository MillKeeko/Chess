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
    public static List<General.PossibleMove> PossibleEnemyAttackList;

    //  
    //  Events & Delegates
    //  
    public delegate void OnBotMove();
    public static event OnBotMove OnBotMoveEvent;
    public delegate void AddDiagonalPawnAttacks();
    public static event AddDiagonalPawnAttacks AddDiagonalPawnAttacksEvent;

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
        PossibleEnemyAttackList = new List<General.PossibleMove>();
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

    private void StartTurn()
    {
        // Reset Check flag after last move
        CheckHandler.SetIsInCheck();

        CreateTurnMoveList();
        CreateEnemyAttackList();

        // Check if in check after generated lists
        CheckHandler.SetIsInCheck();
        
        // Generate turn's move list again if in check to only allow moves that remove check
        CreateTurnMoveList();

        //SquareHighlighter.ShowAttackingSquares(PossibleEnemyAttackList);

        //  Trigger bot move if bot's turn
        if (Turn == BotTag) OnBotMoveEvent?.Invoke();
    }

    private void CreateTurnMoveList()
    {
        if (Turn == BotTag) 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = General.CompilePossibleMoves(BotTag);
        }
        else 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = General.CompilePossibleMoves(PlayerTag);
        }
    }

    public static void GenerateEnemyAttackList()
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
         CreateEnemyAttackList();
    }

    private static void CreateEnemyAttackList()
    {
        if (Turn == BotTag) CopyMoveToAttackList(PossiblePlayerMovesList);
        else CopyMoveToAttackList(PossibleBotMovesList);
        FilterAttackList();
        AddDiagonalPawnAttacksEvent?.Invoke();
    }

    private static void CopyMoveToAttackList(List<General.PossibleMove> moveList)
    {
        PossibleEnemyAttackList.Clear();
        foreach (General.PossibleMove move in moveList)
        {
            PossibleEnemyAttackList.Add(move);
        }
    }

    //  Just filtering out the pawn forward moves which cannot attack
    private static void FilterAttackList()
    {
        int listCount = PossibleEnemyAttackList.Count;
        for (int i = 0; i < PossibleEnemyAttackList.Count; i++)
        {
            if (PossibleEnemyAttackList[i].SelectedPiece is Pawn && 
                PossibleEnemyAttackList[i].TargetPosition.x == PossibleEnemyAttackList[i].SelectedPiece.Position.x)
            {
                PossibleEnemyAttackList.RemoveAt(i);
                i--;
                listCount--;
            }
        }
    }

    private void ClearAndCompileMoveLists()
    {
        PossibleBotMovesList.Clear();
        PossiblePlayerMovesList.Clear();
        PossibleBotMovesList = General.CompilePossibleMoves(BotTag);
        PossiblePlayerMovesList = General.CompilePossibleMoves(PlayerTag);
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
