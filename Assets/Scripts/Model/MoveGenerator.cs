using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Model

//  Purpose of this class is to be used both by GameController and BotController.
//  -   GameController will use it to generate possible move for the player
//  -   BotController will use it to search many moves into the future and return best move

public class MoveGenerator : MonoBehaviour
{
    public static List<PossibleMove> PossibleBotMovesList = new List<PossibleMove>();
    public static List<PossibleMove> PossiblePlayerMovesList = new List<PossibleMove>();
    public static List<PossibleMove> PossibleEnemyAttackList = new List<PossibleMove>();

    public delegate void AddDiagonalPawnAttacks();
    public static event AddDiagonalPawnAttacks AddDiagonalPawnAttacksEvent;

    //
    //  Public Methods
    //

    public static void SetupMoveLists()
    {
        // Reset Check flag after last move
        CheckHandler.IsInCheck = false;

        CreateTurnMoveList();
        CreateEnemyAttackList();

        // Check if in check after generated lists
        CheckHandler.SetIsInCheck();
        
        // Generate turn's move list again if in check to only allow moves that remove check
        CreateTurnMoveList();

        SquareHighlighter.SetSquaresDefault();
    }

    //  Input
    //      -   Piece[8,8], representation of the board
    //      -   String, tag represententing white or black
    //  Output
    //      -   List<PossibleMove>, all possible moves for either white or black
    //  Description
    //      -   Used to output a list of possible moves given a specific board position
    public static List<PossibleMove> CompilePossibleMoves(Piece[,] boardPosition, string tag)
    {
        List<Piece> pieceList = GeneratePieceList(boardPosition, tag);
        List<PossibleMove> possibleMoveList = new List<PossibleMove>();

        foreach (Piece piece in pieceList)
        {
            piece.GeneratePossibleMoves(boardPosition);
            possibleMoveList.AddRange(piece.PossibleMovesList);
        }

        //Debug.Log("PossibleMove count " + possibleMoveList.Count);
        return possibleMoveList;
    }

    public static void GenerateEnemyAttackList()
    {
        if (GameController.Turn == GameController.BotTag) 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.PlayerTag);
        }
        else 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.BotTag);
        }
        CreateEnemyAttackList();
    }

    //
    //  Private Methods
    //

    //  Input
    //      -   Piece[8,8], representation of the board
    //      -   String, tag represententing white or black
    //  Output
    //      -   List<Piece>, list of pieces on the board
    //  Description
    //      -   Used as a helper function for CompilePossibleMoves()
    private static List<Piece> GeneratePieceList(Piece[,] boardPosition, string tag)
    {
        List<Piece> pieceList = new List<Piece>();

        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                Piece piece = boardPosition[rank, file];
                if (piece != null && piece.CompareTag(tag))
                {
                    pieceList.Add(piece);
                }
            }
        }

        //Debug.Log("Piece list count " + pieceList.Count);
        return pieceList;
    } 

    private static void CreateTurnMoveList()
    {
        if (GameController.Turn == GameController.BotTag) 
        {
            PossibleBotMovesList.Clear();
            PossibleBotMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.BotTag);
            GameController.TurnMoveCount = PossibleBotMovesList.Count;
        }
        else 
        {
            PossiblePlayerMovesList.Clear();
            PossiblePlayerMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.PlayerTag);
            GameController.TurnMoveCount = PossiblePlayerMovesList.Count;
        }
    }

    private static void CreateEnemyAttackList()
    {
        if (GameController.Turn == GameController.BotTag) CopyMoveToAttackList(PossiblePlayerMovesList);
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

    private static void ClearAndCompileMoveLists()
    {
        PossibleBotMovesList.Clear();
        PossiblePlayerMovesList.Clear();
        PossibleBotMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.BotTag);
        PossiblePlayerMovesList = CompilePossibleMoves(TrackingHandler.pieceTracker, GameController.PlayerTag);
    }
}
