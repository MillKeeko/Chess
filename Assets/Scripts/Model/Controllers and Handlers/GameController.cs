using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Bugs
//  -   I could have taken a pawn with a queen while in check by the pawn but it only let me take with the king
//      -   I have recreated this, seems to only happen when in check by a pawn.
//  -   I've placed a pawn forward to the 6th rank and received a nullreferenceexception at UpdateTrackerEnPassant, the pawn stopped working
//  -   I've gotten a stalemate when I checkmated the bot on the 8th rank with a rook on the 7th and a rook on the 8th

public enum Pieces
{
    King,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn
}

public struct PossibleMove
{
    public Vector2 TargetPosition;
    public Piece SelectedPiece;

    // Constructor
    public PossibleMove (Vector2 targetPosition, Piece selectedPiece)
    {
        TargetPosition = targetPosition;
        SelectedPiece = selectedPiece;
    }
};

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public static string PlayerTag { get; private set; }
    public static string BotTag { get; private set; }
    public static string Turn { get; private set; }

    public static List<PossibleMove> PossibleBotMovesList;
    public static List<PossibleMove> PossiblePlayerMovesList;
    public static List<PossibleMove> PossibleEnemyAttackList;

    private int _turnMoveCount;

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
        PossibleBotMovesList = new List<PossibleMove>();
        PossiblePlayerMovesList = new List<PossibleMove>();
        PossibleEnemyAttackList = new List<PossibleMove>();
        _turnMoveCount = 0;
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
        // Reset Check flag after last move
        CheckHandler.SetIsInCheck();

        CreateTurnMoveList();
        CreateEnemyAttackList();

        // Check if in check after generated lists
        CheckHandler.SetIsInCheck();
        
        // Generate turn's move list again if in check to only allow moves that remove check
        CreateTurnMoveList();

        SquareHighlighter.SetSquaresDefault();

        //  Game Over Logic
        if ( _turnMoveCount == 0)
        {
            if (CheckHandler.IsInCheck)
            {
                Debug.Log("Checkmate");
            }
            else 
            {
                Debug.Log("Stalemate");
            }
        }
        else
        {
            //  Trigger bot move if bot's turn
            if (Turn == BotTag) OnBotMoveEvent?.Invoke();
        }
    }

    private void CreateTurnMoveList()
    {
        if (Turn == BotTag) 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = CompilePossibleMoves(BotTag);
            _turnMoveCount = PossibleBotMovesList.Count;
        }
        else 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = CompilePossibleMoves(PlayerTag);
            _turnMoveCount = PossiblePlayerMovesList.Count;
        }
    }

    public static void GenerateEnemyAttackList()
    {
        if (Turn == BotTag) 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = CompilePossibleMoves(PlayerTag);
        }
        else 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = CompilePossibleMoves(BotTag);
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

    private static void CopyMoveToAttackList(List<PossibleMove> moveList)
    {
        PossibleEnemyAttackList.Clear();
        foreach (PossibleMove move in moveList)
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
        PossibleBotMovesList = CompilePossibleMoves(BotTag);
        PossiblePlayerMovesList = CompilePossibleMoves(PlayerTag);
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

    public static List<PossibleMove> CompilePossibleMoves(string tag)
    {
        List<Piece> pieceList = GeneratePieceList(tag);
        List<PossibleMove> possibleMoveList = new List<PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            piece.GeneratePossibleMoves();
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("PossibleMove count " + possibleMoveList.Count);
        return possibleMoveList;
    }

    public static List<Piece> GeneratePieceList(string tag)
    {
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Piece piece = TrackingHandler.pieceTracker[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }
            }
        }

        //Debug.Log("Piece list count " + pieceList.Count);
        return pieceList;
    } 

    public static string GetEnemyTag()
    {
        string enemyTag = null;

        if (Turn == Constants.WHITE_TAG) enemyTag = Constants.BLACK_TAG;
        else enemyTag = Constants.WHITE_TAG;

        return enemyTag;    
    }
}
